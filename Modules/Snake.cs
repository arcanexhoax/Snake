using SnakeGame.Enum;
using SnakeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.Modules
{
    internal class Snake
    {
        private readonly List<SnakeElement> _elements;
        private readonly Canvas _canvas;

        private MoveDirection _direction;

        public Point HeadPosition => _elements[0].Position;
        public List<Point> PositionCollection { get; set; }
        public bool IsDead { get; set; }

        public event EventHandler<SnakeMovedEventArgs>? Moved;
        public event EventHandler? Died;
        public event EventHandler? Increased;

        public Snake(Point position, int length, Canvas canvas)
        {
            _elements = new List<SnakeElement>(length);
            _canvas = canvas;
            _direction = MoveDirection.Right;

            for (int i = 0; i < length; i++)
            {
                Rectangle rectangle = InitializeElement(i);
                Point elementPos = new(position.X - MainWindow.CellWidth * i, position.Y);

                Canvas.SetLeft(rectangle, elementPos.X);
                Canvas.SetTop(rectangle, elementPos.Y);

                _elements.Add(new SnakeElement(elementPos, rectangle, i == 0));
            }

            PositionCollection = _elements.Select(p => p.Position).ToList();
        }

        private Rectangle InitializeElement(int number)
        {
            Rectangle rectangle = new()
            {
                Width = MainWindow.CellWidth,
                Height = MainWindow.CellWidth,
                Fill = new SolidColorBrush(Color.FromArgb((byte)(255 - number * 2), 0, 0, 0))
            };

            _canvas.Children.Add(rectangle);

            return rectangle;
        }

        public void Move()
        {
            if (IsDead)
            {
                return;
            }
            if (!CanMove())
            {
                Die();
                return;
            }

            for (int i = 0; i < _elements.Count; i++)
            {
                if (!_elements[i].IsHead)
                {
                    _elements[i].Position = _elements[i - 1].PrevPosition;
                }
                else
                {
                    _elements[i].Position = GetNextPosition(_elements[i].Position);
                }

                Canvas.SetLeft(_elements[i].Rectangle, _elements[i].Position.X);
                Canvas.SetTop(_elements[i].Rectangle, _elements[i].Position.Y);
            }

            Moved?.Invoke(this, new SnakeMovedEventArgs(HeadPosition));
        }

        public bool Turn(MoveDirection direction)
        {
            if (direction != _direction && (int)direction % 2 == (int)_direction % 2)
            {
                return false;
            }

            _direction = direction;

            Move();

            return true;
        }

        public void Increase()
        {
            Rectangle rectangle = InitializeElement(_elements.Count);

            Canvas.SetLeft(rectangle, _elements.Last().PrevPosition.X);
            Canvas.SetTop(rectangle, _elements.Last().PrevPosition.Y);

            _elements.Add(new SnakeElement(_elements.Last().PrevPosition, rectangle, false));

            PositionCollection = _elements.Select(p => p.Position).ToList();

            Increased?.Invoke(this, EventArgs.Empty);
        }

        public void Die()
        {
            IsDead = true;
            Died?.Invoke(this, EventArgs.Empty);

            _elements.ForEach(e => e.Rectangle.Fill = Brushes.Red);
        }

        public bool CanMove()
        {
            Point nextPos = GetNextPosition(HeadPosition);

            bool isInsideBounds = _direction switch
            {
                MoveDirection.Left => nextPos.X >= 0,
                MoveDirection.Top => nextPos.Y >= 0,
                MoveDirection.Right => nextPos.X < MainWindow.HorizontalLength,
                MoveDirection.Bottom => nextPos.Y < MainWindow.VerticalLength,
                _ => true,
            };

            if (!isInsideBounds)
            {
                return false;
            }

            return !_elements.Any(p => p.Position == HeadPosition && !p.IsHead);
        }

        public Point GetNextPosition(Point current)
        {
            return _direction switch
            {
                MoveDirection.Left => new Point(current.X - MainWindow.CellWidth, current.Y),
                MoveDirection.Top => new Point(current.X, current.Y - MainWindow.CellWidth),
                MoveDirection.Right => new Point(current.X + MainWindow.CellWidth, current.Y),
                MoveDirection.Bottom => new Point(current.X, current.Y + MainWindow.CellWidth),
                _ => throw new NotImplementedException()
            };
        }
    }
}
