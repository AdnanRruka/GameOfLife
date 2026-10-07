using GameOfLife.Model;

namespace GameOfLife.Tests;

public class GridTests
{
    [Fact]
    public void Constructor_ValidSize_AllCellsAreDead()
    {
        var grid = new Grid(width: 5, height: 3);

        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 5; x++)
            {
                Assert.False(grid.IsAlive(x, y));
            }
        }
    }

    [Fact]
    public void SetAlive_ValidCell_CellIsAlive()
    {
        var grid = new Grid(width: 5, height: 3);

        grid.SetAlive(2, 1, true);

        Assert.True(grid.IsAlive(2, 1));
    }

    [Fact]
    public void CountLiveNeighbors_NoLiveNeighbors_ReturnsZero()
    {
        var grid = new Grid(width: 5, height: 5);

        var count = grid.CountLiveNeighbors(2, 2);

        Assert.Equal(0, count);
    }

    [Fact]
    public void CountLiveNeighbors_ThreeLiveNeighbors_ReturnsThree()
    {
        var grid = new Grid(width: 5, height: 5);
        grid.SetAlive(1, 1, true);
        grid.SetAlive(2, 1, true);
        grid.SetAlive(3, 3, true);

        var count = grid.CountLiveNeighbors(2, 2);

        Assert.Equal(3, count);
    }

    [Fact]
    public void CountLiveNeighbors_CornerCell_CellsOutsideGridCountAsDead()
    {
        var grid = new Grid(width: 5, height: 5);
        grid.SetAlive(1, 0, true);

        var count = grid.CountLiveNeighbors(0, 0);

        Assert.Equal(1, count);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(-1, 5)]
    [InlineData(5, 0)]
    [InlineData(5, -1)]
    public void Constructor_SizeBelowOne_ThrowsArgumentOutOfRangeException(int width, int height)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(width, height));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 3)]
    public void SetAlive_CellOutsideGrid_ThrowsArgumentOutOfRangeException(int x, int y)
    {
        var grid = new Grid(width: 5, height: 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetAlive(x, y, true));
    }

    // Cell (2,2) in a 5x5 grid has the eight neighbors (1,1) (2,1) (3,1) (1,2) (3,2) (1,3) (2,3) (3,3).
    private static Grid CreateGridWithAliveCells(params (int X, int Y)[] cells)
    {
        var grid = new Grid(width: 5, height: 5);

        foreach (var (x, y) in cells)
        {
            grid.SetAlive(x, y, true);
        }

        return grid;
    }

    [Fact]
    public void NextGeneration_LiveCellWithOneNeighbor_Dies()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1));

        var next = grid.NextGeneration();

        Assert.False(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithTwoNeighbors_Survives()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1));

        var next = grid.NextGeneration();

        Assert.True(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithThreeNeighbors_Survives()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1), (3, 1));

        var next = grid.NextGeneration();

        Assert.True(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithFourNeighbors_Dies()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1), (3, 1), (1, 2));

        var next = grid.NextGeneration();

        Assert.False(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_DeadCellWithThreeNeighbors_BecomesAlive()
    {
        var grid = CreateGridWithAliveCells((1, 1), (2, 1), (3, 1));

        var next = grid.NextGeneration();

        Assert.True(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_DeadCellWithTwoNeighbors_StaysDead()
    {
        var grid = CreateGridWithAliveCells((1, 1), (2, 1));

        var next = grid.NextGeneration();

        Assert.False(next.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_AnyGrid_DoesNotChangeOriginalGrid()
    {
        var grid = CreateGridWithAliveCells((2, 2));

        grid.NextGeneration();

        Assert.True(grid.IsAlive(2, 2));
    }
}
