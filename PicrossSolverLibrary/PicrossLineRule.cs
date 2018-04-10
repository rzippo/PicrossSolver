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

        
        public bool Validate(PicrossLine line)
        {
            var lineBlocks = line.ComputeBlocks();
            if (BlockCount == lineBlocks.Count())
                return false;
            else
            {
                return Enumerable.Range(0, BlockCount).All(blockIndex =>
                    lineBlocks.ElementAt(blockIndex) <= BlocksRule.ElementAt(blockIndex));
            }
        }

        public bool CheckSolution(PicrossLine line)
        {
            var lineBlocks = line.ComputeBlocks();
            if (BlockCount == lineBlocks.Count())
                return false;
            else
            {
                return Enumerable.Range(0, BlockCount).All(blockIndex =>
                    lineBlocks.ElementAt(blockIndex) == BlocksRule.ElementAt(blockIndex));
            }
        }

        public IEnumerable<PicrossLine> GenerateCandidates()
        {
            if (IsTrivial)
                return new List<PicrossLine>()
                {
                    new PicrossLine(TrivialSolution())
                };

            var gapRules = GetGapRules();
            var generatedGaps = GenerateGapStructures(gapRules);
            return GenerateLinesFromGapStructures(generatedGaps);
        }

        private IEnumerable<Tuple<int, int>> GetGapRules()
        {
            int voidsToAllocate = LineLength - FilledCells - MinGaps;
            var gapRules = new List<Tuple<int, int>>();

            //Left outer gap
            gapRules.Add(new Tuple<int, int>(0, voidsToAllocate));

            //Inner gapStructures
            for (int innerGapIndex = 0; innerGapIndex < MinGaps; innerGapIndex++)
                gapRules.Add(new Tuple<int, int>(1, 1 + voidsToAllocate));

            //Right outer gap
            gapRules.Add(new Tuple<int, int>(0, voidsToAllocate));

            return gapRules;
        }

        private IEnumerable<IEnumerable<int>> GenerateGapStructures(IEnumerable<Tuple<int, int>> gapRules)
        {
            if(gapRules.Count() == 0)
                return new List<IEnumerable<int>>() {new List<int>()};

            var gapStructures = new List<IEnumerable<int>>();
            var headRule = gapRules.First();
            foreach (int headValue in Enumerable.Range(headRule.Item1, headRule.Item2))
            {
                var innerGapRules = gapRules.Skip(1);
                var innerGaps = GenerateGapStructures(innerGapRules);

                foreach (var innerGap in innerGaps)
                {
                    var gapStructure = new List<int>() {headValue};
                    gapStructure.AddRange(innerGap);
                    gapStructures.Add(gapStructure);
                }    
            }
            return gapStructures;
        }

        private IEnumerable<PicrossLine> GenerateLinesFromGapStructures(IEnumerable<IEnumerable<int>> gapStructures)
        {
            var lines = new List<PicrossLine>();
            foreach (var gapStructure in gapStructures)
            {
                lines.Add(new PicrossLine(BlocksRule, gapStructure));
            }
            return lines;
        }
    }
}
