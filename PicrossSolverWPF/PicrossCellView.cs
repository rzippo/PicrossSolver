using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PicrossSolverLibrary;

namespace PicrossSolverWPF
{
    public class PicrossCellView : DependencyObject 
    {


        public Brush CellFillBrush
        {
            get { return (Brush)GetValue(CellFillBrushProperty); }
            set { SetValue(CellFillBrushProperty, value); }
        }

        public static readonly DependencyProperty CellFillBrushProperty =
            DependencyProperty.Register("CellFillBrush", typeof(Brush), typeof(PicrossCellView), new PropertyMetadata(Brushes.Transparent));
        
        public PicrossCell Cell { get; set; }

        public PicrossCellView(PicrossCell observedCell)
        {
            Cell = observedCell;
            Cell.PropertyChanged += CellChangedHandler;
        }

        private void CellChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => UpdateRectangle()));
        }

        private void UpdateRectangle()
        {
            switch (Cell.State)
            {
                case PicrossCellState.Undetermined:
                    CellFillBrush = Brushes.Transparent;
                    break;
                case PicrossCellState.Void:
                    CellFillBrush = Brushes.White;
                    break;
                case PicrossCellState.Filled:
                    CellFillBrush = Brushes.Green;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
