using Grid = GameOfLife.Model.Grid;

const int GridWidth = 30;
const int GridHeight = 20;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// The server keeps no state: each browser owns its grid and sends it here to get the next generation.
app.MapGet("/api/size", () => new { width = GridWidth, height = GridHeight });

app.MapPost("/api/step", (GridRows request) =>
{
    var rows = request.Rows;

    if (rows is null || rows.Length != GridHeight || rows.Any(row => row is null || row.Length != GridWidth))
    {
        return Results.BadRequest($"Grid must be {GridWidth} columns by {GridHeight} rows.");
    }

    var grid = new Grid(GridWidth, GridHeight);
    for (var row = 0; row < GridHeight; row++)
    {
        for (var column = 0; column < GridWidth; column++)
        {
            grid.SetAlive(column, row, rows[row][column]);
        }
    }

    var nextGrid = grid.NextGeneration();

    var nextRows = new bool[GridHeight][];
    for (var row = 0; row < GridHeight; row++)
    {
        nextRows[row] = new bool[GridWidth];
        for (var column = 0; column < GridWidth; column++)
        {
            nextRows[row][column] = nextGrid.IsAlive(column, row);
        }
    }

    return Results.Ok(new GridRows(nextRows));
});

app.Run();

record GridRows(bool[][] Rows);
