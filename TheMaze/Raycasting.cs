using System.Numerics;
using System;

namespace TheMaze;

public class Raycasting
{
    public int ScaleX;
    public Field Maze;
    public Player Player;
    public float TileSize;
    public int WinWidth;
    public int WinHeight;
    public float FOV;
    public int RaysCounter;
    public int MaxDepth;
    public float DeltaAngle;


    public void MazeGen() => Maze.GenerateMaze();
    public void TurnLeft(float delta) => Player.Angle += delta;
    public void TurnRight(float delta) => Player.Angle -= delta;

    public void Move(Vector2 direction, float delta)
    {
        var possiblePos = Player.Pos + direction.RotateRadians(Player.Angle) * delta;
        if (Maze.Map[(int) (Math.Floor(possiblePos.X) / TileSize),
                (int) (Math.Floor(possiblePos.Y) / TileSize)].Value == TypeOfSpace.Wall) return;
        Player.Pos += direction.RotateRadians(Player.Angle) * delta * 2;
    }

    public Tuple<int, int> GetCurentTileIndex()
    {
        return new Tuple<int, int>((int) (Math.Floor(Player.Pos.X) / TileSize),
            (int) (Math.Floor(Player.Pos.Y) / TileSize));
    }

    public IEnumerable<Tuple<int,Rectangle>> GetDirectionLines()
    {
        DeltaAngle = FOV / RaysCounter;
        ScaleX = WinWidth / RaysCounter;
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2))/4;
        var projCoeff = 3*dist * TileSize;
        
        var currentAngle = Player.Angle - FOV / 2;
        for (int ray = 0; ray < RaysCounter; ray++)
        {
            // var sinA = Math.Sin(currentAngle);
            // var cosA = Math.Cos(currentAngle);
            var flag = true;
            for (double depth = 0; depth < MaxDepth; depth++)
            {
                var rayVector = Player.Pos + new Vector2(0, (float) depth).RotateRadians(currentAngle);

                if (Maze.Map[(int) (rayVector.X / TileSize), (int) (rayVector.Y / TileSize)].Value == TypeOfSpace.Wall)
                {
                    depth *= Math.Cos(Player.Angle - currentAngle);
                    int c = (int) (255 / (1 + depth * depth * 0.0001));
                    var projY = projCoeff / depth;
                    yield return new Tuple<int, Rectangle>(c,
                        new Rectangle(ray * ScaleX, (int) (WinHeight / 2 - projY/2 ), ScaleX, (int) projY));
                    flag = false;
                    break;
                }
            }

            // if (flag)
            // {
            //     var rayVector = Player.Pos + new Vector2(0, MaxDepth).RotateRadians(currentAngle);
            //
            //     yield return new Point((int) rayVector.X, (int) rayVector.Y);
            // }

            currentAngle += DeltaAngle;
        }
    }

    public Tuple<Point, Point> GetDirectionLine()
    {
        var point1 = new Point((int) Player.Pos.X, (int) Player.Pos.Y);
        var v = new Vector2(0, -TileSize / 4).RotateRadians(Player.Angle);
        var point2 = new Point(point1.X + (int) v.X, (int) (point1.Y + v.Y));
        return Tuple.Create(point1, point2);
    }
    public IEnumerable<Tuple<Point,Point>> lines()
    {
        var a = new Point();
        var b = new Point();
        DeltaAngle = FOV / RaysCounter;
        ScaleX = WinWidth / RaysCounter;
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2))/4;
        var projCoeff = 3*dist * TileSize;
        
        var currentAngle = Player.Angle - FOV / 2;
        for (int ray = 0; ray < RaysCounter; ray++)
        {
            // var sinA = Math.Sin(currentAngle);
            // var cosA = Math.Cos(currentAngle);
            var flag = true;
            for (double depth = 0; depth < MaxDepth; depth++)
            {
                var rayVector = Player.Pos + new Vector2(0, (float) depth).RotateRadians(currentAngle);

                if (Maze.Map[(int) (rayVector.X / TileSize), (int) (rayVector.Y / TileSize)].Value == TypeOfSpace.Wall && flag)
                {
                    depth *= Math.Cos(Player.Angle - currentAngle);
                    int c = (int) (255 / (1 + depth * depth * 0.0001));
                    var projY = projCoeff / depth;
                    a = new Point(ray * ScaleX, (int) (WinHeight / 2 + projY / 2));
                    flag = false;
                    
                }

                if (Maze.Map[(int) (rayVector.X / TileSize), (int) (rayVector.Y / TileSize)].Value ==
                    TypeOfSpace.Wall && !flag)
                {
                    depth *= Math.Cos(Player.Angle - currentAngle);
                    int c = (int) (255 / (1 + depth * depth * 0.0001));
                    var projY = projCoeff / depth;
                    b = new Point(ray * ScaleX, (int) (WinHeight / 2 + projY / 2));
                    flag = true;
                    yield return Tuple.Create<Point, Point>(a,b);
                }
            }

            // if (flag)
            // {
            //     var rayVector = Player.Pos + new Vector2(0, MaxDepth).RotateRadians(currentAngle);
            //
            //     yield return new Point((int) rayVector.X, (int) rayVector.Y);
            // }

            currentAngle += DeltaAngle;
        }
    }
}

public class Player
{
    public Vector2 Pos;
    private bool OnJuppad;
    public float Angle;

    public Player(Vector2 pos, float angle)
    {
        Pos = pos;
        Angle = angle;
        OnJuppad = false;
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