using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace PicrossSolverLibrary.Tests
{
    public class PicrossBoard_Solve
    {
        [Theory]
        [InlineData("p050.json")]
        [InlineData("p080.json")]
        public void LoadFromJson_Solve(string puzzlePath)
        {
            using (FileStream fs = new FileStream(puzzlePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string json = sr.ReadToEnd();
                    PicrossPuzzle puzzle = JsonConvert.DeserializeObject<PicrossPuzzle>(json);
                    PicrossBoard board = new PicrossBoard(puzzle);
                    board.Solve();
                    Assert.True(board.IsSolved);
                }
            }
        }
    }
}
