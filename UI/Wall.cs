using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Wall
{
    private int posX;
    private int posY;
    private int displayWidth;
    private int displayHeight;

    private void InitialiseUI()
    {
        posX = BoardPaddingX + x * (SquareSize + WallWidth);
        if (!isHorizontal)
        {
            posX += SquareSize;
        }

        posY = BoardPaddingY + (BoardSize - y - 2) * (SquareSize + WallWidth);
        if (isHorizontal)
        {
            posY += SquareSize;
        }

        displayWidth = isHorizontal ? 2 * SquareSize + WallWidth : WallWidth;
        displayHeight = isHorizontal ? WallWidth : 2 * SquareSize + WallWidth;
    }

    public void Display()
    {
        DrawRectangle(posX, posY, displayWidth, displayHeight, PlacedWallColour);
    }
}