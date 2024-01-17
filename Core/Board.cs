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
        NewGame();
    }

    private void NewGame()
    {
        Array.Clear(playerPositions);
        walls.Clear();

        int index = BoardSize >> 1;
        playerPositions[0] = new Coord(index, 0);
        playerPositions[1] = new Coord(index, BoardSize - 1);
        if (NumOfPlayers == 4)
        {
            playerPositions[2] = new Coord(0, index);
            playerPositions[3] = new Coord(BoardSize - 1, index);
        }
    }
}