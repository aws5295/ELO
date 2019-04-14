using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Elo
{
    internal class CommandLineOptions
    {
        [Option('f', "file", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('t', "team", Default = false, HelpText = "Use this option if players are grouped by teams.")]
        public bool TeamMode { get; set; }

        [Option('k', "kfactor", Default = 28, HelpText = "K-Factor")]
        public double KFactor { get; set; }

        [Option('s', "score", Default = 1_000, HelpText = "Starting ELO")]
        public double StartingScore { get; set; }

        [Usage(ApplicationAlias = "Elo.exe")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Individual Competition", new CommandLineOptions
                {
                    InputFile = "foobar.txt",
                    TeamMode = false,
                    KFactor = 25,
                    StartingScore = 1000
                });

                yield return new Example("Team Competition", new CommandLineOptions
                {
                    InputFile = "teambar.txt",
                    TeamMode = true,
                    KFactor = 20,
                    StartingScore = 2000
                });
            }
        }
    }
}
