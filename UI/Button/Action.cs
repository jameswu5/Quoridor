using System;

namespace Quoridor;

public class Action
{
    public Coord? coord;
    public Wall? wall;

    public Action(Coord? coord = null, Wall? wall = null)
    {
        this.coord = coord;
        this.wall = wall;
    }
}