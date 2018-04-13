using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using CommandLine;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using PicrossSolverCmd;
using PicrossSolverLibrary;

namespace PicrossSolver
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        public static PicrossBoard Board { get; set; }
        public static PicrossPuzzle Puzzle { get; private set; }

        static void Main(string[] args)
        {
            ConfigureLog4Net();
           
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void ConfigureLog4Net()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var logRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(logRepository, log4netConfig["log4net"]);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Lazy error line: bad arguments");
            PressEnterToContinue();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            try
            {
                using (FileStream fs = new FileStream(opts.JsonPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string json = sr.ReadToEnd();
                        Puzzle = JsonConvert.DeserializeObject<PicrossPuzzle>(json);
                        Console.WriteLine($"Loaded puzzle from {fs.Name}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error while opening json");
            }

            Board = new PicrossBoard(Puzzle);
            Console.WriteLine("Board loaded, next is solving...");
            Board.Solve(verboseLevel: opts.Verbose);
            Console.WriteLine("Solving complete, result:");
            Console.Write(Board.Print());

            PressEnterToContinue();        
        }

        private static void PressEnterToContinue()
        {
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}
