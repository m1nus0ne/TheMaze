using System.Text;
using System.Linq;

namespace TheMaze;

public class Cell
{
    public int X;
    public int Y;
    public List<Cell> Neighbors;
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
        Neighbors = new List<Cell>();
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
    private int _rows;
    private int _cols;
    public Cell[,] Map;
    public HashSet<Cell> WallsSet;
    private int _freeCellsCounter;


    public Field(int rows, int cols)
    {
        _freeCellsCounter = cols * rows;
        _rows = rows * 2 + 1;
        _cols = cols * 2 + 1;
        Map = new Cell[_cols, _rows];
        WallsSet = new HashSet<Cell>();
        GenerateMaze();
    }

    public Cell GetNeibours(Cell cell)
    {
        var i = cell.X;
        var j = cell.Y;
        if (CheckCell(i + 2, j)) cell.Neighbors.Add(Map[i + 2, j]);
        if (CheckCell(i, j + 2)) cell.Neighbors.Add(Map[i, j + 2]);
        if (CheckCell(i - 2, j)) cell.Neighbors.Add(Map[i - 2, j]);
        if (CheckCell(i, j - 2)) cell.Neighbors.Add(Map[i, j - 2]);

        if (Map[i, j].Neighbors.Count != 0)
            return cell.Neighbors[new Random().Next(Map[i, j].Neighbors.Count)];
        return null;
    }

    public void GetWallSet()
    {
        WallsSet = new HashSet<Cell>();
        foreach (var cell in Map)
        {
            if (cell.Value == TypeOfSpace.Wall) WallsSet.Add(cell);
        }
    }

    public bool CheckCell(int x, int y)
    {
        {
            if (0 < x && x < _cols && x % 2 == 1 &&
                0 < y && y < _rows && y % 2 == 1 &&
                !Map[x, y].IsVisited) return true;
            return false;
        }
    }


    public void GenerateMaze()
    {
        // генерация базоваой сетки

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (i == 0 || i == _rows - 1)
                {
                    Map[i, j] = new Cell(i, j, TypeOfSpace.Wall);
                }

                if (j % 2 == 0 || i % 2 == 0)
                {
                    Map[i, j] = new Cell(i, j, TypeOfSpace.Wall);
                }
                else Map[i, j] = new Cell(i, j, TypeOfSpace.Empty);
            }
        }

        
        

        GetWallSet();
    }

    public void Backtrack(ref Cell currentCell, ref Stack<Cell> wayStack)
    {
        
        // recursive backtrack
        currentCell.IsVisited = true;
        var nextCell = GetNeibours(currentCell);
        if (nextCell != null)
        {
            nextCell.IsVisited = true;
            wayStack.Push(nextCell);
            var breackedWall = Map[(currentCell.X + nextCell.X) / 2, (currentCell.Y + nextCell.Y) / 2];
            breackedWall.Value = TypeOfSpace.Empty;
            currentCell = nextCell;
            _freeCellsCounter--;
        }
        else if (wayStack.Count != 0) currentCell = wayStack.Pop();
        GetWallSet();
    }
}