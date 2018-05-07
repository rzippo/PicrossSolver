using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using PicrossSolverLibrary;

namespace PicrossSolverCmd
{
    class Options
    {
        [Option('f', "file", Required = true, HelpText = "Json file to read the puzzle from")]
        public string JsonPath { get; set; }

        [Option('v', "verbose", Default = VerboseLevel.Silent, HelpText = "Prints the board at each fast-forward step")]
        public VerboseLevel Verbose { get; set; }

        [Option("noWait", Default = false, HelpText = "Exits application after solving without waiting for user input")]
        public bool NoWait { get; set; }

        [Option("log", Default = false, HelpText = "Logs application events with log4Net")]
        public bool DoLog { get; set; }
    }
}
