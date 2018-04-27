using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PicrossSolverLibrary.UnitTests.LineRuleTests
{
    public class PicrossLineRule_Trivial
    {
        [Theory]
        [InlineData(new int[]{10}, 10, true)]
        [InlineData(new int[]{0}, 10, true)]
        [InlineData(new int[]{5}, 10, false)]
        public void DetectTrivial(int[] structure, int length, bool expectedResult)
        {
            var lineRule = new PicrossLineRuleTester(structure, length);
            Assert.True(lineRule.IsTrivial == expectedResult);
        }

        [Theory]
        [InlineData(new int[]{10}, 10)]
        [InlineData(new int[]{0}, 10)]
        public void CheckTrivialSolution(int[] structure, int length)
        {
            var lineRule = new PicrossLineRuleTester(structure, length);
            var trivialSolution = lineRule.GetTrivialSolution();
            Assert.True(lineRule.CheckSolution(trivialSolution));
        }
    }
}