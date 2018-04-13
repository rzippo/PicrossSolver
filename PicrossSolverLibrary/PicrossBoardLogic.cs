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
        StepByStep
    }

    public partial class PicrossBoard
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsValid => ActiveLines.All(activeLine => activeLine.IsValid);
        public bool IsSolved => ActiveLines.All(activeLine => activeLine.IsSolved);

        public void Solve(
            VerboseLevel verboseLevel = VerboseLevel.Silent,
            SpeculativeCallContext context = null)
        {
            if(verboseLevel != VerboseLevel.Silent)
                log.Info("Start solving " + 
                    (context == null ? 
                        "main board" 
                        : $"speculative board of depth {context.depth}, option {context.optionIndex} of {context.optionsCount}"));
            
            if(verboseLevel == VerboseLevel.StepByStep)
                DebugSolve();
            else
                BasicSolve();

            if (IsValid && !IsSolved)
            {
                var undeterminedLines = ActiveLines
                    .Where(line => line.CandidateCount > 1);

                PicrossActiveLine speculationTarget = undeterminedLines
                    .First(line => line.CandidateCount ==
                                undeterminedLines.Min(l => l.CandidateCount));

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
                        depth = context == null ? 1 : context.depth + 1,
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

        public void BasicSolve()
        {
            var solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateCount == 1);
            while (solvableLines.Any() && IsValid)
            {
                foreach (PicrossActiveLine solvableLine in solvableLines)
                    solvableLine.ApplySolution(solvableLine.CandidateSolutions.First());

                solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateCount == 1);
            }
        }

        public void DebugSolve()
        {
            Console.Write(Print());

            var solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateCount == 1);
            while (solvableLines.Any() && IsValid)
            {
                foreach (PicrossActiveLine solvableLine in solvableLines)
                    solvableLine.ApplySolution(solvableLine.CandidateSolutions.First());

                Console.Write(Print());
                solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateCount == 1);
            }
        }

        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"IsValid {IsValid}");
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
