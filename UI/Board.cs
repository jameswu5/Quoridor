using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings.Board;

namespace Quoridor;

public partial class Board
{
    public System.Action<Quoridor.Action> clickAction;
    private BoardSquareButton[,] squareButtons;
    private WallButton[,] hWallButtons;
    private WallButton[,] vWallButtons;

    private void InitialiseUI()
    {
        squareButtons = new BoardSquareButton[BoardSize, BoardSize];
        hWallButtons = new WallButton[BoardSize - 1, BoardSize - 1];
        vWallButtons = new WallButton[BoardSize - 1, BoardSize - 1];
        InitialiseButtons();
    }

    private void InitialiseButtons()
    {
        // Squares
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Coord topLeft = GetTopLeftCoord(i, j);
                BoardSquareButton button = new BoardSquareButton(topLeft.x, topLeft.y);
                AddButtonAction(button, new Action(coord: new Coord(i, j), null));
                squareButtons[i, j] = button;
            }
        }

        // Walls
        for (int i = 0; i < BoardSize - 1; i++)
        {
            for (int j = 0; j < BoardSize - 1; j++)
            {
                Coord temp = GetTopLeftCoord(i + 1, j + 1);
                WallButton vButton = new WallButton(temp.x - WallWidth, temp.y, false);
                AddButtonAction(vButton, new Action(coord: new Coord(i, j), wall: new Wall(i, j, false)));
                vWallButtons[i, j] = vButton;

                WallButton hButton = new WallButton(temp.x - SquareSize - WallWidth, temp.y + SquareSize, true);
                AddButtonAction(hButton, new Action(coord: new Coord(i, j), wall: new Wall(i, j, true)));
                hWallButtons[i, j] = hButton;
            }
        }
    }

    private void AddButtonAction(BoardSquareButton button, Action action) => button.OnClick += () => clickAction(action);

    private void AddButtonAction(WallButton button, Action action) => button.OnClick += () => clickAction(action);

    public void SetSquareButtonHighlight(Coord coord, bool b) => squareButtons[coord.x, coord.y].highlighted = b;

    public void SetSquareButtonSelected(Coord coord, bool b) => squareButtons[coord.x, coord.y].selected = b;

    public void Display()
    {
        DisplayBoard();
        DisplayButtons();
        DisplayState();
    }

    /// <summary>
    /// Displays the empty board
    /// </summary>
    private void DisplayBoard()
    {
        // Border
        DrawRectangle(
            BoardPaddingX - BorderWidth,
            BoardPaddingY - BorderWidth,
            BorderWidth * 2 + BoardSideLength,
            BorderWidth * 2 + BoardSideLength,
            BorderColour
        );
        
        // Squares
        DrawRectangle(BoardPaddingX, BoardPaddingY, BoardSideLength, BoardSideLength, BoardColour);

        // Walls
        for (int i = 0; i < BoardSize - 1; i++)
        {
            DrawRectangle(BoardPaddingX, BoardPaddingY + SquareSize + i * (SquareSize + WallWidth), BoardSideLength, WallWidth, WallColour);
            DrawRectangle(BoardPaddingX + SquareSize + i * (SquareSize + WallWidth), BoardPaddingY, WallWidth, BoardSideLength, WallColour);
        }
    }


    private void DisplayButtons()
    {
        foreach (BoardSquareButton button in squareButtons)
        {
            button.Render();
        }

        for (int i = 0; i < BoardSize - 1; i++)
        {
            for (int j = 0; j < BoardSize - 1; j++)
            {
                // Only render the buttons associated with valid walls
                if (validWallsHor[BoardSize - i - 2, j])
                {
                    hWallButtons[BoardSize - i - 2, j].Render();
                }
                if (validWallsVer[i, j])
                {
                    vWallButtons[i, j].Render();
                }
            }
        }
    }

    /// <summary>
    /// Displays the things placed on the board (i.e. placed players and placed walls)
    /// </summary>
    public void DisplayState()
    {
        // Players
        for (int i = 0; i < NumOfPlayers; i++)
        {
            Coord coord = players[i].position;
            Coord topLeft = GetTopLeftCoord(coord);
            DrawCircle(topLeft.x + SquareSize / 2, topLeft.y + SquareSize / 2, PlayerRadius, players[i].colour);
        }

        // Walls
        foreach (Wall wall in walls)
        {
            wall.Display();
        }
    }

    public static Coord GetTopLeftCoord(int x, int y)
    {
        return new Coord(BoardPaddingX + x * (SquareSize + WallWidth), BoardPaddingY + (BoardSize - 1 - y) * (SquareSize + WallWidth));
    }

    public static Coord GetTopLeftCoord(Coord coord) => GetTopLeftCoord(coord.x, coord.y);
}