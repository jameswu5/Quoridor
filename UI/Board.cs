using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    public void Display()
    {
        // Border
        DrawRectangle(
            BoardPaddingX - BorderWidth,
            BoardPaddingY - BorderWidth,
            BorderWidth * 2 + BoardSideLength,
            BorderWidth * 2 + BoardSideLength,
            BorderColour
        );
        
        // Squares
        DrawRectangle(BoardPaddingX, BoardPaddingY, BoardSideLength, BoardSideLength, BoardColour);

        // Walls
        for (int i = 0; i < BoardSize - 1; i++)
        {
            DrawRectangle(BoardPaddingX, BoardPaddingY + SquareSize + i * (SquareSize + WallWidth), BoardSideLength, WallWidth, WallColour);
            DrawRectangle(BoardPaddingX + SquareSize + i * (SquareSize + WallWidth), BoardPaddingY, WallWidth, BoardSideLength, WallColour);
        }

    }
}