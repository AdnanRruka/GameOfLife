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
        _timer.Tick += (sender, e) => Step();
        CreateRectangles();
        Draw();
    }

    private void CreateRectangles()
    {
        for (var y = 0; y < GridHeight; y++)
        {
            for (var x = 0; x < GridWidth; x++)
            {
                var rectangle = new Rectangle
                {
                    Width = CellSize,
                    Height = CellSize,
                    Stroke = Brushes.LightGray
                };

                Canvas.SetLeft(rectangle, x * CellSize);
                Canvas.SetTop(rectangle, y * CellSize);
                GridCanvas.Children.Add(rectangle);
                _rectangles[x, y] = rectangle;
            }
        }
    }

    private void Draw()
    {
        for (var y = 0; y < GridHeight; y++)
        {
            for (var x = 0; x < GridWidth; x++)
            {
                _rectangles[x, y].Fill = _grid.IsAlive(x, y) ? Brushes.Black : Brushes.White;
            }
        }
    }

    private void Step()
    {
        _grid = _grid.NextGeneration();
        Draw();
    }

    private void GridCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(GridCanvas);
        var x = (int)(position.X / CellSize);
        var y = (int)(position.Y / CellSize);

        if (x >= GridWidth || y >= GridHeight)
        {
            return;
        }

        _grid.SetAlive(x, y, !_grid.IsAlive(x, y));
        Draw();
    }

    private void StepButton_Click(object sender, RoutedEventArgs e)
    {
        Step();
    }

    private void StartStopButton_Click(object sender, RoutedEventArgs e)
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

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        _grid = new Grid(GridWidth, GridHeight);
        Draw();
    }
}
