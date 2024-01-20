using System;
using Raylib_cs;

namespace Quoridor;

/// <summary>
/// Bot that only plays random moves
/// </summary>
public class RandomBot : Bot
{
    private readonly Random rng;

    public RandomBot(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal)
    {
        rng = new Random();
    }

    public override void ChooseMove()
    {
        int num = rng.Next(board.legalMoves.Count);
        chosenMove = board.legalMoves[num];
        moveFound = true;
    }
}