using System;
using Raylib_cs;

namespace Quoridor;

public static class Settings
{
    public const int ScreenWidth  = 1080;
    public const int ScreenHeight = 720;
    public static readonly Color ScreenColour = new Color(235, 235, 235, 255);

    public static readonly Color DefaultDarkTextColour = Color.BLACK;
    public const int DefaultFontSize = 32;

    public static class Board
    {
        public const int BoardSize = 9;
        public const int NumOfPlayers = 4;

        public static readonly Color BorderColour = Color.BLACK;
        public static readonly Color BoardColour = Color.LIGHTGRAY;
        public static readonly Color WallColour = Color.DARKGRAY;
        public const int BorderWidth = 10;
        public const int SquareSize = 50;
        public const int WallWidth = 10;

        public const int BoardSideLength = BoardSize * (SquareSize + WallWidth) - WallWidth;
        public const int BoardPaddingX = (ScreenWidth - BoardSideLength) >> 1;
        public const int BoardPaddingY = (ScreenHeight - BoardSideLength) >> 1;
    }


}