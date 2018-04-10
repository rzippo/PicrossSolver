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

        public PicrossLineRule Rule { get; }
        public IEnumerable<PicrossLine> CandidateSolutions { get; private set; }

        public bool IsValid => Rule.Validate(this);
        public bool IsSolved => Rule.CheckSolution(this);
        
        public PicrossActiveLine(IEnumerable<PicrossCell> cells, PicrossLineRule rule)
           : base(cells)
        {
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
    }
}