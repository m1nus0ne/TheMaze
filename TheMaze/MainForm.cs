using System.Numerics;
using static TheMaze.Drawer;

namespace TheMaze;

public partial class MainForm : Form
{
    public Raycasting Handler;


    public MainForm()
    {
        Handler = new Raycasting();
        InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Size = new Size(CFG.WindowWidth, CFG.WindowHeight);
        DoubleBuffered = true;
        var g = e.Graphics;
        InitialiseGraphics(g);
        DrawGameObjects(Handler);
    }
}