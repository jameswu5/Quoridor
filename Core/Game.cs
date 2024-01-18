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
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Game");
        Raylib.SetTargetFPS(60);

        board = new Board();
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
            if (legalSquares.Contains(coord))
            {
                board.MakeMove(turn, coord);
                OnMove();
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
            mainScreen.SetSquareButtonHighlight(coord, true);
        }
    }

    private void OnMove()
    {
        // Unhighlight all the legal moves
        foreach (Coord coord in legalSquares)
        {
            // I'm not a fan of this, this function should belong to Board, really
            mainScreen.SetSquareButtonHighlight(coord, false);
        }

        turn = (turn + 1) % Settings.Board.NumOfPlayers;

        legalSquares = board.GetLegalSquares(turn);

        foreach (Coord coord in legalSquares)
        {
            mainScreen.SetSquareButtonHighlight(coord, true);
        }
    }
}