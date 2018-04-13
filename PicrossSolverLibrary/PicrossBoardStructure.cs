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
            for (int rowIndex = 0; rowIndex < ColumnCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < RowCount; columnIndex++)
                {
                    Matrix[rowIndex, columnIndex] = new PicrossCell()
                    {
                        Row = rowIndex,
                        Column = columnIndex
                    };
                }
            }

            Columns = GatherColumns();
            Rows = GatherRows();
            ActiveLines = (new[] {Columns, Rows}).SelectMany(collection => collection);
        }

        public PicrossBoard(PicrossBoard other)
        {
            Puzzle = other.Puzzle;
            RowCount = Puzzle.RowCount;
            ColumnCount = Puzzle.ColumnCount;

            Matrix = new PicrossCell[RowCount, ColumnCount];
            for (int rowIndex = 0; rowIndex < ColumnCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < RowCount; columnIndex++)
                {
                    PicrossCell otherCell = other.Matrix[rowIndex, columnIndex];
                    Matrix[rowIndex, columnIndex] = new PicrossCell()
                    {
                        State = otherCell.State,
                        Row = rowIndex,
                        Column = columnIndex
                    };
                }
            }

            Columns = GatherColumns();
            Rows = GatherRows();
            ActiveLines = (new[] { Columns, Rows }).SelectMany(collection => collection);
        }

        public void Copy(PicrossBoard other)
        {
            if (Puzzle != other.Puzzle)
                throw new ArgumentException();

            for (int rowIndex = 0; rowIndex < ColumnCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < RowCount; columnIndex++)
                {
                    PicrossCell otherCell = other.Matrix[rowIndex, columnIndex];
                    Matrix[rowIndex, columnIndex].State = otherCell.State;
                }
            }
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

                columns[columnIndex] = new PicrossActiveLine(
                    type: LineType.Column,
                    index: columnIndex,
                    cells: columnCells,
                    rule: columnRule);
            }

            return columns;
        }

        private PicrossActiveLine[] GatherRows()
        {
            var rows = new PicrossActiveLine[ColumnCount];

            for (int rowIndex = 0; rowIndex < ColumnCount; rowIndex++)
            {
                var rowCells = new List<PicrossCell>();
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    rowCells.Add(Matrix[rowIndex, columnIndex]);

                var rowRule =
                    new PicrossLineRule(Puzzle.RowRules[rowIndex], RowCount);

                rows[rowIndex] = new PicrossActiveLine(
                    type: LineType.Row,
                    index: rowIndex,
                    cells: rowCells,
                    rule: rowRule);
            }

            return rows;
        }

        private void SetLineSolution(LineType lineType, int lineIndex, PicrossLine candidateToSet)
        {
            PicrossActiveLine target;
            target = lineType == LineType.Column ?
                Columns.First(line => line.Index == lineIndex) 
                : Rows.First(line => line.Index == lineIndex);

            target.ApplyLine(candidateToSet);
        }
    }
}
