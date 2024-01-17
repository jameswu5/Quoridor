using System;

namespace Quoridor;

public class Wall
{
    public Coord coord;
    public bool isHorizontal;

    public int x;
    public int y;

    public Wall(Coord coord, bool isHorizontal)
    {
        this.coord = coord;
        this.isHorizontal = isHorizontal;
        
        x = coord.x;
        y = coord.y;
    }

    public Wall(int x, int y, bool isHorizontal)
    {
        this.x = x;
        this.y = y;
        this.isHorizontal = isHorizontal;
        
        coord = new Coord(x, y);
    }
}