using System;
using Raylib_cs;

namespace Quoridor;

/// <summary>
/// Minimax bot that only works for two players
/// </summary>
public class PositionBot : Bot
{

    public PositionBot(Board board, int ID, Coord position, Color colour, Coord goal) : base(board, ID, position, colour, goal)
    {
        if (Settings.Board.NumOfPlayers != 2)
        {
            throw new Exception($"Number of players is {Settings.Board.NumOfPlayers}, PositionBot only suppots 2 players");
        }
    }

    public override void ChooseMove()
    {
        Search(3);
        moveFound = true;
    }

    private static int Evaluate(Board board)
    {
        return board.players[0].DistanceFromGoal() - board.players[1].DistanceFromGoal();
    }

    private int Search(int depth)
    {
        

        if (depth == 0)
        {

        }


        return 0;
    }
}