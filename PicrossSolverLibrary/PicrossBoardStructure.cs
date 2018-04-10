using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        public PicrossCell[,] Matrix { get; }

        public PicrossActiveLine[] Columns { get; }
        public PicrossActiveLine[] Rows { get; }

        public IEnumerable<PicrossActiveLine> ActiveLines { get; }

        public int RowCount { get; }
        public int ColumnCount { get; }

        public PicrossPuzzle Puzzle { get; }

        public PicrossBoard(PicrossPuzzle puzzle)
        {
            Puzzle = puzzle;

            RowCount = Puzzle.RowCount;
            ColumnCount = Puzzle.ColumnCount;
            Matrix = new PicrossCell[RowCount, ColumnCount];

            Columns = GatherColumns();
            Rows = GatherRows();
            ActiveLines = (new[] {Columns, Rows}).SelectMany(collection => collection);
        }

        private PicrossActiveLine[] GatherColumns()
        {
            var columns = new PicrossActiveLine[ColumnCount];

            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                var columnCells = new List<PicrossCell>();
                for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                    columnCells.Add(Matrix[rowIndex, columnIndex]);

                var columnRule =
                    new PicrossLineRule(Puzzle.ColumnRules[columnIndex], RowCount);

                columns[columnIndex] = new PicrossActiveLine(columnCells, columnRule);
            }

            return columns;
        }

        private PicrossActiveLine[] GatherRows()
        {
            var rows = new PicrossActiveLine[ColumnCount];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                var rowCells = new List<PicrossCell>();
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    rowCells.Add(Matrix[rowIndex, columnIndex]);

                var rowRule =
                    new PicrossLineRule(Puzzle.RowRules[rowIndex], RowCount);

                rows[rowIndex] = new PicrossActiveLine(rowCells, rowRule);
            }

            return rows;
        }
    }
}
