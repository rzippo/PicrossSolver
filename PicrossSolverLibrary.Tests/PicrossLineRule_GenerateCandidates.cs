using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PicrossSolverLibrary.Tests
{
    public class PicrossLineRule_GenerateCandidates
    {
        [Fact]
        public void GenerateCandidatesOf_2_2_l5()
        {
            int length = 5;
            var lineRule = new PicrossLineRule(lineStructure: new[] { 2, 2 }, lineLength: length);
            var candidates = lineRule.GenerateCandidates();
            var expectedSolution = new List<PicrossLine>
            {
                new PicrossLine(
                    new List<PicrossCell>
                    {
                        new PicrossCell(){State = PicrossCellState.Filled},
                        new PicrossCell(){State = PicrossCellState.Filled},
                        new PicrossCell(){State = PicrossCellState.Void},
                        new PicrossCell(){State = PicrossCellState.Filled},
                        new PicrossCell(){State = PicrossCellState.Filled}
                    })
            };

            Assert.True(candidates.Count() == expectedSolution.Count);
            foreach (PicrossLine candidateLine in candidates)
            {
                Assert.Contains(
                    expectedSolution,
                    line => line.Print() == candidateLine.Print());
            }
        }
    }
}
