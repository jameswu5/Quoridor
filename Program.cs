﻿using System;
using Raylib_cs;

namespace Quoridor;

public static class Program
{
    public static void Main()
    {
        Game game = new Game();
        game.Run();
    }
}