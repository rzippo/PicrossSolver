﻿using System;
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
