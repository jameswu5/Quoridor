using System;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    private static readonly int[] DIR = new int[] {0, 1, 0, -1, 0};

    private List<Player> players;
    private List<Wall> walls;

    public Board()
    {
        players = new List<Player>();
        walls = new List<Wall>();
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
            if (CheckInBounds(newCoord) && CheckSquareOccupied(newCoord) == -1)
            {
                legalSquares.Add(newCoord);
            }
        }

        return legalSquares;
    }

    private static bool CheckInBounds(Coord coord) => coord.x >= 0 && coord.x < BoardSize && coord.y >= 0 && coord.y < BoardSize;


    public void MakeMove(int playerIndex, Coord newSquare)
    {
        players[playerIndex].position = newSquare;
    }
}