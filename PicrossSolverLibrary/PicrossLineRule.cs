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
        public bool IsEmpty => BlocksRule.SequenceEqual(new int[]{0});
        public int BlockCount => BlocksRule.Count;
        public int OuterBlockCount => (BlockCount == 1) ? 1 : 2;
        public int InnerBlockCount => BlockCount - OuterBlockCount;

        public int FilledCellCount => BlocksRule.Sum();
        public int VoidCellCount => LineLength - FilledCellCount;
        public int MinRequiredSpace => FilledCellCount + MinGaps;

        public int MinGaps
        {
            get
            {
                if(BlockCount == 1)
                {
                    return FilledCellCount == LineLength ? 0 : 1;
                }
                else
                {
                    return InnerBlockCount + 1;
                }
            }
        }

        public int InnerGapCount => BlockCount == 1 ? 0 : InnerBlockCount + 1;

        public int MaxGaps
        {
            get
            {
                if (MinRequiredSpace < LineLength)
                {
                    return MinGaps + Math.Min(LineLength - MinRequiredSpace, OuterBlockCount);
                }
                else
                {
                    return MinGaps;
                }
            }
        }

        public bool IsLegal => MinRequiredSpace <= LineLength;

        public bool IsTrivial => IsEmpty || (IsLegal && MinGaps == MaxGaps);

        public IEnumerable<PicrossCellState> TrivialSolution()
        {
            if (!IsTrivial)
                return null;

            if(IsEmpty)
                return (new PicrossLine(LineLength, PicrossCellState.Void)).Cells.Select(cell => cell.State);

            PicrossCellState[] solution = new PicrossCellState[LineLength];
            int lineIndex = 0;
            for (int blockIndex = 0; blockIndex < BlockCount; blockIndex++)
            {
                for (int fillingIndex = 0; fillingIndex < BlocksRule[blockIndex]; fillingIndex++)
                {
                    solution[lineIndex] = PicrossCellState.Filled;
                    lineIndex++;
                }
                if (blockIndex < BlockCount - 1)
                    solution[lineIndex] = PicrossCellState.Void;
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
            if (BlockCount <= lineBlocks.Count())
                return false;
            else
            {
                return Enumerable.Range(0, lineBlocks.Count() - 1).All(blockIndex =>
                    lineBlocks.ElementAt(blockIndex) <= BlocksRule.ElementAt(blockIndex));
            }
        }

        public bool CheckSolution(PicrossLine line)
        {
            var lineBlocks = line.ComputeBlocks();
            if (BlockCount != lineBlocks.Count())
                return false;
            else
            {
                return Enumerable.Range(0, lineBlocks.Count() - 1).All(blockIndex =>
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
            var generatedGaps = GenerateGapStructures(gapRules, VoidCellCount);
            return GenerateLinesFromGapStructures(generatedGaps);
        }

        protected IEnumerable<Tuple<int, int>> GetGapRules()
        {
            int voidsToAllocate = LineLength - FilledCellCount - InnerGapCount;
            var gapRules = new List<Tuple<int, int>>();

            //Left outer gap
            gapRules.Add(new Tuple<int, int>(0, voidsToAllocate));

            //Inner gapStructures
            for (int innerGapIndex = 0; innerGapIndex < InnerGapCount; innerGapIndex++)
                gapRules.Add(new Tuple<int, int>(1, 1 + voidsToAllocate));

            //Right outer gap
            gapRules.Add(new Tuple<int, int>(0, voidsToAllocate));

            return gapRules;
        }

        protected IEnumerable<IEnumerable<int>> GenerateGapStructures(IEnumerable<Tuple<int, int>> gapRules, int gapsToBeAllocated)
        {
            if (gapRules.Sum(gapRule => gapRule.Item2) < gapsToBeAllocated)
                return null;

            var gapStructures = new List<IEnumerable<int>>();
            var headRule = gapRules.First();
            var headValues = Enumerable.Range(
                headRule.Item1,
                (headRule.Item2 - headRule.Item1) + 1);

            foreach (int headValue in headValues)
            {
                var innerGapRules = gapRules.Skip(1);
                int nextGapsToBeAllocated = gapsToBeAllocated - headValue;
                if (nextGapsToBeAllocated >= 0)
                {
                    if (innerGapRules.Count() == 1)
                    {
                        var gapStructure = new List<int>() {headValue, nextGapsToBeAllocated};
                        gapStructures.Add(gapStructure);
                    }
                    else
                    {
                        var innerGaps = GenerateGapStructures(innerGapRules, nextGapsToBeAllocated);
                        if (innerGaps != null)
                        {
                            foreach (var innerGap in innerGaps)
                            {
                                var gapStructure = new List<int>() {headValue};
                                gapStructure.AddRange(innerGap);
                                gapStructures.Add(gapStructure);
                            }
                        }
                    }
                }
            }
            return gapStructures;
        }

        protected IEnumerable<PicrossLine> GenerateLinesFromGapStructures(IEnumerable<IEnumerable<int>> gapStructures)
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
