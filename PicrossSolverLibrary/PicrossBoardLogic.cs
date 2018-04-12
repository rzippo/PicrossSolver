using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsValid => ActiveLines.All(activeLine => activeLine.IsValid);
        public bool IsSolved => ActiveLines.All(activeLine => activeLine.IsSolved);

        public void Solve(bool speculative = false, bool verbose = false)
        {
            if (!speculative)
            {
                log.Info("Start solving" + (verbose ? " with verbose option enabled" : ""));

                SureCellsPass();
                if (verbose)
                    Console.Write(Print());
            }
            
            if(verbose)
                DebugSolve();
            else
                BasicSolve();

            if (IsValid && !IsSolved)
            {
                var undeterminedLines = ActiveLines
                    .Where(line => line.CandidateCount > 1);

                PicrossActiveLine speculationTarget = undeterminedLines
                    .First(line => line.CandidateCount ==
                                undeterminedLines.Max(l => l.CandidateCount));  //todo: review this criteria. Max reduces memory footprint

                Random rng = new Random();
                var candidateSolutions = speculationTarget.CandidateSolutions
                    .OrderBy(cs => rng.Next());

                foreach (var candidateSolution in candidateSolutions)
                {
                    PicrossBoard speculativeBoard = new PicrossBoard(this);
                    speculativeBoard.SetLineSolution(
                        lineType: speculationTarget.Type,
                        lineIndex: speculationTarget.Index,
                        candidateToSet: candidateSolution
                    );

                    speculativeBoard.Solve(speculative: true);
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

        public void BasicSolve()
        {
            var solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateCount == 1);
            while (solvableLines.Any() && IsValid)
            {
                foreach (PicrossActiveLine solvableLine in solvableLines)
                    solvableLine.ApplyLine(solvableLine.CandidateSolutions.First());

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
                    solvableLine.ApplyLine(solvableLine.CandidateSolutions.First());

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
