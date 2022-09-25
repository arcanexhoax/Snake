using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    internal class Apple
    {
        private Random _random;
        private List<Point> _probablyPoints;

        public Ellipse Circle { get; set; }
        public Point Position { get; set; }

        public Apple()
        {
            _random = new();

            Circle = new()
            {
                Fill = Brushes.Red,
                Width = MainWindow.CellWidth,
                Height = MainWindow.CellWidth
            };
        }

        public void Spawn(List<Point> exceptPoints)
        {
            _probablyPoints = new List<Point>(MainWindow.HorizontalCellNumber * MainWindow.VerticalCellNumber);

            for (int i = 0; i < MainWindow.HorizontalCellNumber; i++)
            {
                for (int j = 0; j < MainWindow.VerticalCellNumber; j++)
                {
                    _probablyPoints.Add(new Point(i * MainWindow.CellWidth, j * MainWindow.CellWidth));
                }
            }

            _probablyPoints.RemoveAll(p => exceptPoints.Any(point => point.X == p.X && point.Y == p.Y));

            Position = _probablyPoints[_random.Next(0, _probablyPoints.Count - 1)];

            Canvas.SetLeft(Circle, Position.X);
            Canvas.SetTop(Circle, Position.Y);
        }
    }
}
