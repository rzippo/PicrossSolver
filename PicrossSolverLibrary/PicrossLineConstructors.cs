using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public partial class PicrossLine
    {
        public PicrossLine(int length, PicrossCellState state)
        {
            var cells = new PicrossCell[length];
            for (int i = 0; i < length; i++)
            {
                cells[i] = new PicrossCell()
                {
                    State = state
                };
            }
            Cells = cells;
        }

        public PicrossLine(IEnumerable<PicrossCell> cells)
        {
            Cells = cells;
        }

        public PicrossLine(IEnumerable<PicrossCellState> cellStates)
        {
            var cells = new List<PicrossCell>();
            foreach (var cellState in cellStates)
            {
                cells.Add(new PicrossCell()
                {
                    State = cellState
                });
            }
            Cells = cells;
        }

        public PicrossLine(PicrossLine other)
        {
            var cells = new PicrossCell[other.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new PicrossCell()
                {
                    State = other.Cells.ElementAt(i).State
                };
            }
            Cells = cells;
        }

        public PicrossLine(IEnumerable<int> blocksRule, IEnumerable<int> gap)
        {
            if (gap.Count() != blocksRule.Count() + 1)
                throw new ArgumentException();

            var cells = new List<PicrossCell>();

            for (int pairIndex = 0; pairIndex < blocksRule.Count(); pairIndex++)
            {
                cells.AddRange(FillGap(gap.ElementAt(pairIndex)));
                cells.AddRange(FillBlock(blocksRule.ElementAt(pairIndex)));
            }

            cells.AddRange(FillGap(gap.Last()));

            Cells = cells;
        }
    }
}
