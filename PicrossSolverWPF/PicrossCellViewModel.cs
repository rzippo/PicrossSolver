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
    public class PicrossCellViewModel : DependencyObject 
    {


        public Brush FillBrush
        {
            get { return (Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register("FillBrush", typeof(Brush), typeof(PicrossCellViewModel), new PropertyMetadata(Brushes.Transparent));
        
        public PicrossCell Cell { get; set; }

        public PicrossCellViewModel(PicrossCell observedCell)
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
                    FillBrush = Brushes.Transparent;
                    break;
                case PicrossCellState.Void:
                    FillBrush = Brushes.White;
                    break;
                case PicrossCellState.Filled:
                    FillBrush = Brushes.Green;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
