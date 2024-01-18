using System;
using Raylib_cs;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class MainScreen : Screen
{
    private Board board;

    private Button[,] squareButtons;

    public MainScreen(Board board)
    {
        this.board = board;
        squareButtons = new Button[BoardSize, BoardSize];
        InitialiseButtons();
    }

    public override void Display()
    {
        board.Display();
        foreach (Button button in squareButtons)
        {
            button.Render();
        }
        board.DisplayState();
    }

    private void InitialiseButtons()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Coord topLeft = Board.GetTopLeftCoord(i, j);
                BoardSquareButton button = new BoardSquareButton(topLeft.x, topLeft.y);
                AddButtonAction(button, new Action(coord: new Coord(i, j)));
                squareButtons[i, j] = button;
            }
        }
    }
}