using System;

namespace Quoridor;

public partial class Board
{
    public const int BoardSize = 9;
    public const int NumOfPlayers = 4;

    private Coord[] playerPositions;
    private List<Wall> walls;

    public Board()
    {
        NewGame();
    }

    private void NewGame()
    {
        Array.Clear(playerPositions);
        walls.Clear();
    }
}