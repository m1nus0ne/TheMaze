namespace TheMaze;

public class Cell
{
    public int X;
    public int Y;
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
    private int _freeCellsCounter;
    public (int, int) EndPos;
    public (int, int) StartPos;

    public Field(int rows, int cols)
    {
        _freeCellsCounter = cols * rows - 1;
        _rows = rows * 2 + 1;
        _cols = cols * 2 + 1;
        Map = new Cell[_cols, _rows];

        GenerateMaze();
    }

    public Cell GetNeighbours(Cell cell)
    {
        var Neighbours = new List<Cell>();
        var i = cell.X;
        var j = cell.Y;
        if (CheckCell(i + 2, j)) Neighbours.Add(Map[i + 2, j]);
        if (CheckCell(i, j + 2)) Neighbours.Add(Map[i, j + 2]);
        if (CheckCell(i - 2, j)) Neighbours.Add(Map[i - 2, j]);
        if (CheckCell(i, j - 2)) Neighbours.Add(Map[i, j - 2]);

        if (Neighbours.Count != 0)
            return Neighbours[new Random().Next(Neighbours.Count)];
        return null;
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


        var wayStack = new Stack<Cell>();
        var currentCell = Map[1, 1];
        while (_freeCellsCounter != 0)
        {
            Backtrack(ref currentCell, ref wayStack);
        }

        StartPos = ((int) (1.5 * CFG.TileSize), (int) (1.5 * CFG.TileSize));
        EndPos = ((_rows - 2) * CFG.TileSize, (_cols - 2) * CFG.TileSize);
    }

    public void Backtrack(ref Cell currentCell, ref Stack<Cell> wayStack)
    {
        // recursive backtrack
        currentCell.IsVisited = true;
        var nextCell = GetNeighbours(currentCell);
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
    }
}