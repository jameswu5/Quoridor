using System;

namespace Quoridor;

public static class Move
{
    /*
     *  Moves are encoded as an integer.
     *  [1 bit - isWall][1 bit - isHorizontal][4 bits - startCoord.X][4 bits - startCoord.Y][4 bits - endCoord.X][4 bits - endCoord.Y]
     */

    public const int TargetYShift = 0;
    public const int TargetXShift = TargetYShift + 4;
    public const int StartYShift = TargetXShift + 4;
    public const int StartXShift = StartYShift + 4;
    public const int IsHorizontalShift = StartXShift + 1;
    public const int IsWallShift = IsHorizontalShift + 1;

    public const int StartXMask = 0b1111 << StartXShift;
    public const int StartYMask = 0b1111 << StartYShift;
    public const int TargetXMask = 0b1111 << TargetXShift;
    public const int TargetYMask = 0b1111 << TargetYShift;
    public const int IsHorizontalMask = 1 << IsHorizontalShift;
    public const int IsWallMask = 1 << IsWallShift;

    // Player move
    public static int GenerateMove(Coord start, Coord target)
    {
        int move = 0;
        move |= start.x << StartXShift;
        move |= start.y << StartYShift;
        move |= target.x << TargetXShift;
        move |= target.y << TargetYShift;
        return move;
    }

    // Wall move
    public static int GenerateMove(Wall wall)
    {
        int move = 0;
        move |= wall.x << TargetXShift;
        move |= wall.y << TargetYShift;
        move |= 1 << IsWallShift;
        move |= (wall.isHorizontal ? 1 : 0) << IsHorizontalShift;
        return move;
    }

    public static bool IsWall(int move) => (move & IsWallMask) > 0;

    public static bool IsHorizontal(int move) => (move & IsHorizontalMask) > 0;

    public static int GetStartX(int move) => (move & StartXMask) >> StartXShift;

    public static int GetStartY(int move) => (move & StartYMask) >> StartYShift;

    public static int GetTargetX(int move) => (move & TargetXMask) >> TargetXShift;

    public static int GetTargetY(int move) => (move & TargetYMask) >> TargetYShift;

    public static Coord RetrieveStartCoord(int move) => new Coord(GetStartX(move), GetStartY(move));

    public static Coord RetrieveTargetCoord(int move) => new Coord(GetTargetX(move), GetTargetY(move));

    public static Wall RetrieveWall(int move) => new Wall(move & TargetXMask, move & TargetYMask, IsHorizontal(move));
}