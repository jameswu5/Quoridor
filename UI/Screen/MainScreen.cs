using System;
using Raylib_cs;
using static Quoridor.Settings.Board;

namespace Quoridor;

public class MainScreen : Screen
{
    private Board board;

    public MainScreen(Board board)
    {
        this.board = board;
    }

    public override void Display()
    {
        board.Display();
    }
}