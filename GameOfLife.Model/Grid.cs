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
    

    public bool IsAlive(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return false;
        }
        return _cells[x, y];
    }

    public void SetAlive(int x, int y, bool alive)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Cell is outside the grid.");
        }

        _cells[x, y] = alive;

    }

    public  int CountLiveNeighbors(int x, int y)
    {
        var count = 0;
        for (var dy = -1; dy <= 1; dy++)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                if(dx == 0 && dy == 0)
                {
                    continue;
                    
                }

            if (IsAlive(x + dx, y + dy))
                {
                   count++; 
                } 
            }
        
        }
        return count;
    }

    public Grid  NextGeneration()
    {
        var next =  new Grid(Width, Height);

        for (var y = 0; y < Height; y++)
        {
            for(  var x = 0; x < Width; x++)
            {
                var neighbors = CountLiveNeighbors(x, y);
                var alive = IsAlive(x, y);
                bool aliveNext;
                if(alive)
                {
                    aliveNext = neighbors == 2 || neighbors == 3;
                }
                else
                {
                    aliveNext = neighbors == 3;
                }
                next.SetAlive(x, y, aliveNext);
            }
        }

        return next;
    }
}
