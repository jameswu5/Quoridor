using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    /// <summary>
    /// Displays the empty board
    /// </summary>
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

    /// <summary>
    /// Displays the things placed on the board (i.e. placed players and placed walls)
    /// </summary>
    public void DisplayState()
    {
        // Players
        for (int i = 0; i < NumOfPlayers; i++)
        {
            Coord coord = players[i].position;
            Coord topLeft = GetTopLeftCoord(coord);
            DrawCircle(topLeft.x + SquareSize / 2, topLeft.y + SquareSize / 2, PlayerRadius, players[i].colour);
        }
    }

    public static Coord GetTopLeftCoord(int x, int y)
    {
        return new Coord(BoardPaddingX + x * (SquareSize + WallWidth), BoardPaddingY + (BoardSize - 1 - y) * (SquareSize + WallWidth));
    }

    public static Coord GetTopLeftCoord(Coord coord) => GetTopLeftCoord(coord.x, coord.y);
}