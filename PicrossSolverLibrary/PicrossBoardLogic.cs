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
                    log.Info("Solving aborted, board invalid - " + BoardName(context));
                return;
            }

            if(verboseLevel != VerboseLevel.Silent)
                log.Info("Start solving - " + BoardName(context));

            if (context == null)
                SureCellsPass();
            
            CandidateExclusionSolve(verboseLevel);

            if (IsValid && !IsSolved)
            {
                var undeterminedLines = ActiveLines
                    .Where(line => !line.IsSet);

                PicrossActiveLine speculationTarget = undeterminedLines
                    .First(line => line.CandidateCount ==
                                undeterminedLines.Min(l => l.CandidateCount));  //todo: review this criteria. Max reduces memory footprint

                Random rng = new Random();
                var candidateSolutions = speculationTarget.CandidateSolutions
                    .OrderBy(cs => rng.Next()).ToArray();
                int candidatesCount = candidateSolutions.Count();

                for (int i = 0; i < candidatesCount; i++)
                {
                    PicrossBoard speculativeBoard = new PicrossBoard(this);
                    speculativeBoard.SetLineSolution(
                        lineType: speculationTarget.Type,
                        lineIndex: speculationTarget.Index,
                        candidateToSet: candidateSolutions[i]
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

            if (verboseLevel != VerboseLevel.Silent)
            {
                if(!IsSolved)
                    log.Info("Solving failed - " + BoardName(context));
                else
                    log.Info("Solving succeeded - " + BoardName(context));
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
                Console.Write(Print("At start of CandidateExclusionSolve"));

            IEnumerable<PicrossActiveLine> solvableLines;
            while (
                (solvableLines = ActiveLines.Where(line => !line.IsSet && line.CandidateCount == 1)).Any() 
                && IsValid)
            {
                var selectedLine = solvableLines.First();
                selectedLine.ApplyLine(selectedLine.CandidateSolutions.First());

                if (verboseLevel == VerboseLevel.StepByStep)
                    Console.Write(Print("After a CandidateExclusionStep"));
            }
        }
        
        public string Print(string headLine = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headLine ?? "");
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

        public static string BoardName(SpeculativeCallContext context)
        {
            if (context == null)
                return "main board";
            else
                return
                    $"speculative board of depth {context.depth}, option {context.optionIndex + 1} of {context.optionsCount}";
        }
    }
}
