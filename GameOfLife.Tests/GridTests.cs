using GameOfLife.Model;

namespace GameOfLife.Tests;

public class GridTests
{
    [Fact]
    public void Constructor_ValidSize_AllCellsAreDead()
    {
        var grid = new Grid(width: 5, height: 3);

        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 5; column++)
            {
                Assert.False(grid.IsAlive(column, row));
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

        var liveNeighborCount = grid.CountLiveNeighbors(2, 2);

        Assert.Equal(0, liveNeighborCount);
    }

    [Fact]
    public void CountLiveNeighbors_ThreeLiveNeighbors_ReturnsThree()
    {
        var grid = new Grid(width: 5, height: 5);
        grid.SetAlive(1, 1, true);
        grid.SetAlive(2, 1, true);
        grid.SetAlive(3, 3, true);

        var liveNeighborCount = grid.CountLiveNeighbors(2, 2);

        Assert.Equal(3, liveNeighborCount);
    }

    [Fact]
    public void CountLiveNeighbors_CornerCell_CellsOutsideGridCountAsDead()
    {
        var grid = new Grid(width: 5, height: 5);
        grid.SetAlive(1, 0, true);

        var liveNeighborCount = grid.CountLiveNeighbors(0, 0);

        Assert.Equal(1, liveNeighborCount);
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
    public void SetAlive_CellOutsideGrid_ThrowsArgumentOutOfRangeException(int column, int row)
    {
        var grid = new Grid(width: 5, height: 3);

        Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetAlive(column, row, true));
    }

    // Cell (2,2) in a 5x5 grid has the eight neighbors (1,1) (2,1) (3,1) (1,2) (3,2) (1,3) (2,3) (3,3).
    private static Grid CreateGridWithAliveCells(params (int Column, int Row)[] cells)
    {
        var grid = new Grid(width: 5, height: 5);

        foreach (var (column, row) in cells)
        {
            grid.SetAlive(column, row, true);
        }

        return grid;
    }

    [Fact]
    public void NextGeneration_LiveCellWithOneNeighbor_Dies()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1));

        var nextGrid = grid.NextGeneration();

        Assert.False(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithTwoNeighbors_Survives()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1));

        var nextGrid = grid.NextGeneration();

        Assert.True(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithThreeNeighbors_Survives()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1), (3, 1));

        var nextGrid = grid.NextGeneration();

        Assert.True(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_LiveCellWithFourNeighbors_Dies()
    {
        var grid = CreateGridWithAliveCells((2, 2), (1, 1), (2, 1), (3, 1), (1, 2));

        var nextGrid = grid.NextGeneration();

        Assert.False(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_DeadCellWithThreeNeighbors_BecomesAlive()
    {
        var grid = CreateGridWithAliveCells((1, 1), (2, 1), (3, 1));

        var nextGrid = grid.NextGeneration();

        Assert.True(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_DeadCellWithTwoNeighbors_StaysDead()
    {
        var grid = CreateGridWithAliveCells((1, 1), (2, 1));

        var nextGrid = grid.NextGeneration();

        Assert.False(nextGrid.IsAlive(2, 2));
    }

    [Fact]
    public void NextGeneration_AnyGrid_DoesNotChangeOriginalGrid()
    {
        var grid = CreateGridWithAliveCells((2, 2));

        grid.NextGeneration();

        Assert.True(grid.IsAlive(2, 2));
    }
}
