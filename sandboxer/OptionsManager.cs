
// third party library for parsing command line arguments
using CommandLine; // https://github.com/commandlineparser/commandline

namespace sandboxer
{
    partial class Program
    {
        public class OptionsManager
        {
            [Option('d', "debugmode", Required = false, HelpText = "Display debug information while running program")]
            public bool DebugMode { get; set; }

            [Option('p', "program", Required = false, HelpText = "Specify program(s) to run in sandbox")]
            public string Program { get; set; }

            [Option('a', "arguments", Required = false, HelpText = "Specify arguments for the program, please provide all arguments for the program in the same quote")]
            public string Arguments { get; set; }

            [Option('m', "mode", Required = false, HelpText = "Specify sandbox mode to run the program in (default: INTERACTIVE)")]
            public string Mode { get; set; }

            [Option('l', "logmode", Required = false, HelpText = "Specify log mode to run the program in (default: CONSOLE)")]
            public string LogMode { get; set; }
        }
    }
}
