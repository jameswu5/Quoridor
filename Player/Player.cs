using System;
using Raylib_cs;

namespace Quoridor;

public abstract class Player
{
    public Board board;
    public int ID;
    public Coord position;
    public Color colour;
    public Coord goal; // Not strictly a coord, but it's convenient as it packs two integers
    public int wallsLeft;

    public Player(Board board, int ID, Coord position, Color colour, Coord goal)
    {
        this.board = board;
        this.ID = ID;
        this.position = position;
        this.colour = colour;
        this.goal = goal;
        wallsLeft = Settings.Board.WallsPerPlayer;
    }

    public void SetPosition(Coord coord)
    {
        position = coord;
    }

    public bool ReachedGoal()
    {
        return position.x == goal.x || position.y == goal.y;
    }

    public int DistanceFromGoal()
    {
        return goal.x == -1 ? Math.Abs(position.y - goal.y) : Math.Abs(position.x - goal.x);
    }

    /* Interface for human and bot */

    public event System.Action<int> PlayChosenMove;

    public abstract void Update();

    public abstract void TurnToMove();

    public abstract override string ToString();

    public void Decided(int move)
    {
        PlayChosenMove.Invoke(move);
    }
}