using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public partial class PicrossLine
    {
        public IEnumerable<PicrossCell> Cells { get; }
        public int Length => Cells.Count();

        private static IEnumerable<PicrossCell> FillGap(int gapSize)
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

        private static IEnumerable<PicrossCell> FillBlock(int blockSize)
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
                throw new ArgumentException();

            var activeLineDeterminedIndexes = Enumerable.Range(0, activeLine.Count() - 1)
                .Where(lineIndex => activeLine.ElementAt(lineIndex) != PicrossCellState.Undetermined);

            return activeLineDeterminedIndexes.All(lineIndex => Cells.ElementAt(lineIndex).State == activeLine.ElementAt(lineIndex).State);
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
