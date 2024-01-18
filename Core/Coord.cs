using System;

namespace Quoridor;

public struct Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator ==(Coord coord1, Coord coord2) => coord1.x == coord2.x && coord1.y == coord2.y;

    public static bool operator !=(Coord coord1, Coord coord2) => !(coord1 == coord2);

    public override readonly string ToString() => $"({x}, {y})";
}