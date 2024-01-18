using System;
using Raylib_cs;

namespace Quoridor;

public class Game
{
    public enum GameScreen {Main};
    private GameScreen currentScreen;
    private MainScreen mainScreen;

    private Board board;
    private int turn;
    private List<Coord> legalSquares;

    public Game()
    {
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Quoridor");
        Raylib.SetTargetFPS(60);

        board = new Board();
        board.clickAction += ExecuteAction;
        currentScreen = GameScreen.Main;
        mainScreen = new MainScreen(board);
        mainScreen.clickAction += ExecuteAction;

        turn = 0;
        legalSquares = new List<Coord>();

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
                if (legalSquares.Contains(coord))
                {
                    OnMove(coord);
                }
            }
            else
            {
                Wall wall = (Wall)action.wall;
                if (wall.isHorizontal)
                {
                    if (board.validWallsHor[wall.x, wall.y])
                    {
                        OnMove(coord, action.wall);
                    }
                }
                else
                {
                    if (board.validWallsVer[wall.x, wall.y])
                    {
                        OnMove(coord, action.wall);
                    }
                }
            }
        }
    }

    public void NewGame()
    {
        board.NewGame();
        turn = 0;
        legalSquares = board.GetLegalSquares(turn);
        foreach (Coord coord in legalSquares)
        {
            board.SetSquareButtonHighlight(coord, true);
        }
        board.SetSquareButtonSelected(board.players[turn].position, true);
    }

    private void OnMove(Coord newSquare, Wall? wall = null)
    {
        // Unhighlight all the legal moves
        foreach (Coord pos in legalSquares)
        {
            board.SetSquareButtonHighlight(pos, false);
        }

        board.SetSquareButtonSelected(board.players[turn].position, false);

        board.MakeMove(turn, newSquare, wall);

        if (board.players[turn].ReachedGoal())
        {
            GameOver();
            return;
        }

        turn = (turn + 1) % Settings.Board.NumOfPlayers;

        legalSquares = board.GetLegalSquares(turn);

        foreach (Coord pos in legalSquares)
        {
            board.SetSquareButtonHighlight(pos, true);
        }

        board.SetSquareButtonSelected(board.players[turn].position, true);
    }

    private void GameOver()
    {
        legalSquares.Clear();
        board.GameOver();
    }
}