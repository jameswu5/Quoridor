using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Wall
{
    private int displayWidth;
    private int displayHeight;

    private void InitialiseUI()
    {
        displayWidth = isHorizontal ? 2 * SquareSize + WallWidth : WallWidth;
        displayHeight = isHorizontal ? WallWidth : 2 * SquareSize + WallWidth;
    }

    public void Display()
    {
        DrawRectangle(x, y, displayWidth, displayHeight, PlacedWallColour);
    }
}