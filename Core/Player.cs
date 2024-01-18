using System;
using Raylib_cs;

namespace Quoridor;

public class Player
{
    public Coord position;
    public Color colour;

    public Player(Coord position, Color colour)
    {
        this.position = position;
        this.colour = colour;
    }

    public void SetPosition(Coord coord)
    {
        position = coord;
    }
}