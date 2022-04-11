using System.Drawing.Drawing2D;
using System.Numerics;
using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public partial class Form1 : Form
{
    private Field _maze;
    private float _tile;
    private Player _player;


    public Form1()
    {
        InitializeComponent();
        _maze = new Field(10, 10);
        _tile = 20;
        _maze.GenerateMaze();
        _player = new Player(new Vector2(_tile * 1.5f, _tile * 1.5f), 0);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        DoubleBuffered = true;
        var g = e.Graphics;
        foreach (var w in _maze.WallsSet)
        {
            g.FillRectangle(new SolidBrush(Color.Black), w.X * _tile, w.Y * _tile, _tile, _tile);
        }

        g.FillEllipse(new SolidBrush(Color.Red), _player.Pos.X, _player.Pos.Y, _tile / 2, _tile / 2);
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar == 's')
        {
            _player.Move(new Vector2(0f, 1f), 1f,_maze.WallsSet,_tile);
            Invalidate();
        }
        if (e.KeyChar == 'w')
        {
            _player.Move(new Vector2(0f, -1f), 1f,_maze.WallsSet,_tile);
            Invalidate();
        }
        if (e.KeyChar == 'a')
        {
            _player.Move(new Vector2(-1f, 0f), 1f,_maze.WallsSet,_tile);
            Invalidate();
        }
        if (e.KeyChar == 'd')
        {
            _player.Move(new Vector2(1f, 0), 1f,_maze.WallsSet,_tile);
            Invalidate();
        }
    }
}