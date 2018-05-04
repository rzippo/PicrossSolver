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
        public bool IsSet => Cells.All(cell => cell.State != PicrossCellState.Undetermined);
        public bool IsSolved => Rule.CheckSolution(this);
        
        public PicrossActiveLine(IEnumerable<PicrossCell> cells, PicrossLineRule rule, LineType type, int index)
           : base(cells)
        {
            Type = type;
            Index = index;

            Rule = rule;
            CandidateSolutions = Rule.GenerateCandidates();
            ReviewCandidates();
            
            foreach(PicrossCell cell in Cells)
            {
                cell.PropertyChanged += CellPropertyChangedHandler;
            }
        }

        private void CellPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            ReviewCandidates();
        }

        private void ReviewCandidates()
        {
            var survivingCandidates = CandidateSolutions.Where(candidate => candidate.IsCandidate(Cells));
            CandidateSolutions = survivingCandidates;
        }

        public PicrossLine GetDeterminableCells()
        {
            if (!IsValid)
                return new PicrossLine(Length, PicrossCellState.Undetermined);

            PicrossLine sureCells = new PicrossLine(CandidateSolutions.First());
            foreach (var candidateSolution in CandidateSolutions.Skip(1))
            {
                sureCells.And(candidateSolution);
            }
            return sureCells;
        }

        public void ApplyLine(PicrossLine line)
        {
            if(line.Length != Length)
                throw new ArgumentException();

            for (int cellIndex = 0; cellIndex < Length; cellIndex++)
            {
                var newState = line.Cells.ElementAt(cellIndex).State;
                if (newState != PicrossCellState.Undetermined)
                    Cells.ElementAt(cellIndex).State = newState;
            }
        }
    }

    public enum LineType
    {
        Column,
        Row
    }
}