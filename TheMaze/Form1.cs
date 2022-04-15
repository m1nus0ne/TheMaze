using System.Drawing.Drawing2D;
using System.Numerics;
using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public partial class Form1 : Form
{
    public Field _maze;
    private float _tile;
    private Player _player;


    public Form1()
    {
        InitializeComponent();
        _maze = new Field(10, 10);
        _tile = 20;
        _player = new Player(1, 1);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        _maze.GetVision(_player.X,_player.Y,1);
        DoubleBuffered = true;
        var g = e.Graphics;
        foreach (var w in _maze.Map)
        {
            if (w.Value == TypeOfSpace.Wall && w.IsVisible)
            g.FillRectangle(new SolidBrush(Color.Black), w.X * _tile, w.Y * _tile, _tile, _tile);
        }

        g.FillRectangle(new SolidBrush(Color.Red), _player.X * _tile, _player.Y * _tile, _tile, _tile);
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar == 's')
        {
            _player.Move(0, 1, _maze.WallsSet);
        }

        if (e.KeyChar == 'w')
        {
            _player.Move(0, -1, _maze.WallsSet);
        }

        if (e.KeyChar == 'a')
        {
            _player.Move(-1, 0, _maze.WallsSet);
        }

        if (e.KeyChar == 'd')
        {
            _player.Move(1, 0, _maze.WallsSet);
        }

        if (e.KeyChar == 'e') _maze.GenerateMaze();
        Invalidate();
    }
}