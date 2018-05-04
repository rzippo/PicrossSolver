using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PicrossSolverLibrary.Tests
{
    public class PicrossActiveLine_GetSureCells
    {
        [Fact]
        public void SureCellsOf_5_3_l10()
        {
            int length = 10;
            var lineRule = new PicrossLineRule(lineStructure: new []{5, 3}, lineLength: length);
            var activeLine = new PicrossActiveLine(
                cells: (new PicrossLine(length: length, state: PicrossCellState.Undetermined)).Cells,
                rule: lineRule,
                type: LineType.Row,
                index: 0);

            var sureCells = activeLine.GetDeterminableCells();
            var expectedSolution = new PicrossLine(
                new List<PicrossCell>
                {
                    new PicrossCell(){State = PicrossCellState.Undetermined},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Undetermined},
                    new PicrossCell(){State = PicrossCellState.Undetermined},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Undetermined},
                }
            );

            Assert.True(expectedSolution.Print() == sureCells.Print());
        }

        [Fact]
        public void SureCellsOf_2_2_5()
        {
            int length = 5;
            var lineRule = new PicrossLineRule(lineStructure: new[] { 2, 2 }, lineLength: length);
            var activeLine = new PicrossActiveLine(
                cells: (new PicrossLine(length: length, state: PicrossCellState.Undetermined)).Cells,
                rule: lineRule,
                type: LineType.Row,
                index: 0);

            var sureCells = activeLine.GetDeterminableCells();
            var expectedSolution = new PicrossLine(
                new List<PicrossCell>
                {
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Void},
                    new PicrossCell(){State = PicrossCellState.Filled},
                    new PicrossCell(){State = PicrossCellState.Filled}
                }
            );

            Assert.True(expectedSolution.Print() == sureCells.Print());
        }
    }
}
