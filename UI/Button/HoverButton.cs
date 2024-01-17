using System;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Quoridor;

public class HoverButton : RectangularButton
{
    private Color colour;
    private Color hoverColour;

    public HoverButton(int posX, int posY, int width, int height, Color? colour = null, Color? hoverColour = null) : base(posX, posY, width, height)
    {
        this.colour = colour ?? Settings.DefaultButtonColour;
        this.hoverColour = hoverColour ?? Settings.DefaultButtonHoverColour;
    }

    protected override void Display()
    {
        DrawRectangle(posX, posY, width, height, colour);
    }

    protected override void HoverDisplay()
    {
        DrawRectangle(posX, posY, width, height, hoverColour);
    }
}