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
        List<Coord> available = board.GetLegalSquareMoves();
        int num = rng.Next(available.Count);
        chosenMove = Move.GenerateMove(board.players[board.turn].position, available[num]);
        moveFound = true;
    }
}