using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class BoardSquareButton : HoverButton
{
    public bool highlighted;
    public bool selected;

    public BoardSquareButton(int posX, int posY) : base(posX, posY, SquareSize, SquareSize, BoardColour)
    {
        highlighted = false;
        selected = false;
    }

    protected override void Display()
    {
        if (selected)
        {
            DrawRectangle(posX, posY, width, height, SquareTurnColour);
        }
        else
        {
            base.Display();
        }

        if (highlighted)
        {
            Highlight();
        }
    }

    protected override void HoverDisplay()
    {
        Display();
        DrawRectangle(posX, posY, width, height, Settings.DefaultHoverTint);
    }

    private void Highlight()
    {
        DrawCircle(posX + width / 2, posY + height / 2, SquareHighlightRadius, SquareHighlightColour);
    }
}