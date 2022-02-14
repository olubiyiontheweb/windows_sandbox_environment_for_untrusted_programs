using System;
using System.Collections.Generic;

using NDesk.Options; // https://github.com/Latency/NDesk.Options/

namespace securedZone
{
    class Program
    {
        static int verbose = 0;
        static RunningModes running_mode = RunningModes.CONSOLE;
        static LogModes log_mode = LogModes.CONSOLE;
        static LogLevels log_level = LogLevels.INFO;
        static SecurityLevels security_level = SecurityLevels.DEFAULT;
        static States state = States.START;

        static void Main(string[] args)
        {
            bool display_guide = false;            
            List<string> program_names = new List<string>();

            OptionSet options = new OptionSet() {
                {"v|verbose", "Display debug information while running program", delegate (string value) { if (value != null) ++verbose; } },
                {"h|?|help", "Display how to use information", delegate (string value) { display_guide = value != null; } },
                {"p|program=", "Specify program(s) to run in sandbox", delegate (string value) { program_names.Add(value); } },
            };

            List<string> arguments;
            try
            {
                arguments = options.Parse(args);
                Console.WriteLine("display_guide: " + display_guide + ", verbose: " + verbose + ", args: " + args[0]);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            string unrecognized_arguments = "";
            if (arguments.Count > 0) {
                unrecognized_arguments = string.Join (" ", arguments.ToArray ());
                Debug ("Displaying unrecognized arguments: " + unrecognized_arguments);
            }
            else {
                Debug ("No unrecognized arguments found", unrecognized_arguments);
            }

            if (display_guide)
            {
                DisplayGuide(options);                
                return;
            }
        }

        static void DisplayGuide (OptionSet options)
        {
            Console.WriteLine("Usage: Program [OPTIONS]+");
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        static void Debug (string format, params object[] args)
        {            
            if ( verbose > 0) {
                Console.Write ("# ");
                Console.WriteLine (format, args);
            }
        }
    }
}
