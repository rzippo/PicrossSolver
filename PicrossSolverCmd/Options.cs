using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace PicrossSolverCmd
{
    class Options
    {
        [Option('f', "file", Required = true, HelpText = "Json file to read the puzzle from")]
        public string JsonPath { get; set; }

        [Option(Default = false, HelpText = "Prints the board at each fast-forward step")]
        public bool Verbose { get; set; }

        //anything else?
    }
}
