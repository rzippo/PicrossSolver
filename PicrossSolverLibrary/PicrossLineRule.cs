using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public class PicrossLineRule
    {
        public List<int> BlocksRule { get; }
        public int LineLength { get; }

        public int BlockCount => BlocksRule.Count;
        public int InnerBlockCount => BlockCount - 2;

        public int FilledCells => BlocksRule.Sum();
        public int MinRequiredSpace => FilledCells + MinGaps;

        public int MinGaps
        {
            get
            {
                if(BlockCount == 1)
                {
                    return FilledCells == LineLength ? 0 : 1;
                }
                else
                {
                    return InnerBlockCount + 1;
                }
            }
        }

        public int MaxGaps
        {
            get
            {
                if (MinRequiredSpace < LineLength)
                {
                    return MinGaps + Math.Min(LineLength - MinRequiredSpace, 2);
                }
                else
                {
                    return MinGaps;
                }
            }
        }

        public bool IsLegal => MinRequiredSpace <= LineLength;

        public bool IsTrivial => IsLegal && MinGaps == MaxGaps;

        public IEnumerable<PicrossCellState> TrivialSolution()
        {
            if (!IsTrivial)
                return null;

            PicrossCellState[] solution = new PicrossCellState[LineLength];
            int lineIndex = 0;
            for (int blockIndex = 0; blockIndex < BlockCount; blockIndex++)
            {
                for (int fillingIndex = 0; fillingIndex < BlocksRule[blockIndex]; fillingIndex++)
                {
                    solution[lineIndex] = PicrossCellState.Filled;
                    lineIndex++;
                }
                lineIndex++;
            }
            return solution;
        }


        public PicrossLineRule(IEnumerable<int> lineStructure, int lineLength)
        {
            BlocksRule = new List<int>(lineStructure);
            LineLength = lineLength;
        }

        
        public bool Validate(IEnumerable<PicrossCellState> line)
        {
            return BlockByBlockValidate(line);

            //todo: find a more clever algorithm that computes possible connections and available space
        }

        private bool BlockByBlockValidate(IEnumerable<PicrossCellState> line)
        {
            var lineBlocks = ComputeBlocks(line);
            return BlockCount == lineBlocks.Count() &&
                   Enumerable.Range(0, BlockCount).All(blockIndex =>
                       lineBlocks.ElementAt(blockIndex) <= BlocksRule.ElementAt(blockIndex));
        }

        private IEnumerable<int> ComputeBlocks(IEnumerable<PicrossCellState> line)
        {
            List<int> lineBlocks = new List<int>();

            int nextBlock = 0;
            bool blockActive = false;

            for (int lineIndex = 0; lineIndex < line.Count(); lineIndex++)
            {
                if (line.ElementAt(lineIndex) == PicrossCellState.Filled)
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
    }
}
