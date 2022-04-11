using System.Numerics;

namespace TheMaze;

public class Raycasting
{
}

public class Player
{
    public Vector2 Pos;
    private bool OnJuppad;
    public double Angle;

    public Player(Vector2 pos, double angle)
    {
        Pos = pos;
        Angle = angle;
        OnJuppad = false;
    }

    public void TurnLeft(double delta) => Angle += delta;
    public void TurnRight(double delta) => Angle -= delta;

    public void Move(Vector2 direction, float delta, HashSet<Cell> wallSet, float cellSize)
    {
        var possiblePos = Pos + direction.RotateRadians(Angle) * delta;
        //TODO: Обработка колизий

        Pos += direction.RotateRadians(Angle) * delta;
    }
}

public static class VectorExt
{
    private const double DegToRad = Math.PI / 180;

    public static Vector2 Rotate(this Vector2 v, double degrees)
    {
        return v.RotateRadians(degrees * DegToRad);
    }

    public static Vector2 RotateRadians(this Vector2 v, double radians)
    {
        var ca = (float) Math.Cos(radians);
        var sa = (float) Math.Sin(radians);
        return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
    }
}