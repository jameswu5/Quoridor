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
            Console.WriteLine(coord);
            if (legalSquares.Contains(coord))
            {
                OnMove(coord);
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

    private void OnMove(Coord newSquare)
    {
        // Unhighlight all the legal moves
        foreach (Coord coord in legalSquares)
        {
            board.SetSquareButtonHighlight(coord, false);
        }

        board.SetSquareButtonSelected(board.players[turn].position, false);

        board.MakeMove(turn, newSquare);
        turn = (turn + 1) % Settings.Board.NumOfPlayers;

        legalSquares = board.GetLegalSquares(turn);

        foreach (Coord coord in legalSquares)
        {
            board.SetSquareButtonHighlight(coord, true);
        }

        board.SetSquareButtonSelected(board.players[turn].position, true);
    }
}