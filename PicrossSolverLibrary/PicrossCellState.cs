using System;
using System.Collections.Generic;
using System.Text;

namespace PicrossSolverLibrary
{
    public enum PicrossCellState
    {
        Undetermined,
        Void,
        Filled
    }

    public static class PicrossCellStateMethods
    {
        public static PicrossCellState And(this PicrossCellState s1, PicrossCellState s2)
        {
            if (s1 == s2)
                return s1;
            else
                return PicrossCellState.Undetermined;
        }
    }
}

