using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Grid = GameOfLife.Model.Grid;

namespace GameOfLife.WpfApp;

public partial class MainWindow : Window
{
    private const int GridWidth = 30;
    private const int GridHeight = 20;
    private const int CellSize = 20;

    private Grid _grid = new(GridWidth, GridHeight);
    private readonly Rectangle[,] _rectangles = new Rectangle[GridWidth, GridHeight];
    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromMilliseconds(200) };

    public MainWindow()
    {
        InitializeComponent();
        _timer.Tick += (sender, eventArguments) => Step();
        CreateRectangles();
        Draw();
    }

    private void CreateRectangles()
    {
        for (var row = 0; row < GridHeight; row++)
        {
            for (var column = 0; column < GridWidth; column++)
            {
                var rectangle = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Stroke = Brushes.LightGray
                };

                Canvas.SetLeft(rectangle, column * CellSize);
                Canvas.SetTop(rectangle, row * CellSize);
                GridCanvas.Children.Add(rectangle);
                _rectangles[column, row] = rectangle;
            }
        }
    }

    private void Draw()
    {
        for (var row = 0; row < GridHeight; row++)
        {
            for (var column = 0; column < GridWidth; column++)
            {
                _rectangles[column, row].Fill = _grid.IsAlive(column, row) ? Brushes.Black : Brushes.White;
            }
        }
    }

    private void Step()
    {
        _grid = _grid.NextGeneration();
        Draw();
    }

    private void GridCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs eventArguments)
    {
        var position = eventArguments.GetPosition(GridCanvas);
        var column = (int)(position.X / CellSize);
        var row = (int)(position.Y / CellSize);

        if (column >= GridWidth || row >= GridHeight)
        {
            return;
        }

        _grid.SetAlive(column, row, !_grid.IsAlive(column, row));
        Draw();
    }

    private void StepButton_Click(object sender, RoutedEventArgs eventArguments)
    {
        Step();
    }

    private void StartStopButton_Click(object sender, RoutedEventArgs eventArguments)
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
            StartStopButton.Content = "Start";
        }
        else
        {
            _timer.Start();
            StartStopButton.Content = "Stop";
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs eventArguments)
    {
        _grid = new Grid(GridWidth, GridHeight);
        Draw();
    }
}
