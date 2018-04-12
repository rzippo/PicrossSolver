using System;
using System.Collections.Generic;

namespace PicrossSolverLibrary.UnitTests.LineRuleTests
{
    public class PicrossLineRuleTester : PicrossLineRule
    {
        public PicrossLineRuleTester(IEnumerable<int> lineStructure, int lineLength) : base(lineStructure, lineLength)
        {}

        public new IEnumerable<Tuple<int, int>> GetGapRules()
        {
            return base.GetGapRules();
        }

        public new IEnumerable<IEnumerable<int>> GenerateGapStructures(IEnumerable<Tuple<int, int>> gapRules)
        {
            return base.GenerateGapStructures(gapRules);
        }
    }
}