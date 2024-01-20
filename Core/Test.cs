using System;

namespace Quoridor;

public static class Test
{
    /// <summary>
    /// Returns the number of leaf nodes at a depth given a board
    /// </summary>
    public static int Perft(Board board, int depth)
    {
        if (depth == 1)
        {
            return board.legalMoves.Count;
        }

        int numOfPositions = 0;

        foreach (int move in board.legalMoves)
        {
            board.MakeMove(move);
            numOfPositions += Perft(board, depth - 1);
            board.UndoMove();
        }

        return numOfPositions;
    }

    /// <summary>
    /// Makes 'numOfMoves' moves on random then unmakes them all (going back to original position)
    /// </summary>
    public static void Shuffle(Board board, int numOfMoves)
    {
        Random rng = new();

        for (int i = 0; i < numOfMoves; i++)
        {
            int index = rng.Next(board.legalMoves.Count);
            board.MakeMove(board.legalMoves[index]);
        }
        for (int i = 0; i < numOfMoves; i++)
        {
            board.UndoMove();
        }
    }

    /// <summary>
    /// No Perft data avaiable, so I make some random moves and unmake them, and all the perft results in each iteration should be the same.
    /// </summary>
    public static void TestMoveGeneration(int iterations, int numOfMoves, int depth)
    {
        for (int i = 0; i < iterations; i++)
        {
            Board board = new();
            Shuffle(board, numOfMoves);
            Console.WriteLine(Perft(board, depth));
        }
    }
}