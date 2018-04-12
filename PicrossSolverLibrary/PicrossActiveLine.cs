using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PicrossSolverLibrary
{
    public class PicrossActiveLine : PicrossLine, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LineType Type { get; }
        public int Index { get; }

        public PicrossLineRule Rule { get; }
        public IEnumerable<PicrossLine> CandidateSolutions { get; private set; }
        public int CandidateCount => CandidateSolutions.Count();

        public bool IsValid => CandidateSolutions.Any();
        public bool IsSolved => Rule.CheckSolution(this);
        
        public PicrossActiveLine(IEnumerable<PicrossCell> cells, PicrossLineRule rule, LineType type, int index)
           : base(cells)
        {
            Type = type;
            Index = index;

            Rule = rule;
            CandidateSolutions = Rule.GenerateCandidates();
            
            foreach(PicrossCell cell in Cells)
            {
                cell.PropertyChanged += CellPropertyChangedHandler;
            }
        }

        private void CellPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            ReviewCandidates();
        }

        //todo: how to propagate speculation failed error?
        //note: this is assuming that cells only change Void->Filled
        private void ReviewCandidates()
        {
            var survivingCandidates = CandidateSolutions.Where(candidate => candidate.IsCandidate(Cells));
            CandidateSolutions = survivingCandidates;
        }

        //todo: find better name
        public PicrossLine GetSureCells()
        {
            PicrossLine sureCells = new PicrossLine(Length, PicrossCellState.Filled);
            foreach (var candidateSolution in CandidateSolutions)
            {
                sureCells.And(candidateSolution);
            }
            return sureCells;
        }

        public void ApplySolution(PicrossLine solution)
        {
            if(solution.Length != Length)
                throw new ArgumentException();

            for (int cellIndex = 0; cellIndex < Length; cellIndex++)
            {
                Cells.ElementAt(cellIndex).State = solution.Cells.ElementAt(cellIndex).State;
            }
        }
    }

    public enum LineType
    {
        Column,
        Row
    }
}