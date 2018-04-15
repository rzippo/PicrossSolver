﻿using System;
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
        public PicrossSolverLibrary.VerboseLevel Verbose { get; set; }

        //anything else?
    }
}