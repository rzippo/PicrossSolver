using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        public PicrossCellState[,] Matrix { get; }
        public PicrossLineRule[] ColumnRules { get; }
        public PicrossLineRule[] RowRules { get; }

        public int RowCount { get; }
        public int ColumnCount { get; }

        public PicrossBoard(PicrossPuzzle puzzle)
        {
            RowCount = puzzle.RowCount;
            ColumnCount = puzzle.ColumnCount;
            Matrix = new PicrossCellState[RowCount, ColumnCount];

            ColumnRules = new PicrossLineRule[ColumnCount];
            RowRules = new PicrossLineRule[RowCount];

            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                ColumnRules[columnIndex] =
                    new PicrossLineRule(puzzle.ColumnRules[columnIndex], RowCount);
            }

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                RowRules[rowIndex] =
                    new PicrossLineRule(puzzle.RowRules[rowIndex], ColumnCount);
            }
        }
    }
}
