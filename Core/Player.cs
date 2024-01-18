using System;
using Raylib_cs;

namespace Quoridor;

public class Player
{
    public Coord position;
    public Color colour;
    public Coord goal; // Not strictly a coord, but it's convenient as it packs two integers

    public Player(Coord position, Color colour, Coord goal)
    {
        this.position = position;
        this.colour = colour;
        this.goal = goal;
    }

    public void SetPosition(Coord coord)
    {
        position = coord;
    }

    public bool ReachedGoal()
    {
        return position.x == goal.x || position.y == goal.y;
    }
}