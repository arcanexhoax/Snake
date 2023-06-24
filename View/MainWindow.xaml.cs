using SnakeGame.Enum;
using SnakeGame.Model;
using SnakeGame.Modules;
using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        public const int HorizontalCellNumber = 15;
        public const int VerticalCellNumber = 10;
        public const int CellWidth = 40;
        public const int HorizontalLength = HorizontalCellNumber * CellWidth;
        public const int VerticalLength = VerticalCellNumber * CellWidth;
        public const int StartSpeed = 500;

        private const string FileName = "BestScore.txt";

        private Point _startPos;
        private int _startLength;
        private Snake? _snake;
        private Apple? _apple;
        private Timer? _movingTimer;
        private bool _gameStarted;
        private int _points;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                string fileData = File.ReadAllText(FileName);

                if (int.TryParse(fileData, out int result))
                {
                    best.Text = fileData;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch
            {
                best.Text = _points.ToString();
            }
        }

        private void Start()
        {
            Refresh();
            InitializeTimer();
            RenderSnake();
            SpawnApple();

            _gameStarted = true;
        }

        private void Stop()
        {
            _gameStarted = false;

            if (_snake is not null)
            {
                _snake.Moved -= OnSnakeMoved;
                _snake.Died -= OnSnakeDied;
                _snake.Increased -= OnSnakeIncreased;
            }

            _movingTimer?.Stop();

            if (_points > int.Parse(best.Text))
            {
                File.WriteAllText(FileName, _points.ToString());
                best.Text = _points.ToString(); 
            }
        }

        private void Restart()
        {
            Stop();
            Start();
        }

        private void Refresh()
        {
            canvas.Children.RemoveRange(0, canvas.Children.Count);

            _points = 0;
            current.Text = _points.ToString();
        }

        private void InitializeTimer()
        {
            _movingTimer = new Timer(StartSpeed);
            _movingTimer.AutoReset = true;
            _movingTimer.Elapsed += TimerTicked;
            _movingTimer.Start();
        }

        private void TimerTicked(object? sender, ElapsedEventArgs e) => Dispatcher.Invoke(() => _snake?.Move());

        private void RenderSnake()
        {
            _startPos = new Point(HorizontalLength / 2 - 20, VerticalLength / 2);
            _startLength = HorizontalLength / 5 / 40;
            _snake = new Snake(_startPos, _startLength, canvas);

            _snake.Moved += OnSnakeMoved;
            _snake.Died += OnSnakeDied;
            _snake.Increased += OnSnakeIncreased;
        }

        private void SpawnApple()
        {
            _apple = new Apple();

            canvas.Children.Add(_apple.Circle);
            _apple.Spawn(_snake!.PositionCollection);
        }

        private void OnSnakeIncreased(object? sender, EventArgs e)
        {
            _points++;
            current.Text = _points.ToString();

            if (_movingTimer is not null)
                _movingTimer.Interval -= 5;
        }

        private void OnSnakeDied(object? sender, EventArgs e) => Stop();

        private void OnSnakeMoved(object? sender, SnakeMovedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_snake is not null && _apple is not null && _snake.HeadPosition == _apple.Position)
                {
                    _snake.Increase();
                    _apple.Spawn(_snake.PositionCollection);
                }
            });
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            MoveDirection direction;

            switch (e.Key)
            {
                case Key.A:
                    direction = MoveDirection.Left;
                    break;
                case Key.W:
                    direction = MoveDirection.Top;
                    break;
                case Key.D:
                    direction = MoveDirection.Right;
                    break;
                case Key.S:
                    direction = MoveDirection.Bottom;
                    break;
                case Key.R:
                    Restart();
                    return;
                default:
                    return;
            }

            if (_gameStarted && _snake!.Turn(direction))
            {
                _movingTimer?.Stop();
                _movingTimer?.Start();
            }
        }

        private void OnRestartClicked(object sender, RoutedEventArgs e) => Restart();

        private void OnWindowLoaded(object sender, RoutedEventArgs e) => Start();
    }
}
