using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;

namespace Elo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Parse command line args
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(options => Run(options))
                .WithNotParsed<CommandLineOptions>((errors) => HandleParsingErrors(errors));
        }

        internal static void Run(CommandLineOptions options)
        {
            Console.WriteLine("You Entered: ");
            Console.WriteLine($@"File: {options.InputFile}");
            Console.WriteLine($"Teams: {options.TeamMode}");
            Console.WriteLine($"K Factor: {options.KFactor}");
            Console.WriteLine($"Starting Score: {options.StartingScore}");

            IDataProvider dataProvider = new FileDataProvider(options.InputFile);
            EloCalculator calculator = new EloCalculator(dataProvider, options.KFactor, options.StartingScore);

            var rankings = calculator.GetPlayerRankings();

            int rank = 1;
            Console.WriteLine("Rankings: ");
            foreach (var kvp in rankings.OrderByDescending(kvp => kvp.Value))
            {
                Console.WriteLine($"Rank #{rank++,3}: {kvp.Key,-10} {kvp.Value,10:0.00}");
            }

            Console.WriteLine("Press <ENTER> to quit!");
            Console.ReadKey();
        }

        internal static void HandleParsingErrors(IEnumerable<Error> errors)
        {
#if DEBUG
            Console.Error.WriteLine($"Error parsing arguments: ");
            foreach (var error in errors)
            {
                Console.Error.WriteLine(error.ToString());
            }
#endif
        }
    }
}
