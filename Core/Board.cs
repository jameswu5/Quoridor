using System;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    private Coord[] playerPositions;
    private List<Wall> walls;

    public Board()
    {
        playerPositions = new Coord[NumOfPlayers];
        walls = new List<Wall>();
    }

    private void NewGame()
    {
        Array.Clear(playerPositions);
        walls.Clear();
    }
}