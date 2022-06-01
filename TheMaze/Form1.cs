using System.Numerics;

namespace TheMaze;

public partial class Form1 : Form
{
    public Raycasting Handler;


    public Form1()
    {
        // FormBorderStyle = FormBorderStyle.None;
        Handler = new Raycasting();


        
        InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Size = new Size(1200, 800);
        DoubleBuffered = true;
        var g = e.Graphics;


        g.FillRectangle(new SolidBrush(Color.Blue), 0, 0, CFG.WindowWidth, CFG.WindowHeight / 2);
        g.FillRectangle(new SolidBrush(Color.Green), 0, CFG.WindowHeight / 2, CFG.WindowWidth, CFG.WindowHeight);
        var pen = new Pen(Color.Red, 1);
        Handler.lines(g);

        


        foreach (var w in Handler.WallMinimapSet)
        {
            {
                g.FillRectangle(new SolidBrush(Color.Black), w.Item1, w.Item2,
                    CFG.MapTileSize, CFG.MapTileSize);
            }
        }
        
        g.FillEllipse(new SolidBrush(Color.Red), (Handler.Player.Pos.X - 5) / 5, (Handler.Player.Pos.Y - 5) / 5, 10,
            10);
    }


    protected override void OnKeyDown(KeyEventArgs e)
    {
        var v = new Vector2(0, 0);
        if (e.KeyValue == (int) Keys.S)
        {
            v += new Vector2(0f, -1f);
        }

        if (e.KeyValue == (int) Keys.W)
        {
            v += new Vector2(0f, 1f);
        }

        if (e.KeyValue == (int) Keys.A)
        {
            v += new Vector2(1f, 0f);
        }

        if (e.KeyValue == (int) Keys.D)
        {
            v += new Vector2(-1f, 0f);
        }

        if (e.KeyValue == (int) Keys.E)
        {
            Handler.TurnLeft((float) (Math.PI / 8));
        }

        if (e.KeyValue == (int) Keys.Q)
        {
            Handler.TurnRight((float) (Math.PI / 8));
        }

        if (e.KeyValue == (int) Keys.R)
        {
            Handler.Maze = new Field(10, 10);
        }

        Handler.Move(v, 2f);
        Invalidate();
    }
}