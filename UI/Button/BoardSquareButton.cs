using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class BoardSquareButton : HoverButton
{
    private bool highlighted;

    public BoardSquareButton(int posX, int posY) : base(posX, posY, SquareSize, SquareSize, BoardColour, SquareHoverColour)
    {
        highlighted = false;
    }

    protected override void Display()
    {
        base.Display();
        if (highlighted)
        {
            Highlight();
        }
    }

    protected override void HoverDisplay()
    {
        base.HoverDisplay();
        if (highlighted)
        {
            Highlight();
        }
    }

    private void Highlight()
    {
        DrawCircle(posX + width / 2, posY + height / 2, (SquareSize - 10) / 2, Color.DARKGRAY);
    }
}