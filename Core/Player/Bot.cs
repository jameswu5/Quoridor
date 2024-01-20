using System;
using System.Threading;
using Raylib_cs;

namespace Quoridor;

public class Bot : Player
{
    private bool moveFound = false;
    private int chosenMove = 0;

    public Bot(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal) {}

    public override void TurnToMove()
    {
        moveFound = false;
        Thread backgroundThread = new Thread(ChooseMove);
        backgroundThread.Start();
    }

    public override void Update()
    {
        if (moveFound)
        {
            moveFound = false;
            Decided(chosenMove);
        }
    }

    public void ChooseMove()
    {
        // Implement the move selection algorithm here.

        // Simple example: choose the first move available
        chosenMove = board.legalMoves[0];
        moveFound = true;
    }

    public override string ToString()
    {
        return "Bot";
    }
}