using System;
using System.Threading;
using Raylib_cs;

namespace Quoridor;

public abstract class Bot : Player
{
    protected bool moveFound = false;
    protected int chosenMove = 0;

    public Bot(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal) {}

    public static Bot CreateBot(Board.PlayerType botType, Board board, int ID, Coord startPos, Color colour, Coord goal)
    {
        return botType switch
        {
            Board.PlayerType.RandomBot => new RandomBot(board, ID, startPos, colour, goal),
            Board.PlayerType.MoveBot => new MoveBot(board, ID, startPos, colour, goal),
            _ => throw new Exception("Bot not found"),
        };
    }

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

    // Implement the move choosing algorithm in this function
    public abstract void ChooseMove();

    public override string ToString()
    {
        return "Bot";
    }
}