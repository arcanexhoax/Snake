using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snake.Model
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
