using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        public bool IsValid => ActiveLines.All(activeLine => activeLine.IsValid);
        public bool IsSolved => ActiveLines.All(activeLine => activeLine.IsSolved);

        public void Solve()
        {
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

                foreach (var candidateSolution in candidateSolutions)
                {
                    PicrossBoard speculativeBoard = new PicrossBoard(this);
                    speculativeBoard.SetLineSolution(
                        lineType: speculationTarget.Type,
                        lineIndex: speculationTarget.Index,
                        candidateToSet: candidateSolution
                    );

                    speculativeBoard.Solve();
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

                solvableLines = ActiveLines.Where(line => line.CandidateCount == 1);
            }
        }
    }
}
