namespace GameOfLife.Model;

public class Grid
{
    private readonly bool[,] _cells;

    public int Width { get; }
    public int Height { get; }

    public Grid(int width, int height)
    {
        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be at least 1.");
        }

        if (height < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be at least 1.");
        }

        Width = width;
        Height = height;
        _cells = new bool[width, height];
    }

    public bool IsAlive(int column, int row)
    {
        if (IsOutsideGrid(column, row))
        {
            return false;
        }

        return _cells[column, row];
    }

    public void SetAlive(int column, int row, bool alive)
    {
        if (IsOutsideGrid(column, row))
        {
            throw new ArgumentOutOfRangeException(nameof(column), "Cell is outside the grid.");
        }

        _cells[column, row] = alive;
    }

    public int CountLiveNeighbors(int column, int row)
    {
        var liveNeighborCount = 0;

        for (var rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (var columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (columnOffset == 0 && rowOffset == 0)
                {
                    continue;
                }

                if (IsAlive(column + columnOffset, row + rowOffset))
                {
                    liveNeighborCount++;
                }
            }
        }

        return liveNeighborCount;
    }

    public Grid NextGeneration()
    {
        var nextGrid = new Grid(Width, Height);

        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                var liveNeighborCount = CountLiveNeighbors(column, row);
                var isAliveNow = IsAlive(column, row);
                bool isAliveNext;

                if (isAliveNow)
                {
                    isAliveNext = liveNeighborCount == 2 || liveNeighborCount == 3;
                }
                else
                {
                    isAliveNext = liveNeighborCount == 3;
                }

                nextGrid.SetAlive(column, row, isAliveNext);
            }
        }

        return nextGrid;
    }

    private bool IsOutsideGrid(int column, int row)
    {
        return column < 0 || column >= Width || row < 0 || row >= Height;
    }
}
