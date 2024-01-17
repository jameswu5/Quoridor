using System;
using Raylib_cs;

namespace Quoridor;

public class MainScreen : Screen
{
    public override void Display()
    {
        Raylib.DrawText("Main Screen", 20, 20, 40, Color.BLACK);
    }
}