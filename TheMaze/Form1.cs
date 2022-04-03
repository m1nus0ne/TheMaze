using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public partial class Form1 : Form
{
    private Field _maze;
    private int _tile;
    

    public Form1()
    {
        InitializeComponent();
        _maze = new Field(10, 10);
        _tile = 20;
        _maze.GenerateMaze();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        foreach (var w in _maze.WallsSet)
        {
            g.FillRectangle(new SolidBrush(Color.Black), w.X * _tile, w.Y * _tile, _tile, _tile);
        }
        
    }
}