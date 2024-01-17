using System;
using Raylib_cs;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class MainScreen : Screen
{
    private Board board;
    private List<Button> squareButtons;

    public MainScreen()
    {
        board = new Board();
        squareButtons = new List<Button>();
        InitialiseButtons();
    }

    public override void Display()
    {
        board.Display();
        foreach (Button button in squareButtons) {
            button.Render();
        }
    }

    private void InitialiseButtons()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Coord topLeft = Board.GetTopLeftCoord(i, j);
                HoverButton button = new HoverButton(topLeft.x, topLeft.y, SquareSize, SquareSize);
                AddButtonAction(button, new Action(coord: new Coord(i, j)));
                squareButtons.Add(button);
            }
        }
    }
}