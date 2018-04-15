using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PicrossSolverLibrary.UnitTests.LineRuleTests
{
    public class PicrossLineRule_GetGapRules
    {
        [Fact]
        public void GapRulesOf_5_3_l10()
        {
            var lineRule = new PicrossLineRuleTester(new []{5, 3}, 10);
            var gapRules = lineRule.GetGapRules();

            var expectedGapRules = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(1, 2),
                new Tuple<int, int>(0, 1)
            };
            Assert.True(gapRules.SequenceEqual(expectedGapRules));
        }

        [Fact]
        public void GapRulesOf_3_l15()
        {
            PicrossLineRuleTester lineRule = new PicrossLineRuleTester(new[] { 3 }, 15);
            var gapRules = lineRule.GetGapRules();

            var expectedGapRules = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(0, 12),
                new Tuple<int, int>(0, 12),
            };
            Assert.True(gapRules.SequenceEqual(expectedGapRules));

        }

    }
}