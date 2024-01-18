using System;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Quoridor;

public class HoverButton : RectangularButton
{
    protected Color colour;

    public HoverButton(int posX, int posY, int width, int height, Color? colour = null) : base(posX, posY, width, height)
    {
        this.colour = colour ?? Settings.DefaultButtonColour;
    }

    protected override void Display()
    {
        DrawRectangle(posX, posY, width, height, colour);
    }

    protected override void HoverDisplay()
    {
        Display();
        DrawRectangle(posX, posY, width, height, Settings.DefaultHoverTint);
    }
}