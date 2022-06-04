namespace TheMaze;

public static class Drawer
{
    public static Graphics g;
    public static void InitialiseGraphics(Graphics graphics) => g = graphics;

    private static void DrawMiniMap(HashSet<(int, int)> MiniMap, (int,int) endPos)
    {
        foreach (var w in MiniMap)
        {
            {
                g.FillRectangle(new SolidBrush(Color.Black), w.Item1, w.Item2,
                    CFG.MapTileSize, CFG.MapTileSize);
            }
        }

        
        g.FillRectangle(new SolidBrush(Color.Green), endPos.Item1* CFG.MapTileSize / CFG.TileSize, endPos.Item2* CFG.MapTileSize / CFG.TileSize,
            CFG.MapTileSize, CFG.MapTileSize);
    }

    private static void DrawWalls(IEnumerable<(Rectangle destinationRect, Rectangle sourceRect)> walls)
    {
        g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, CFG.WindowWidth, CFG.WindowHeight / 2);
        g.FillRectangle(new SolidBrush(Color.Green), 0, CFG.WindowHeight / 2, CFG.WindowWidth, CFG.WindowHeight);
        foreach (var wall in walls)
        {
            g.DrawImage(ImageHandler.WallTexture, wall.destinationRect, wall.sourceRect, GraphicsUnit.Pixel);
        }
    }

    public static void DrawGameObjects(Raycasting handler)
    {
        if (handler.IsSolved)
        {
            DrawWalls(handler.GetWallsPosition());
            DrawSolve(handler);
            return;
        }
        DrawWalls(handler.GetWallsPosition());
        DrawMiniMap(handler.WallMinimapSet,handler.Maze.EndPos);
        g.FillEllipse(new SolidBrush(Color.Red), (handler.Player.Pos.X - 5) / 5, (handler.Player.Pos.Y - 5) / 5, 10,
            10);
        DrawScore(handler);
    }

    public static void DrawSolve(Raycasting handler)
    {
        g.DrawString($"You solved TheMaze! \n Your result is {handler.Score} \nPress R to restart",
            new Font("Times New Roman", 100, FontStyle.Regular), new SolidBrush(Color.Black),
            new PointF(200, 200));
    }

    public static void DrawScore(Raycasting handler) => g.DrawString($@"Score: {handler.Score}",
        new Font("Times New Roman", 36, FontStyle.Regular), new SolidBrush(Color.Black),
        new PointF(CFG.WindowWidth / 2, 20));
}