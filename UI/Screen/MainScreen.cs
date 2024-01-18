using System;
using Raylib_cs;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class MainScreen : Screen
{
    private Board board;

    private BoardSquareButton[,] squareButtons;

    public MainScreen(Board board)
    {
        this.board = board;
        squareButtons = new BoardSquareButton[BoardSize, BoardSize];
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

    public void SetSquareButtonHighlight(Coord coord, bool b) => squareButtons[coord.x, coord.y].highlighted = b;

    public void SetSquareButtonSelected(Coord coord, bool b) => squareButtons[coord.x, coord.y].selected = b;
}