using System;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    private static readonly int[] DIR = new int[] {0, 1, 0, -1, 0};

    public List<Player> players;
    public List<Wall> walls;

    public bool[,] validWallsHor;
    public bool[,] validWallsVer;

    public Board()
    {
        players = new List<Player>();
        walls = new List<Wall>();
        validWallsHor = new bool[BoardSize - 1, BoardSize - 1];
        validWallsVer = new bool[BoardSize - 1, BoardSize - 1];
        InitialiseUI();
        NewGame();
    }

    public void NewGame()
    {
        players.Clear();
        walls.Clear();

        int index = BoardSize >> 1;
        players.Add(new Player(new Coord(index, 0), PlayerColours[0]));
        players.Add(new Player(new Coord(index, BoardSize - 1), PlayerColours[1]));
        if (NumOfPlayers == 4)
        {
            players.Add(new Player(new Coord(0, index), PlayerColours[2]));
            players.Add(new Player(new Coord(BoardSize - 1, index), PlayerColours[3]));
        }

        for (int i = 0; i < BoardSize - 1; i++)
        {
            for (int j = 0; j < BoardSize - 1; j++)
            {
                validWallsHor[i, j] = true;
                validWallsVer[i, j] = true;
            }
        }
    }

    /// <summary>
    /// Returns the Player number if it's occupied, -1 otherwise
    /// </summary>
    /// <returns></returns>
    public int CheckSquareOccupied(Coord coord)
    {
        for (int i = 0; i < NumOfPlayers; i++)
        {
            if (players[i].position == coord)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Return the legal squares a player can move to
    /// </summary>
    /// <returns></returns>
    public List<Coord> GetLegalSquares(int playerIndex)
    {
        List<Coord> legalSquares = new();
        Coord coord = players[playerIndex].position;

        for (int d = 0; d < 4; d++)
        {
            Coord newCoord = new Coord(coord.x + DIR[d], coord.y + DIR[d+1]);

            if (CheckSquareOccupied(newCoord) == -1)
            {
                if (CheckInBounds(newCoord))
                {
                    legalSquares.Add(newCoord);
                }
            }
            else
            {
                Coord newCoord2 = new Coord(newCoord.x + DIR[d], newCoord.y + DIR[d+1]);
                if (CheckInBounds(newCoord2) && CheckSquareOccupied(newCoord2) == -1)
                {
                    legalSquares.Add(newCoord2);
                }
            }
        }

        return legalSquares;
    }

    private static bool CheckInBounds(Coord coord) => coord.x >= 0 && coord.x < BoardSize && coord.y >= 0 && coord.y < BoardSize;


    public void MakeMove(int playerIndex, Coord newSquare, Wall? wall)
    {
        if (wall == null)
        {
            players[playerIndex].position = newSquare;
        }
        else
        {
            PlaceWall(wall);
        }
    }

    public void PlaceWall(Wall wall)
    {
        walls.Add(wall);
        int i = wall.x;
        int j = wall.y;
        if (wall.isHorizontal)
        {
            validWallsVer[i, j] = false;
            validWallsHor[i, j] = false;
            validWallsHor[Math.Min(i + 1, BoardSize - 2), j] = false;
            validWallsHor[Math.Max(0, i-1), j] = false;
        }
        else
        {
            validWallsHor[i, j] = false;
            validWallsVer[i, j] = false;
            validWallsVer[i, Math.Min(j + 1, BoardSize - 2)] = false;
            validWallsVer[i, Math.Max(0, j - 1)] = false;
        }
    }
}