using System;
using Raylib_cs;

namespace Quoridor;

/// <summary>
/// Bot that plays random moves, but doesn't place walls
/// </summary>
public class MoveBot : Bot
{
    private readonly Random rng;

    public MoveBot(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal)
    {
        rng = new Random();
    }

    public override void ChooseMove()
    {
        // Implement the move selection algorithm here.

        // Simple example: pick randomly
        int num = rng.Next(board.legalSquares.Count);
        chosenMove = Move.GenerateMove(board.players[board.turn].position, board.legalSquares[num]);
        moveFound = true;
    }
}