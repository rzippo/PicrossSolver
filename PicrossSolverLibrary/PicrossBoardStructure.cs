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

        public static PicrossBoard GetEmpty() => new PicrossBoard(PicrossPuzzle.GetEmpty());

        public PicrossBoard(PicrossPuzzle puzzle)
        {
            Puzzle = puzzle;

            RowCount = Puzzle.RowCount;
            ColumnCount = Puzzle.ColumnCount;

            Matrix = new PicrossCell[RowCount, ColumnCount];
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
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

        public PicrossBoard(PicrossBoard copySource)
        {
            Puzzle = copySource.Puzzle;
            RowCount = Puzzle.RowCount;
            ColumnCount = Puzzle.ColumnCount;

            Matrix = new PicrossCell[RowCount, ColumnCount];
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    PicrossCell otherCell = copySource.Matrix[rowIndex, columnIndex];
                    Matrix[rowIndex, columnIndex] = new PicrossCell()
                    {
                        State = otherCell.State,
                        Row = rowIndex,
                        Column = columnIndex
                    };
                }
            }

            Columns = CopyColumns(copySource);
            Rows = CopyRows(copySource);
            ActiveLines = (new[] { Columns, Rows }).SelectMany(collection => collection);
        }

        public void Copy(PicrossBoard source)
        {
            if (Puzzle != source.Puzzle)
                throw new ArgumentException();

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    PicrossCell otherCell = source.Matrix[rowIndex, columnIndex];
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
                    new PicrossLineRule(
                        lineStructure: Puzzle.ColumnRules[columnIndex],
                        lineLength: RowCount);

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
            var rows = new PicrossActiveLine[RowCount];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                var rowCells = new List<PicrossCell>();
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    rowCells.Add(Matrix[rowIndex, columnIndex]);

                var rowRule =
                    new PicrossLineRule(
                        lineStructure: Puzzle.RowRules[rowIndex], 
                        lineLength: ColumnCount);

                rows[rowIndex] = new PicrossActiveLine(
                    type: LineType.Row,
                    index: rowIndex,
                    cells: rowCells,
                    rule: rowRule);
            }

            return rows;
        }

        private PicrossActiveLine[] CopyColumns(PicrossBoard copySource)
        {
            var columns = new PicrossActiveLine[ColumnCount];

            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                var columnCells = new List<PicrossCell>();
                for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
                    columnCells.Add(Matrix[rowIndex, columnIndex]);

                columns[columnIndex] = new PicrossActiveLine(
                    cells: columnCells,
                    copySource: copySource.Columns.ElementAt(columnIndex));
            }

            return columns;
        }

        private PicrossActiveLine[] CopyRows(PicrossBoard copySource)
        {
            var rows = new PicrossActiveLine[RowCount];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                var rowCells = new List<PicrossCell>();
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    rowCells.Add(Matrix[rowIndex, columnIndex]);

                rows[rowIndex] = new PicrossActiveLine(
                    cells: rowCells,
                    copySource: copySource.Rows.ElementAt(rowIndex));
            }

            return rows;
        }

        private void SetLineSolution(LineType lineType, int lineIndex, PicrossLine candidateToSet)
        {
            var targetSet = lineType == LineType.Column ? Columns : Rows;
            PicrossActiveLine target = targetSet.First(line => line.Index == lineIndex);

            target.ApplyLine(candidateToSet);
        }
    }
}
