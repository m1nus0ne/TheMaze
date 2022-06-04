using System.Numerics;
using static TheMaze.CFG;

namespace TheMaze;

public class Raycasting
{
    public Field Maze;
    public Player Player;
    public bool IsSolved;
    public int Score;

    private readonly int _scaleX;
    private readonly float _deltaAngle;
    private HashSet<(int, int)> _wallSet;
    public HashSet<(int, int)> WallMinimapSet;

    public Raycasting()
    {
        Score = 0;
        _deltaAngle = FOV / RaysCounter;
        _scaleX = WindowWidth / RaysCounter;
        Maze = new Field(10, 10);
        Player = new Player(new Vector2(Maze.StartPos.Item1, Maze.StartPos.Item2), (float) (Math.PI / 2));
        IsSolved = false;
        WallSetsInitialize();
    }


    private void WallSetsInitialize()
    {
        _wallSet = new HashSet<(int, int)>();
        WallMinimapSet = new HashSet<(int, int)>();
        foreach (var cell in Maze.Map)
        {
            if (cell.Value == TypeOfSpace.Wall)
            {
                _wallSet.Add((cell.X * TileSize, cell.Y * TileSize));
                WallMinimapSet.Add((cell.X * MapTileSize, cell.Y * MapTileSize));
            }
        }
    }

    private void RotatePlayer(float delta) => Player.Angle += delta;

    private void Move(Vector2 direction)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var possiblePos = Player.Pos + direction.RotateRadians(Player.Angle) * MoveStep +
                                  new Vector2(i * 5, j * 5);
                if (Maze.Map[(int) (Math.Floor(possiblePos.X) / TileSize),
                        (int) (Math.Floor(possiblePos.Y) / TileSize)].Value == TypeOfSpace.Wall) return;
            }
        }

        if (Mapping((int) Player.Pos.X, (int) Player.Pos.Y) == Maze.EndPos)
            IsSolved = true;

        Player.Pos += direction.RotateRadians(Player.Angle) * MoveStep;
    }

    public void UpdatePos((Vector2, float) data)
    {
        Move(data.Item1);
        RotatePlayer(data.Item2 * RotationStep);
    }


    (int, int) Mapping(int a, int b)
    {
        return (a / TileSize * TileSize, b / TileSize * TileSize);
    }

    public IEnumerable<(Rectangle destinationRect, Rectangle sourceRect)> GetWallsPosition()
    {
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2)) / 2;
        var projCoeff = 3 * dist * TileSize;
        var currentAngle = Player.Angle - FOV / 2;
        var (xm, ym) = Mapping((int) Player.Pos.X, (int) Player.Pos.Y);
        for (int ray = 0; ray < RaysCounter; ray++)
        {
            var (yv, xh, x, y, dx, dy) = (0, 0, 0, 0, 0, 0);
            var sinA = Math.Sin(currentAngle + Math.PI / 2);
            var cosA = Math.Cos(currentAngle + Math.PI / 2);
            (x, dx) = cosA >= 0 ? (xm + TileSize, 1) : (xm, -1);
            var (verticalDepth, horizontalDepth) = (0.0, 0.0);

            for (int i = 0; i < MaxDepth; i += TileSize)
            {
                verticalDepth = (x - Player.Pos.X) / cosA;
                yv = (int) (Player.Pos.Y + verticalDepth * sinA);
                if (_wallSet.Contains(Mapping(x + dx, yv)))
                    break;
                x += dx * TileSize;
            }

            (y, dy) = sinA >= 0 ? (ym + TileSize, 1) : (ym, -1);
            for (int i = 0; i < MaxDepth; i += TileSize)
            {
                horizontalDepth = (y - Player.Pos.Y) / sinA;
                xh = (int) (Player.Pos.X + horizontalDepth * cosA);
                if (_wallSet.Contains(Mapping(xh, y + dy)))
                    break;
                y += dy * TileSize;
            }

            var (depth, offset) = horizontalDepth > verticalDepth ? (verticalDepth, yv) : (horizontalDepth, xh);
            depth *= Math.Cos(Player.Angle - currentAngle);
            offset %= TileSize;
            var projHeight = projCoeff / depth;
            var img = ImageHandler.WallTexture;
            var sourceRect = new Rectangle(offset * TextureScale, 0, TextureScale, TextureHeight);
            var destinationRect = new Rectangle(ray * _scaleX, (int) ((WindowHeight - projHeight) / 2), _scaleX,
                (int) projHeight);

            yield return (destinationRect, sourceRect);
            currentAngle += _deltaAngle;
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