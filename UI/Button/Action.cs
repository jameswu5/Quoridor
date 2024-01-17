using System;

namespace Quoridor;

public class Action
{
    public Coord? coord;

    public Action(Coord? coord = null)
    {
        this.coord = coord;
    }
}