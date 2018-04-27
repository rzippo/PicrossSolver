using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PicrossSolverLibrary.Tests
{
    public class PicrossBoard_GetEmpty
    {
        [Fact]
        public void ConstructEmpty()
        {
            //Testing that this doesn't throw
            var board = PicrossBoard.GetEmpty();
        }

        [Fact]
        public void SolveEmpty()
        {
            //Testing that this doesn't throw
            var board = PicrossBoard.GetEmpty();
            board.Solve();
        }
    }
}
