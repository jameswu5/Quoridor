using System;
using Raylib_cs;

namespace Quoridor;

public class Human : Player
{
    public Human(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal) {}

    public override void TurnToMove() {}

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "Human";
    }
}