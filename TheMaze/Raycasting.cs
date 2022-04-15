using System.Numerics;

namespace TheMaze;


public class Player
{
    public int X;
    public int Y;
    private bool OnJuppad;
    

    public Player(int x, int y)
    {
        X = x;
        Y = y;
        OnJuppad = false;
    }
    

    public void Move(int deltaX, int deltaY,HashSet<Cell> wallSet)
    {
        var nextCell =  new Cell(X + deltaX, Y + deltaY, TypeOfSpace.Wall);
        if (wallSet.Contains(nextCell)) return;
        X += deltaX;
        Y += deltaY;
    }
}

