# Game of Life

A WPF version of Conway's Game of Life, built as one of the tasks in the developer test.

## Requirements

.NET 10 SDK. The WPF app needs Windows. The web app and tests run anywhere, including Docker.

## Run

```
dotnet test
dotnet run --project GameOfLife.WpfApp
```

- Click a cell to make it alive or dead.
- **Step** moves one generation forward.
- **Start / Stop** runs a generation every 200 ms.
- **Clear** kills all cells.

Stop the simulation before drawing a pattern. A single cell has no neighbors and dies on the next tick.

## Structure

- `GameOfLife.Model`: all logic (`Grid`). No UI code.
- `GameOfLife.WpfApp`: window, drawing and buttons. Calls the model.
- `GameOfLife.Tests`: xUnit tests for the model.

## Assumptions

- The grid has a fixed size of 30 x 20 in the window. `Grid` itself accepts any size of at least 1 x 1.
- Cells outside the grid count as dead. A glider that reaches the edge therefore ends up as a stable block instead of wrapping around.
- `NextGeneration()` returns a new grid and never changes the old one. Cells must not affect each other during the same step.
- Invalid input throws an error with a clear message:
  - a width or height below 1 throws `ArgumentOutOfRangeException`
  - `SetAlive` outside the grid throws `ArgumentOutOfRangeException`
  - `IsAlive` outside the grid returns `false`, because grid edges are a normal case when counting neighbors
- The window ignores clicks outside the grid.

## Code standards

See [CODING_STANDARDS.md](CODING_STANDARDS.md).

## Web version (any platform)

```
docker build -t gameoflife .
docker run --rm -p 8080:8080 gameoflife
```

Open http://localhost:8080. It has the same Step / Start / Clear buttons and click-to-toggle cells as the WPF app. Without Docker: `dotnet run --project GameOfLife.WebApp`.

Run the tests in Docker:

```
docker build --target test -t gameoflife-tests .
docker run --rm gameoflife-tests
```
