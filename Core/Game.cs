using System;
using Raylib_cs;

namespace Quoridor;

public class Game 
{
    public enum GameScreen {Main};
    private GameScreen currentScreen;
    private MainScreen mainScreen;

    public Game()
    {
        Raylib.InitWindow(Settings.ScreenWidth, Settings.ScreenHeight, "Game");
        Raylib.SetTargetFPS(60);

        currentScreen = GameScreen.Main;
        mainScreen = new MainScreen();
        mainScreen.clickAction += ExecuteAction;
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
        Console.WriteLine("Button pressed");
    }
}