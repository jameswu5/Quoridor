using System;
using Raylib_cs;
using static Quoridor.Settings.Board;
using static Quoridor.Move;

namespace Quoridor;

public partial class Board
{
    public enum PlayerType {Human, RandomBot, MoveBot};

    private static readonly int[] DIR = new int[] {0, 1, 0, -1, 0};

    public List<Player> players;
    public Stack<Wall> walls;

    public bool[,] validWallsHor;
    public bool[,] validWallsVer;
    public int[,] validMoves; // Each square will be 0b____ : up, right, down, left

    public bool[,] placedWallsHor;
    public bool[,] placedWallsVer;

    public int turn;
    public bool gameOver;

    public List<int> legalMoves;
    private Stack<List<int>> moveCache;

    public Stack<int> gameMoves;

    // Reusable buffers for wall path-validation, sized once to avoid per-check allocation.
    // pathVisited stores a search id per square rather than being cleared each search.
    private int[,] pathVisited;
    private int pathSearchId;
    private int[] pathStack;

    public static readonly int[] dirMask = new int[] {0b1000, 0b0100, 0b0010, 0b0001};

    public event System.Action<int> PlayMove;

    public Board()
    {
        players = new List<Player>();
        walls = new Stack<Wall>();
        validWallsHor = new bool[BoardSize - 1, BoardSize - 1];
        validWallsVer = new bool[BoardSize - 1, BoardSize - 1];
        placedWallsHor = new bool[BoardSize - 1, BoardSize - 1];
        placedWallsVer = new bool[BoardSize - 1, BoardSize - 1];
        validMoves = new int[BoardSize, BoardSize];
        turn = 0;
        gameOver = false;
        legalMoves = new List<int>();
        moveCache = new Stack<List<int>>();
        gameMoves = new Stack<int>();
        pathVisited = new int[BoardSize, BoardSize];
        pathStack = new int[BoardSize * BoardSize];
        pathSearchId = 0;
        InitialiseUI();
        NewGame();
    }

    // This is run every frame
    public void Update()
    {
        Display();
        players[turn].Update();
    }

    public void NewGame()
    {
        players.Clear();
        walls.Clear();

        int index = BoardSize >> 1;

        players.Add(CreatePlayer(PlayerTypes[0], 0, new Coord(index, 0), PlayerColours[0], new Coord(-1, BoardSize - 1)));
        players.Add(CreatePlayer(PlayerTypes[1], 1, new Coord(index, BoardSize - 1), PlayerColours[1], new Coord(-1, 0)));
        if (NumOfPlayers == 4)
        {
            players.Add(CreatePlayer(PlayerTypes[2], 2, new Coord(0, index), PlayerColours[2], new Coord(BoardSize - 1, -1)));
            players.Add(CreatePlayer(PlayerTypes[3], 3, new Coord(BoardSize - 1, index), PlayerColours[3], new Coord(0, -1)));
        }

        for (int i = 0; i < BoardSize - 1; i++)
        {
            for (int j = 0; j < BoardSize - 1; j++)
            {
                validWallsHor[i, j] = true;
                validWallsVer[i, j] = true;
                placedWallsHor[i, j] = false;
                placedWallsVer[i, j] = false;
            }
        }

        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                validMoves[i, j] = 0b1111;
            }
        }

        turn = 0;
        gameOver = false;
        legalMoves = GetLegalMoves(turn);
        moveCache.Clear();
        moveCache.Push(new List<int>(legalMoves));
        gameMoves.Clear();
        players[turn].TurnToMove();
    }

    public void GameOver()
    {
        legalMoves.Clear();
        gameOver = true;
    }

    public bool CheckSquareOccupied(Coord coord)
    {
        for (int i = 0; i < NumOfPlayers; i++)
        {
            if (players[i].position == coord)
            {
                return true;
            }
        }
        return false;
    }

    public List<Coord> GetLegalSquares(int playerIndex)
    {
        List<Coord> legalSquares = new();
        Coord coord = players[playerIndex].position;

        for (int d = 0; d < 4; d++)
        {
            // If there is a wall directly in front, no need to execute the rest
            if ((validMoves[coord.x, coord.y] & dirMask[d]) == 0)
            {
                continue;
            }

            Coord newCoord = new Coord(coord.x + DIR[d], coord.y + DIR[d+1]);

            if (!CheckSquareOccupied(newCoord))
            {
                if (CheckInBounds(newCoord))
                {
                    legalSquares.Add(newCoord);
                }
            }
            else
            {
                // If there is a wall behind the new player, check the two perpendicular directions
                if ((validMoves[newCoord.x, newCoord.y] & dirMask[d]) == 0)
                {
                    int d1 = (d + 3) % 4;
                    int d2 = (d + 1) % 4;

                    if ((validMoves[newCoord.x, newCoord.y] & dirMask[d1]) > 0)
                    {
                        Coord newCoord2 = new Coord(newCoord.x + DIR[d1], newCoord.y + DIR[d1 + 1]);
                        if (CheckInBounds(newCoord2) && !CheckSquareOccupied(newCoord2))
                        {
                            legalSquares.Add(newCoord2);
                        }
                    }
                    if ((validMoves[newCoord.x, newCoord.y] & dirMask[d2]) > 0)
                    {
                        Coord newCoord2 = new Coord(newCoord.x + DIR[d2], newCoord.y + DIR[d2 + 1]);
                        if (CheckInBounds(newCoord2) && !CheckSquareOccupied(newCoord2))
                        {
                            legalSquares.Add(newCoord2);
                        }
                    }
                }
                else
                {
                    Coord newCoord2 = new Coord(newCoord.x + DIR[d], newCoord.y + DIR[d+1]);
                    if (CheckInBounds(newCoord2) && !CheckSquareOccupied(newCoord2))
                    {
                        legalSquares.Add(newCoord2);
                    }
                }
            }
        }
        return legalSquares;
    }

    private static bool CheckInBounds(Coord coord) => coord.x >= 0 && coord.x < BoardSize && coord.y >= 0 && coord.y < BoardSize;

    public void MakeMove(int move)
    {
        if (IsWall(move))
        {
            PlaceWall(RetrieveWall(move));
            players[turn].wallsLeft--;
        }
        else
        {
            players[turn].position = RetrieveTargetCoord(move);
        }

        if (players[turn].ReachedGoal())
        {
            GameOver();
            return;
        }

        turn = (turn + 1) % NumOfPlayers;
        legalMoves = GetLegalMoves(turn);
        moveCache.Push(new List<int>(legalMoves));
        gameMoves.Push(move);
    }

    // Undo the most recent move
    public void UndoMove()
    {
        // If there is no move to pop, then do nothing
        if (gameMoves.Count == 0)
        {
            return;
        }

        turn = (turn + 3) % NumOfPlayers;

        int move = gameMoves.Pop();
        if (IsWall(move))
        {
            // We assume the wall associated with the move is the same as the most recent wall pushed onto the list
            PopWall();
            players[turn].wallsLeft++;
        }
        else
        {
            players[turn].position = RetrieveStartCoord(move);
        }

        moveCache.Pop();
        legalMoves = moveCache.Peek();
    }

    public void PlaceWall(Wall wall)
    {
        walls.Push(wall);
        int i = wall.x;
        int j = wall.y;

        validWallsVer[i, j] = false;
        validWallsHor[i, j] = false;

        if (wall.isHorizontal)
        {
            validWallsHor[Math.Min(i+1, BoardSize-2), j] = false;
            validWallsHor[Math.Max(0, i-1), j] = false;

            // Cannot move up
            validMoves[i  , j] &= ~dirMask[0];
            validMoves[i+1, j] &= ~dirMask[0];
            // Cannot move down
            validMoves[i  , j+1] &= ~dirMask[2];
            validMoves[i+1, j+1] &= ~dirMask[2];

            placedWallsHor[i, j] = true;
        }
        else
        {
            validWallsVer[i, Math.Min(j+1, BoardSize-2)] = false;
            validWallsVer[i, Math.Max(0, j-1)] = false;

            // Cannot move right
            validMoves[i, j  ] &= ~dirMask[1];
            validMoves[i, j+1] &= ~dirMask[1];
            // Cannot move left
            validMoves[i+1, j  ] &= ~dirMask[3];
            validMoves[i+1, j+1] &= ~dirMask[3];

            placedWallsVer[i, j] = true;
        }
    }

    public void PopWall()
    {
        // We assume this is the MOST RECENT wall placed as it's only called from UndoMove, which unmakes the most recent move
        // We also need to be careful since placing a wall can set a false -> false, so setting it to true without checking is incorrect
        // Simplest solution is keeping track of each game state in a stack and recovering, but that is inefficient

        Wall wall = walls.Pop();
        int i = wall.x;
        int j = wall.y;

        if (wall.isHorizontal)
        {
            placedWallsHor[i, j] = false;
            validWallsHor[i, j] = true;

            if (!placedWallsVer[i, Math.Min(j+1, BoardSize-2)] && !placedWallsVer[i, Math.Max(j-1, 0)])
            {
                validWallsVer[i, j] = true;
            }
            if (!placedWallsVer[Math.Min(i+1, BoardSize-2), j] && !placedWallsHor[Math.Min(i+2, BoardSize-2), j])
            {
                validWallsHor[Math.Min(i+1, BoardSize-2), j] = true;
            }
            if (!placedWallsVer[Math.Max(i-1, 0), j] && !placedWallsHor[Math.Max(i-2, 0), j])
            {
                validWallsHor[Math.Max(0, i-1), j] = true;
            }
            
            // Can move up
            validMoves[i  , j] |= dirMask[0];
            validMoves[i+1, j] |= dirMask[0];
            // Can move down
            validMoves[i  , j+1] |= dirMask[2];
            validMoves[i+1, j+1] |= dirMask[2];
        }
        else
        {
            placedWallsVer[i, j] = false;
            validWallsVer[i, j] = true;

            if (!placedWallsHor[Math.Min(i+1, BoardSize-2), j] && !placedWallsHor[Math.Max(i-1, 0), j])
            {
                validWallsHor[i, j] = true;
            }
            if (!placedWallsHor[i, Math.Min(j+1, BoardSize-2)] && !placedWallsVer[i, Math.Min(j+2, BoardSize-2)])
            {
                validWallsVer[i, Math.Min(j+1, BoardSize-2)] = true;
            }
            if (!placedWallsHor[i, Math.Max(j-1, 0)] && !placedWallsVer[i, Math.Max(j-2, 0)])
            {
                validWallsVer[i, Math.Max(0, j-1)] = true;
            }

            // Can move right
            validMoves[i, j  ] |= dirMask[1];
            validMoves[i, j+1] |= dirMask[1];
            // Can move left
            validMoves[i+1, j  ] |= dirMask[3];
            validMoves[i+1, j+1] |= dirMask[3];
        }
    }

    public Player CreatePlayer(PlayerType playerType, int ID, Coord startPos, Color colour, Coord goal)
    {
        return playerType == PlayerType.Human ? CreateHuman(ID, startPos, colour, goal) : CreateBot(playerType, ID, startPos, colour, goal);
    }

    public Human CreateHuman(int ID, Coord startPos, Color colour, Coord goal)
    {
        Human human = new Human(this, ID, startPos, colour, goal);
        human.PlayChosenMove += PlayMove;
        return human;
    }

    public Bot CreateBot(PlayerType botType, int ID, Coord startPos, Color colour, Coord goal)
    {
        Bot bot = Bot.CreateBot(botType, this, ID, startPos, colour, goal);
        bot.PlayChosenMove += PlayMove;
        return bot;
    }

    public List<int> GetLegalMoves(int playerIndex)
    {
        List<int> res = new();
        List<Coord> legalSquares = GetLegalSquares(playerIndex);

        foreach (Coord coord in legalSquares)
        {
            res.Add(GenerateMove(players[playerIndex].position, coord));
        }
        
        if (players[playerIndex].wallsLeft == 0)
        {
            return res;
        }

        for (int i = 0; i < BoardSize - 1; i++)
        {
            for (int j = 0; j < BoardSize - 1; j++)
            {
                if (validWallsHor[i, j] && !WallBlocksAnyPlayer(i, j, true))
                {
                    res.Add(GenerateMove(new Wall(i, j, true)));
                }
                if (validWallsVer[i, j] && !WallBlocksAnyPlayer(i, j, false))
                {
                    res.Add(GenerateMove(new Wall(i, j, false)));
                }
            }
        }

        return res;
    }

    /// <summary>
    /// Returns true if placing the wall at (i, j) would leave at least one player
    /// with no path to their goal (an illegal placement in Quoridor).
    /// The wall's four blocked edges are applied to validMoves, every player is
    /// path-checked, then the edges are restored. validMoves is the natural
    /// adjacency graph here: it already encodes wall blocking and ignores pawns,
    /// which is exactly what the path-existence rule requires.
    /// </summary>
    private bool WallBlocksAnyPlayer(int i, int j, bool isHorizontal)
    {
        // Save and clear the four edges this wall removes. Saving the whole int
        // (rather than OR-ing back) keeps restore correct regardless of prior state.
        int a, b, c, d;
        if (isHorizontal)
        {
            a = validMoves[i  , j  ]; validMoves[i  , j  ] &= ~dirMask[0]; // up
            b = validMoves[i+1, j  ]; validMoves[i+1, j  ] &= ~dirMask[0]; // up
            c = validMoves[i  , j+1]; validMoves[i  , j+1] &= ~dirMask[2]; // down
            d = validMoves[i+1, j+1]; validMoves[i+1, j+1] &= ~dirMask[2]; // down
        }
        else
        {
            a = validMoves[i  , j  ]; validMoves[i  , j  ] &= ~dirMask[1]; // right
            b = validMoves[i  , j+1]; validMoves[i  , j+1] &= ~dirMask[1]; // right
            c = validMoves[i+1, j  ]; validMoves[i+1, j  ] &= ~dirMask[3]; // left
            d = validMoves[i+1, j+1]; validMoves[i+1, j+1] &= ~dirMask[3]; // left
        }

        bool blocks = false;
        for (int p = 0; p < NumOfPlayers; p++)
        {
            if (!HasPathToGoal(p))
            {
                blocks = true;
                break;
            }
        }

        if (isHorizontal)
        {
            validMoves[i  , j  ] = a;
            validMoves[i+1, j  ] = b;
            validMoves[i  , j+1] = c;
            validMoves[i+1, j+1] = d;
        }
        else
        {
            validMoves[i  , j  ] = a;
            validMoves[i  , j+1] = b;
            validMoves[i+1, j  ] = c;
            validMoves[i+1, j+1] = d;
        }

        return blocks;
    }

    /// <summary>
    /// Allocation-free BFS/DFS over the validMoves graph from a player's current
    /// square to its goal edge. Uses a per-call search id in pathVisited so the
    /// visited grid never needs clearing.
    /// </summary>
    private bool HasPathToGoal(int playerIndex)
    {
        Coord start = players[playerIndex].position;
        Coord goal = players[playerIndex].goal;

        pathSearchId++;
        int top = 0;
        pathStack[top++] = start.x * BoardSize + start.y;
        pathVisited[start.x, start.y] = pathSearchId;

        while (top > 0)
        {
            int encoded = pathStack[--top];
            int x = encoded / BoardSize;
            int y = encoded % BoardSize;

            // Goal has -1 on one axis (a whole edge); matching the other axis wins.
            if (x == goal.x || y == goal.y)
            {
                return true;
            }

            int mask = validMoves[x, y];
            for (int dir = 0; dir < 4; dir++)
            {
                if ((mask & dirMask[dir]) == 0)
                {
                    continue;
                }

                int nx = x + DIR[dir];
                int ny = y + DIR[dir + 1];

                // validMoves does not encode board edges, so bounds-check here.
                if (nx < 0 || nx >= BoardSize || ny < 0 || ny >= BoardSize)
                {
                    continue;
                }
                if (pathVisited[nx, ny] == pathSearchId)
                {
                    continue;
                }

                pathVisited[nx, ny] = pathSearchId;
                pathStack[top++] = nx * BoardSize + ny;
            }
        }

        return false;
    }

    public List<Coord> GetLegalSquareMoves()
    {
        List<Coord> res = new();
        foreach (int move in legalMoves)
        {
            if (!IsWall(move))
            {
                res.Add(RetrieveTargetCoord(move));
            }
        }
        return res;
    }
}