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
        [Fact]
        public void LoadFromJson_Solve()
        {
            using (FileStream fs = new FileStream("p050.json", FileMode.Open))
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
