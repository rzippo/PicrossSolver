using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicrossSolverLibrary
{
    public partial class PicrossBoard
    {
        public async Task SolveAsync(SpeculativeCallContext context = null)
        {
            if (!IsValid)
                return;
            
            if (context == null)
                SetDetermibableCells();

            await CandidateExclusionSolveAsync();

            if (IsValid && !IsSolved)
            {
                PicrossBoard speculationBaseBoard = new PicrossBoard(this);
                PicrossBoard solvedBoard = await speculationBaseBoard.SpeculativeSolveAsync(context);
                if (solvedBoard != null)
                    this.Copy(solvedBoard);                
            }
        }

        private async Task<PicrossBoard> SpeculativeSolveAsync(SpeculativeCallContext context)
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

                var speculativeTask = new Task(() => speculativeBoard.Solve(VerboseLevel.Silent, speculativeContext));
                speculativeTask.Start();
                await speculativeTask.ConfigureAwait(false);

                if (speculativeBoard.IsValid && speculativeBoard.IsSolved)
                    return speculativeBoard;                
            }

            return null;
        }

        private async Task CandidateExclusionSolveAsync()
        {
            IEnumerable<PicrossActiveLine> solvableLines;
            while (
                (solvableLines = ActiveLines.Where(line => !line.IsSet && line.CandidateCount == 1)).Any()
                && IsValid)
            {
                var selectedLine = solvableLines.First();
                await new Task(() => selectedLine.ApplyLine(selectedLine.CandidateSolutions.First()));
            }
        }
    }
}