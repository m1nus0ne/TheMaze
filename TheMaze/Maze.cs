using System.Text;

namespace TheMaze;

public class Cell
{
    public int X;
    public int Y;
    public HashSet<Cell> Neighbors;
    public bool IsVisited;
    public int Hash;
    public TypeOfSpace Value;

    public Cell(int x, int y, TypeOfSpace value)
    {
        X = x;
        Y = y;
        Value = value;
        IsVisited = false;
        Hash = GetHashCode();
    }


    public bool CheckCell(int x, int y, int cols, int rows)
    {
        {
            if (0 < x && x < cols && x % 2 == 1 && 0 < y && y < rows && y % 2 == 1 && !IsVisited) return true;
            return false;
        }
    }

    public sealed override int GetHashCode()
    {
        return X * 1000 + Y;
    }

    public override bool Equals(object? obj)
    {
        return obj.GetHashCode() == Hash;
    }
}

public class Field 
{
    private int rows;
    private int cols;
    private Cell[,] Map;
    public HashSet<Cell> WallsSet;


    public Field(int rows, int cols)
    {
        rows = rows * 2 + 1;
        cols = cols * 2 + 1;
        Map = new Cell[cols, rows];
        WallsSet = new HashSet<Cell>();
        
    }

    public void GenerateMaze()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (i == 0 || i == rows - 1)
                {
                    Map[i, j] = new Cell(i, j, TypeOfSpace.Wall);
                    WallsSet.Add(Map[i,j]);
                }
                if (j % 2 == 0 || i % 2 == 0)
                {
                    Map[i, j] = new Cell(i, j, TypeOfSpace.Wall);
                    WallsSet.Add(Map[i, j]);
                }
                else Map[i, j] = new Cell(i, j, TypeOfSpace.Empty);
            }
        }
    }
}

