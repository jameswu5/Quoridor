using System;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    private List<Player> players;
    private List<Wall> walls;

    public Board()
    {
        players = new List<Player>();
        walls = new List<Wall>();
        NewGame();
    }

    private void NewGame()
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
}