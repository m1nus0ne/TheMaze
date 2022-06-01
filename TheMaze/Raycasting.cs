using System.Numerics;
using static TheMaze.CFG;

namespace TheMaze;

public class Raycasting
{
    public Field Maze;
    public Player Player;


    public int ScaleX;
    private float _deltaAngle;
    public HashSet<(int, int)> WallSet;
    public HashSet<(int, int)> WallMinimapSet;

    public Raycasting()
    {
        _deltaAngle = FOV / RaysCounter;
        ScaleX = WindowWidth / RaysCounter;
        Maze = new Field(10, 10);
        Player = new Player(new Vector2(TileSize * 1.5f, TileSize * 1.5f), (float) (Math.PI / 2));
        WallSetsInitialize();
    }


    public void MazeGen()
    {
        Maze.GenerateMaze();
        WallSetsInitialize();
    }

    public void TurnLeft(float delta) => Player.Angle += delta;
    public void TurnRight(float delta) => Player.Angle -= delta;

    private void WallSetsInitialize()
    {
        WallSet = new HashSet<(int, int)>();
        WallMinimapSet = new HashSet<(int, int)>();
        foreach (var cell in Maze.Map)
        {
            if (cell.Value == TypeOfSpace.Wall)
            {
                WallSet.Add((cell.X * TileSize, cell.Y * TileSize));
                WallMinimapSet.Add((cell.X * MapTileSize, cell.Y * MapTileSize));
            }
        }
    }

    public void Move(Vector2 direction, float delta)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var possiblePos = Player.Pos + direction.RotateRadians(Player.Angle) * delta +
                                  new Vector2(i * 5, j * 5);
                if (Maze.Map[(int) (Math.Floor(possiblePos.X) / TileSize),
                        (int) (Math.Floor(possiblePos.Y) / TileSize)].Value == TypeOfSpace.Wall) return;
            }
        }


        Player.Pos += direction.RotateRadians(Player.Angle) * delta;
    }

    public void GetDirectionLines(Graphics g)
    {
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2)) / 2;
        var projCoeff = 3 * dist * TileSize;
        var currentAngle = Player.Angle - FOV / 2;

        for (int ray = 0; ray < RaysCounter; ray++)
        {
            var flag = true;
            for (double depth = 0; depth < MaxDepth; depth++)
            {
                var rayVector = Player.Pos + new Vector2(0, (float) depth).RotateRadians(currentAngle);

                if (Maze.Map[(int) (rayVector.X / TileSize), (int) (rayVector.Y / TileSize)].Value ==
                    TypeOfSpace.Wall)
                {
                    depth *= Math.Cos(Player.Angle - currentAngle);
                    int c = (int) (255 / (1 + depth * depth * 0.0001));
                    var projY = projCoeff / depth;
                    var a = Color.FromArgb(c, c, c);
                    g.FillRectangle(new SolidBrush(a),
                        new Rectangle(ray * ScaleX, (int) (WindowHeight / 2 - projY / 2), ScaleX, (int) projY));


                    break;
                }
            }

            currentAngle += _deltaAngle;
        }
    }


    (int, int) Mapping(int a, int b)
    {
        return (a / TileSize * TileSize, b / TileSize * TileSize);
    }

    public void lines(Graphics g)
    {
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2)) / 2;
        var projCoeff = 3 * dist * TileSize;
        var currentAngle = Player.Angle - FOV / 2;
        var (xm, ym) = Mapping((int) Player.Pos.X, (int) Player.Pos.Y);
        var ofsList = new List<int>();
        for (int ray = 0; ray < RaysCounter; ray++)
        {
            var sinA = Math.Sin(currentAngle + Math.PI / 2);
            var cosA = Math.Cos(currentAngle + Math.PI / 2);
            var (xh, dx) = cosA >= 0 ? (xm + TileSize, 1) : (xm, -1);
            int yv;
            var (verticalDepth, horizontalDepth) = (0.0, 0.0);
            for (int i = 0; i < MaxDepth; i += TileSize)
            {
                verticalDepth = (xh - Player.Pos.X) / cosA;
                yv = (int) (Player.Pos.Y + verticalDepth * sinA);
                if (WallSet.Contains(Mapping(xh + dx, yv)))
                    break;
                xh += dx * TileSize;
            }
            
            (yv, var dy) = sinA >= 0 ? (ym + TileSize, 1) : (ym, -1);
            for (int i = 0; i < MaxDepth; i += TileSize)
            {
                horizontalDepth = (yv - Player.Pos.Y) / sinA;
                xh = (int) (Player.Pos.X + horizontalDepth * cosA);
                if (WallSet.Contains(Mapping(xh, yv + dy)))
                    break;
                yv += dy * TileSize;
            }

            var (depth, offset) = horizontalDepth > verticalDepth ? (verticalDepth, yv) : (horizontalDepth, xh);
            depth *= Math.Cos(Player.Angle - currentAngle);
            offset %= TileSize;
            ofsList.Add(offset);
            var projHeight = projCoeff / depth;
            var img = ImageHandler.WallTexture;
            var sourceRect = new Rectangle(offset * TextureScale, 0, TextureScale, TextureHeight);
            var destinationRect = new Rectangle(ray * ScaleX, (int) ((WindowHeight - projHeight) / 2), ScaleX,
                (int) projHeight);
            
            g.DrawImage(img, destinationRect, sourceRect, GraphicsUnit.Pixel);
            
            // int c = (int) (255 / (1 + depth * depth * 0.00002));
            // var color = Color.FromArgb(c, c, c);
            // // g.FillRectangle(new SolidBrush(color),
            // //     new Rectangle(ray * ScaleX, (int) ((CFG.WindowHeight - proj_height) / 2), ScaleX, (int) proj_height));
            
            currentAngle += _deltaAngle;
        }
    }

    public void Scanlines(Graphics g)
    {
        var dist = RaysCounter / (2 * Math.Tan(FOV / 2)) / 2;
        var projCoeff = 1 * dist * TileSize;
        var currentAngle = Player.Angle - FOV / 2;
        var (xm, ym) = Mapping((int) Player.Pos.X, (int) Player.Pos.Y);

        for (int ray = 0; ray < RaysCounter; ray++)
        {
            var Xstack = new List<int>();
            var Ystack = new List<int>();
            var sinA = Math.Sin(currentAngle + Math.PI / 2);
            var cosA = Math.Cos(currentAngle + Math.PI / 2);
            var (hx, dx) = cosA >= 0 ? (xm + TileSize, 1) : (xm, -1);
            int yv;
            var (verticalDepth, horizontalDepth) = (0.0, 0.0);
            for (int i = 0; i < MaxDepth / 2; i += TileSize)
            {
                verticalDepth = (hx - Player.Pos.X) / cosA;
                yv = (int) (Player.Pos.Y + verticalDepth * sinA);
                if (WallSet.Contains(Mapping(hx + dx, yv)))
                    Xstack.Add((int) verticalDepth);
                hx += dx * TileSize;
            }

            (yv, var dy) = sinA >= 0 ? (ym + TileSize, 1) : (ym, -1);
            for (int i = 0; i < MaxDepth / 2; i += TileSize)
            {
                horizontalDepth = (yv - Player.Pos.Y) / sinA;
                hx = (int) (Player.Pos.X + horizontalDepth * cosA);
                if (WallSet.Contains(Mapping(hx, yv + dy)))
                    Ystack.Add((int) horizontalDepth);
                yv += dy * TileSize;
            }

            foreach (var ponts in Ystack.Zip(Xstack))
            {
                var a = ponts.First * Math.Cos(FOV - currentAngle);
                var b = ponts.Second * Math.Cos(FOV - currentAngle);
                var color = Color.Black;
                var proj_height1 = projCoeff / a;
                var proj_height2 = projCoeff / b;
                // g.DrawLine(new Pen(new SolidBrush(Color.Black),2),
                //     new Point(ray*ScaleX,(int) ((CFG.WindowHeight + proj_height1) / 2)),
                //     new Point(ray*ScaleX,(int) ((CFG.WindowHeight + proj_height2) / 2)));

                g.FillRectangle(new SolidBrush(color),
                    new RectangleF(ray * ScaleX, (int) ((WindowHeight + proj_height1) / 2), ScaleX,
                        (int) proj_height1 / 10));
                g.FillRectangle(new SolidBrush(color),
                    new RectangleF(ray * ScaleX, (int) ((WindowHeight + proj_height2) / 2), ScaleX,
                        (int) proj_height2 / 10));
            }

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