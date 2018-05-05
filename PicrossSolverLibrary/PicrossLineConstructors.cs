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
            Cells = cells.ToArray();
        }

        public PicrossLine(IEnumerable<PicrossCellState> cellStates)
        {
            Cells = new PicrossCell[cellStates.Count()];
            int i = 0;
            foreach (var cellState in cellStates)
            {
                Cells[i] = new PicrossCell()
                {
                    State = cellState
                };
                i++;
            }            
        }

        public PicrossLine(PicrossLine copySource)
        {
            Cells = new PicrossCell[copySource.Length];
            for (int i = 0; i < Length; i++)
            {
                Cells[i] = new PicrossCell()
                {
                    State = copySource.Cells[i].State
                };
            }
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

            Cells = cells.ToArray();
        }
    }
}
