using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PicrossSolverLibrary.UnitTests.LineRuleTests
{
    public class PicrossLineRule_GenerateGapStructures
    {
        public static bool AreEqualGapStructures(IEnumerable<IEnumerable<int>> g1, IEnumerable<IEnumerable<int>> g2)
        {
            return g1.Count() == g2.Count() && g2.All(gs2 => g1.Count(gs1 => gs1.SequenceEqual(gs2)) == 1);
        }

        [Fact]
        public void AreEqualsGapStructuresTest()
        {
            var g1 = new List<IEnumerable<int>>()
            {
                new List<int>() { 1, 1, 0 },
                new List<int>() { 0, 2, 0 },
                new List<int>() { 0, 1, 1 }
            };

            var g2 = new List<IEnumerable<int>>()
            {
                new List<int>() { 0, 2, 0 },
                new List<int>() { 1, 1, 0 },
                new List<int>() { 0, 1, 1 }
            };

            var g3 = new List<IEnumerable<int>>()
            {
                new List<int>() { 0, 2, 0 },
                new List<int>() { 1, 3, 0 },
                new List<int>() { 0, 1, 1 }
            };

            var g4 = new List<IEnumerable<int>>()
            {
                new List<int>() { 0, 2, 0 },
                new List<int>() { 1, 1, 0 },
                new List<int>() { 1, 1, 0 },
                new List<int>() { 0, 1, 1 }
            };

            Assert.True(AreEqualGapStructures(g1, g2));
            Assert.True(AreEqualGapStructures(g2, g1));

            Assert.False(AreEqualGapStructures(g1, g3));
            Assert.False(AreEqualGapStructures(g3, g1));

            Assert.False(AreEqualGapStructures(g1, g4));
            Assert.False(AreEqualGapStructures(g4, g1));
        }

        [Fact]
        public void GapStructuresOf_5_3_l10()
        {
            var lineRule = new PicrossLineRuleTester(new[] { 5, 3 }, 10);
            var gapRules = lineRule.GetGapRules();
            var gapStructures = lineRule.GenerateGapStructures(gapRules, lineRule.VoidCellCount);

            var expectedGapStructures = new List<IEnumerable<int>>()
            {
                new List<int>() {1, 1, 0},
                new List<int>() {0, 2, 0},
                new List<int>() {0, 1, 1}
            };

            Assert.True(AreEqualGapStructures(gapStructures, expectedGapStructures));
        }
    }
}