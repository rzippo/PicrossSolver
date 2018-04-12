using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using PicrossSolverLibrary;

namespace PicrossSolverWPF
{
    public class PicrossCellView   
    {
        public Rectangle Rectangle { get; set; }
        public PicrossCell Cell { get; set; }

        public PicrossCellView(PicrossCell observedCell)
        {
            Rectangle = new Rectangle()
            {
                Height = 5,
                Width = 5,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Cell = observedCell;
            Cell.PropertyChanged += UpdateRectangle;
        }

        private void UpdateRectangle(object sender, PropertyChangedEventArgs e)
        {
            switch (Cell.State)
            {
                case PicrossCellState.Void:
                    Rectangle.Fill = Brushes.White;
                    break;
                case PicrossCellState.Filled:
                    Rectangle.Fill = Brushes.Green;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
