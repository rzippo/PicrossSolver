using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public class SpeculativeCallContext
    {
        public int depth;
        public int optionIndex;
        public int optionsCount;
    }

    public enum VerboseLevel
    {
        Silent,
        StartDeclaration,
        StepByStep,
    }

    public partial class PicrossBoard
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsValid => ActiveLines.All(activeLine => activeLine.IsValid);
        public bool IsSet => ActiveLines.All(activeLine => activeLine.IsSet);
        public bool IsSolved => ActiveLines.All(activeLine => activeLine.IsSolved);

        public void Solve(
            VerboseLevel verboseLevel = VerboseLevel.Silent,
            SpeculativeCallContext context = null)
        {
            if (!IsValid)
            {
                if (verboseLevel != VerboseLevel.Silent)
                {
                    StringBuilder sb = new StringBuilder("Solving aborted, board invalid - ");
                    if (context == null)
                        sb.Append("main board");
                    else
                        sb.Append(
                            $"speculative board of depth {context.depth}, option {context.optionIndex} of {context.optionsCount}");
                    log.Info(sb);
                }
                return;
            }

            if(verboseLevel != VerboseLevel.Silent)
            {
                StringBuilder sb = new StringBuilder("Start solving - ");
                if (context == null)
                    sb.Append("main board");
                else
                    sb.Append(
                        $"speculative board of depth {context.depth}, option {context.optionIndex} of {context.optionsCount}");
                log.Info(sb);
            }
            
            SureCellsPass();
            if (verboseLevel == VerboseLevel.StepByStep)
                Print();
            CandidateExclusionSolve(verboseLevel);

            if (verboseLevel == VerboseLevel.StepByStep)
                Console.Write(Print());
            
            if (IsValid && !IsSolved)
            {
                var undeterminedLines = ActiveLines
                    .Where(line => !line.IsSet);

                PicrossActiveLine speculationTarget = undeterminedLines
                    .First(line => line.CandidateCount ==
                                undeterminedLines.Max(l => l.CandidateCount));  //todo: review this criteria. Max reduces memory footprint

                Random rng = new Random();
                var candidateSolutions = speculationTarget.CandidateSolutions
                    .OrderBy(cs => rng.Next());
                int candidatesCount = candidateSolutions.Count();

                for (int i = 0; i < candidatesCount; i++)
                {
                    PicrossBoard speculativeBoard = new PicrossBoard(this);
                    speculativeBoard.SetLineSolution(
                        lineType: speculationTarget.Type,
                        lineIndex: speculationTarget.Index,
                        candidateToSet: candidateSolutions.ElementAt(i)
                    );
                    
                    SpeculativeCallContext speculativeContext = new SpeculativeCallContext()
                    {
                        depth = context?.depth + 1 ?? 1,
                        optionIndex = i,
                        optionsCount = candidatesCount
                    };
                        
                    speculativeBoard.Solve(verboseLevel, speculativeContext);
                    if (speculativeBoard.IsValid && speculativeBoard.IsSolved)
                    {
                        this.Copy(speculativeBoard);
                        return;
                    }
                }
            }
        }

        //todo: find better name
        private void SureCellsPass()
        {
            foreach(var line in ActiveLines)
            {
                line.ApplyLine(line.GetSureCells());
            }
        }

        public void CandidateExclusionSolve(VerboseLevel verboseLevel)
        {
            if(verboseLevel == VerboseLevel.StepByStep)
                Console.Write(Print());

            var solvableLines = ActiveLines.Where(line => !line.IsSet && line.CandidateCount == 1);
            while (solvableLines.Any() && IsValid)
            {
                foreach (PicrossActiveLine solvableLine in solvableLines)
                    solvableLine.ApplyLine(solvableLine.CandidateSolutions.First());

                if (verboseLevel == VerboseLevel.StepByStep)
                    Console.Write(Print());

                solvableLines = ActiveLines.Where(line => !line.IsSet && line.CandidateCount == 1);
            }
        }
        
        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"IsValid {IsValid}");
            sb.AppendLine($"IsSet {IsSet}");
            sb.AppendLine($"IsSolved {IsSolved}");
            sb.AppendLine();

            foreach (var row in Rows)
            {
                sb.Append(row.Print());
            }
            return sb.ToString();
        }
    }
}
