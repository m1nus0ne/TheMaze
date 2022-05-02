using System.Drawing.Drawing2D;
using System.Numerics;
using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public partial class Form1 : Form
{
    private Raycasting Handler;
    

    public Form1()
    {
        // FormBorderStyle = FormBorderStyle.None;
        Handler = new Raycasting();
        Handler.TileSize = 100f;
        Handler.MaxDepth = 800;
        Handler.RaysCounter = 1200;
        Handler.FOV = (float) (Math.PI/3);
        Handler.WinWidth = 1200;
        Handler.WinHeight = 800;
        
        Handler.Maze = new Field(10, 10);
        Handler.Player = new Player(new Vector2(Handler.TileSize * 1.5f, Handler.TileSize * 1.5f), 0);
        
        InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Size = new Size(1200, 800);
        DoubleBuffered = true;
        var g = e.Graphics;
        
        
        g.FillRectangle(new SolidBrush(Color.Blue),0,0,Handler.WinWidth,Handler.WinHeight/2);
        g.FillRectangle(new SolidBrush(Color.Green),0,Handler.WinHeight/2,Handler.WinWidth,Handler.WinHeight);
        var pen = new Pen(Color.Red, 1);

        // g.DrawLine(pen,Handler.GetDirectionLine().Item1,Handler.GetDirectionLine().Item2);
       
        
        foreach (var point in Handler.GetDirectionLines())
        {
            var a = Color.FromArgb(point.Item1, point.Item1, point.Item1);
            
            g.FillRectangle(new SolidBrush(a),point.Item2);
        }
        // foreach (var w in Handler.Maze.WallsSet)
        // {
        //     g.FillRectangle(new SolidBrush(Color.Black), w.X * Handler.TileSize, w.Y * Handler.TileSize,
        //         Handler.TileSize, Handler.TileSize);
        // }
        // g.FillEllipse(new SolidBrush(Color.Red),Handler.Player.Pos.X-5,Handler.Player.Pos.Y-5,10,10);
    }

    

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyValue == (int)Keys.S)
        {
            Handler.Move(new Vector2(0f, -1f), 2f);
        }

        if (e.KeyValue == (int)Keys.W)
        {
            Handler.Move(new Vector2(0f, 1f), 2f);
        }

        if (e.KeyValue == (int)Keys.A)
        {
            Handler.Move(new Vector2(1f, 0), 1f);
        }

        if (e.KeyValue == (int)Keys.D)
        {
            Handler.Move(new Vector2(-1f, 0f), 2f);
        }

        if (e.KeyValue == (int)Keys.E)
        {
            Handler.TurnLeft((float) (Math.PI/8));
        }

        if (e.KeyValue == (int)Keys.Q)
        {
            Handler.TurnRight((float) (Math.PI/8));
        }
        if (e.KeyValue == (int)Keys.R)
        {
            Handler.Maze = new Field(10, 10);
        }
        Invalidate();
    }
}