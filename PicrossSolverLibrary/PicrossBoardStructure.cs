using System;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        public bool[][] Matrix { get; }
        public PicrossLineRule[] ColumnRules { get; }
        public PicrossLineRule[] RowRules { get; }

        public PicrossBoard(string json)
        {
            //todo: implement PicrossBoard parser from json
        }
    }
}
