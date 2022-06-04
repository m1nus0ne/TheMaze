namespace TheMaze;

public class CFG
{
    public const int WindowWidth = 1800;
    public const int WindowHeight = 1080;
    public const int TextureWidth = 120;
    public const int TextureHeight = 80;
    public static float RunStep = 12f;
    public static float WalkStep = 6f;


    public static float MoveStep = WalkStep;
    public const float RotationStep = (float) Math.PI/18;
    public const int MazeSize = 10;
    public const int TileSize = 100;
    public const int MapTileSize = 20;
    public const int TextureScale = TextureWidth / TileSize;
    public const int RaysCounter = WindowWidth/2;
    public const int MaxDepth = 2000;
    public const float FOV = (float) (Math.PI / 3);
    
    
    
}