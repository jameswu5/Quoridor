using System;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Quoridor;

public class HoverButton : RectangularButton
{
    public HoverButton(int posX, int posY, int width, int height) : base(posX, posY, width, height) {}

    protected override void HoverDisplay()
    {
        DrawRectangle(posX, posY, width, height, Settings.HoverTint);
    }
}