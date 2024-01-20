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

            int move = action.wall == null ? GenerateMove(board.players[board.turn].position, coord) : GenerateMove(action.wall);

            if (board.legalMoves.Contains(move))
            {
                OnMove(move);
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
        
        board.players[board.turn].TurnToMove();
    }
}