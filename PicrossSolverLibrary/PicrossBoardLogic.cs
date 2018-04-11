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
                //todo: speculative solving
            }
        }

        public void BasicSolve()
        {
            var solvableLines = ActiveLines.Where(line => !line.IsSolved && line.CandidateSolutions.Count() == 1);
            while (solvableLines.Any() && IsValid)
            {
                foreach (PicrossActiveLine solvableLine in solvableLines)
                    solvableLine.ApplySolution(solvableLine.CandidateSolutions.First());

                solvableLines = ActiveLines.Where(line => line.CandidateSolutions.Count() == 1);
            }
        }
    }
}
