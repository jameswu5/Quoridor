using System;
using Raylib_cs;
using static Quoridor.Move;

namespace Quoridor;

public class Game
{
    public enum GameScreen {Main};
    private GameScreen currentScreen;
    private MainScreen mainScreen;

    private Board board;

    public Game()
    {
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Quoridor");
        Raylib.SetTargetFPS(60);

        board = new Board();
        board.clickAction += ExecuteAction;
        board.playMove += OnMove;
        currentScreen = GameScreen.Main;
        mainScreen = new MainScreen(board);
        mainScreen.clickAction += ExecuteAction;

        NewGame();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Settings.ScreenColour);
            Update();
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    // This is run every frame
    private void Update() 
    {
        switch (currentScreen)
        {
            case GameScreen.Main:
                mainScreen.Display();
                break;
            default:
                throw new Exception("No screen found.");
        }
    }

    public void ExecuteAction(Action action)
    {
        if (action.coord != null)
        {            
            Coord coord = (Coord)action.coord;

            if (action.wall == null)
            {
                if (board.legalSquares.Contains(coord))
                {
                    OnMove(GenerateMove(board.players[board.turn].position, coord));
                }
            }
            else
            {
                Wall wall = action.wall;
                if (wall.isHorizontal)
                {
                    if (board.validWallsHor[wall.x, wall.y])
                    {
                        OnMove(GenerateMove(action.wall));
                    }
                }
                else
                {
                    if (board.validWallsVer[wall.x, wall.y])
                    {
                        OnMove(GenerateMove(action.wall));
                    }
                }
            }
        }
    }

    public void NewGame()
    {
        board.NewGame();
        foreach (Coord coord in board.legalSquares)
        {
            board.SetSquareButtonHighlight(coord, true);
        }
        board.SetSquareButtonSelected(board.players[board.turn].position, true);
    }

    private void OnMove(int move)
    {
        // Unhighlight all the legal moves
        foreach (Coord pos in board.legalSquares)
        {
            board.SetSquareButtonHighlight(pos, false);
        }

        board.SetSquareButtonSelected(board.players[board.turn].position, false);

        board.MakeMove(move);

        foreach (Coord pos in board.legalSquares)
        {
            board.SetSquareButtonHighlight(pos, true);
        }

        board.SetSquareButtonSelected(board.players[board.turn].position, true);
    }
}