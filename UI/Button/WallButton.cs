using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class WallButton : HoverButton
{
    private readonly int displayWidth;
    private readonly int displayHeight;

    public WallButton(int posX, int posY, bool isHorizontal) : base(posX, posY, GetWidth(isHorizontal), GetHeight(isHorizontal), WallColour)
    {
        displayWidth = isHorizontal ? 2 * SquareSize + WallWidth : WallWidth;
        displayHeight = isHorizontal ? WallWidth : 2 * SquareSize + WallWidth;
    }    

    private static int GetWidth(bool isHorizontal) => isHorizontal ? SquareSize : WallWidth;

    private static int GetHeight(bool isHorizontal) => isHorizontal ? WallWidth : SquareSize;

    // The board already paints the wall channels every frame, so there is nothing
    // to draw here. Repainting WallColour over this button's cell would overdraw
    // (erase) the second square of an adjacent hovered wall's highlight.
    protected override void Display() {}

    protected override void HoverDisplay()
    {
        DrawRectangle(posX, posY, displayWidth, displayHeight, Settings.DefaultHoverTint);
    }
}