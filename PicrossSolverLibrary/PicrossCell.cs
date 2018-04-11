using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PicrossSolverLibrary
{
    public class PicrossCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private PicrossCellState state = PicrossCellState.Void;
        public PicrossCellState State
        {
            get => state;
            set
            {
                if (value != state)
                {
                    state = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public static implicit operator PicrossCellState(PicrossCell cell)
        {
            return cell.State;
        }
    }
}
