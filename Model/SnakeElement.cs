using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.Model
{
    internal class SnakeElement
    {
        private System.Windows.Point _position;

        public System.Windows.Point Position
        {
            get => _position;
            set
            {
                PrevPosition = _position;
                _position = value;
            }
        }
        public System.Windows.Point PrevPosition { get; set; }
        public System.Windows.Shapes.Rectangle Rectangle { get; set; }
        public bool IsHead { get; set; }

        public SnakeElement(System.Windows.Point point, System.Windows.Shapes.Rectangle rectangle, bool isHead)
        {
            Position = point;
            Rectangle = rectangle;
            IsHead = isHead;
        }
    }
}
