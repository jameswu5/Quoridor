using System;
using Raylib_cs;

namespace Quoridor;

public class MainScreen : Screen
{
    private Board board;

    public MainScreen()
    {
        board = new Board();
    }

    public override void Display()
    {
        board.Display();
    }
}