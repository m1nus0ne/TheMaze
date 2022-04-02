using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public partial class Form1 : Form
{
    private Field _maze;
    private int _tile;
    public Cell currentCell;

    public Form1()
    {
        InitializeComponent();
        _maze = new Field(10, 10);
        _tile = 20;
        var timer = new Timer();
        timer.Interval = 500;
        var wayStack = new Stack<Cell>();
        currentCell = _maze.Map[1, 1];
        wayStack.Push(currentCell);
        timer.Tick += (sender, args) =>
        {
            _maze.Backtrack(ref currentCell, ref wayStack);
            Invalidate();
        };
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        DoubleBuffered = true;
        foreach (var w in _maze.WallsSet)
        {
            g.FillRectangle(new SolidBrush(Color.Black), w.X * _tile, w.Y * _tile, _tile, _tile);
        }
        g.FillRectangle(new SolidBrush(Color.Red),currentCell.X*_tile,currentCell.Y*_tile,_tile,_tile);
    }
}