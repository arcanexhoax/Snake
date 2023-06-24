using System;
using System.Windows;

namespace SnakeGame.Model
{
    internal class SnakeMovedEventArgs : EventArgs
    {
        public Point NewHeadPosition { get; set; }

        public SnakeMovedEventArgs(Point point)
        {
            NewHeadPosition = point;
        }
    }
}
