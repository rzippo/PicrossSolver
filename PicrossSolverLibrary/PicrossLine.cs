using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public class PicrossLine
    {
        public IEnumerable<PicrossCell> Cells { get; }

        public PicrossLine(IEnumerable<PicrossCell> cells)
        {
            Cells = cells;
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
        //todo: find better names
        public bool IsCandidate(IEnumerable<PicrossCell> matchingLine)
        {
            if (matchingLine.Count() != Cells.Count())
                return false;
            else
            {
                var matchingFilledIndexes = Enumerable.Range(0, matchingLine.Count())
                    .Where(lineIndex => matchingLine.ElementAt(lineIndex) == PicrossCellState.Filled);

                return matchingFilledIndexes.All(lineIndex => Cells.ElementAt(lineIndex) == PicrossCellState.Filled);
            }
        }
    }
}
