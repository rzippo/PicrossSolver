using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PicrossSolverLibrary
{
    public class PicrossActiveLine : PicrossLine, INotifyPropertyChanged
    {
        private bool skipReview = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LineType Type { get; }
        public int Index { get; }

        public PicrossLineRule Rule { get; }
        public List<PicrossLine> CandidateSolutions { get; private set; }
        public int CandidateCount => CandidateSolutions.Count();

        public bool IsValid => CandidateSolutions.Any();
        public bool IsSet => Cells.All(cell => cell.State != PicrossCellState.Undetermined);
        public bool IsSolved => Rule.CheckSolution(this);

        public PicrossActiveLine(IEnumerable<PicrossCell> cells, PicrossLineRule rule, LineType type, int index)
           : base(cells)
        {
            Type = type;
            Index = index;

            Rule = rule;
            CandidateSolutions = Rule.GenerateCandidates().ToList();
            ReviewCandidates();

            RegisterCellHandlers();
        }

        public PicrossActiveLine(IEnumerable<PicrossCell> cells, PicrossActiveLine copySource)
            : base(cells)
        {
            Type = copySource.Type;
            Index = copySource.Index;
            Rule = copySource.Rule;
            CandidateSolutions = new List<PicrossLine>(copySource.CandidateSolutions);

            RegisterCellHandlers();
        }

        private void RegisterCellHandlers()
        {
            foreach (PicrossCell cell in Cells)
            {
                cell.PropertyChanged += CellPropertyChangedHandler;
            }
        }

        private void CellPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!skipReview)
            {
                ReviewCandidates();
            }
        }

        private void ReviewCandidates()
        {
            CandidateSolutions = 
                CandidateSolutions
                .AsParallel()
                .Where(candidate => candidate.IsCandidateSolutionFor(this))
                .ToList();             
        }

        public PicrossLine GetDeterminableCells()
        {
            if (!IsValid)
                return new PicrossLine(Length, PicrossCellState.Undetermined);

            PicrossLine determinableCells = new PicrossLine(CandidateSolutions.First());
            foreach (var candidateSolution in CandidateSolutions.Skip(1))
            {
                determinableCells.And(candidateSolution);
            }
            return determinableCells;
        }

        public void ApplyLine(PicrossLine line)
        {
            if(line.Length != Length)
                throw new ArgumentException();

            skipReview = true;
            Parallel.ForEach(
                Enumerable.Range(0, Length),
                cellIndex =>
                {
                    var newState = line.Cells[cellIndex].State;
                    if (newState != PicrossCellState.Undetermined)
                        Cells[cellIndex].State = newState;
                });
            skipReview = false;
            ReviewCandidates();
        }
    }

    public enum LineType
    {
        Column,
        Row
    }
}