using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public class PicrossLineRule
    {
        public List<int> LineStructure { get; }
        public int LineLength { get; }

        public int BlockCount => LineStructure.Count;
        public int InnerBlockCount => BlockCount - 2;

        public int FilledCells => LineStructure.Sum();

        public int MinGaps
        {
            get
            {
                if(BlockCount == 1)
                {
                    if (FilledCells == LineLength)
                        return 0;
                    else
                        return 1;
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
                if (FilledCells + MinGaps < LineLength)
                {
                    return MinGaps + Math.Min(LineLength - (FilledCells + MinGaps), 2);
                }
                else
                {
                    return MinGaps;
                }
            }
        }

        public bool IsTrivial => MinGaps == MaxGaps;

        public IEnumerable<bool> TrivialSolution()
        {
            if (!IsTrivial)
                return null;

            bool[] solution = new bool[LineLength];
            int lineIndex = 0;
            for (int blockIndex = 0; blockIndex < BlockCount; blockIndex++)
            {
                for (int fillingIndex = 0; fillingIndex < LineStructure[blockIndex]; fillingIndex++)
                {
                    solution[lineIndex] = true;
                    lineIndex++;
                }
                lineIndex++;
            }
            return solution;
        }


        public PicrossLineRule(IEnumerable<int> lineStructure, int lineLength)
        {
            LineStructure = new List<int>(lineStructure);
            LineLength = lineLength;
        }

        public bool IsLegal()
        {
            //todo: implement IsLegal()
            return true;
        }

        public bool Validate(IEnumerable<bool> line)
        {
            //todo: implement Validate()
            return true;
        }
    }
}
