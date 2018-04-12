using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public class PicrossLine
    {
        public IEnumerable<PicrossCell> Cells { get; }
        public int Length => Cells.Count();

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
            for (int i = 0; i < Length; i++)
            {
                cells[i] = new PicrossCell()
                {
                    State = other.Cells.ElementAt(i).State
                };
            }
        }
        
        public PicrossLine(IEnumerable<int> blocksRule, IEnumerable<int> gap)
        {
            if(gap.Count() != blocksRule.Count() + 1)
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

        private IEnumerable<PicrossCell> FillGap(int gapSize)
        {
            var cells = new List<PicrossCell>();

            for (int gapInnerIndex = 0; gapInnerIndex < gapSize; gapInnerIndex++)
            {
                cells.Add(new PicrossCell()
                {
                    State = PicrossCellState.Void
                });
            }
            return cells;
        }

        private IEnumerable<PicrossCell> FillBlock(int blockSize)
        {
            var cells = new List<PicrossCell>();

            for (int blockInnerIndex = 0; blockInnerIndex < blockSize; blockInnerIndex++)
            {
                cells.Add(new PicrossCell()
                {
                    State = PicrossCellState.Filled
                });
            }
            return cells;
        }

        public IEnumerable<int> ComputeBlocks()
        {
            List<int> lineBlocks = new List<int>();

            int nextBlock = 0;
            bool blockActive = false;

            for (int lineIndex = 0; lineIndex < Cells.Count(); lineIndex++)
            {
                if (Cells.ElementAt(lineIndex) == PicrossCellState.Filled)
                {
                    if (blockActive)
                        lineBlocks[nextBlock - 1]++;
                    else
                    {
                        lineBlocks.Add(1);
                        nextBlock++;
                        blockActive = true;
                    }
                }
                else
                {
                    if (blockActive)
                    {
                        blockActive = false;
                    }
                }
            }

            return lineBlocks;
        }


        //This function should check if this line is a valid speculative candidate for the line we pass as a parameter
        //This does not count validity - we suppose that the speculative line was generated from the rule
        public bool IsCandidate(IEnumerable<PicrossCell> activeLine)
        {
            if (activeLine.Count() != Cells.Count())
                return false;
            else
            {
                var activeLineFilledIndexes = Enumerable.Range(0, activeLine.Count() - 1)
                    .Where(lineIndex => activeLine.ElementAt(lineIndex) == PicrossCellState.Filled);

                return activeLineFilledIndexes.All(lineIndex => Cells.ElementAt(lineIndex) == PicrossCellState.Filled);
            }
        }

        public void And(PicrossLine otherLine)
        {
            if (Length != otherLine.Length)
                throw new ArgumentException();

            for (int i = 0; i < Length; i++)
            {
                var cell = Cells.ElementAt(i);
                var otherCell = otherLine.Cells.ElementAt(i);
                cell.State = cell.State.And(otherCell.State);
            }
        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PicrossCell cell in Cells)
            {
                switch(cell.State)
                {
                    case PicrossCellState.Undetermined:
                        sb.Append(" ? ");
                        break;
                    case PicrossCellState.Void:
                        sb.Append("   ");
                        break;
                    case PicrossCellState.Filled:
                        sb.Append(" ■ ");
                        break;
                }
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
