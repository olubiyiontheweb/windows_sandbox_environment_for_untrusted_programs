using System;
using System.Collections.Generic;

using NDesk.Options; // https://github.com/Latency/NDesk.Options/

// https://docs.microsoft.com/en-us/previous-versions/dotnet/framework/code-access-security/how-to-run-partially-trusted-code-in-a-sandbox?redirectedfrom=MSDN

namespace sandboxer
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
            List<string> programs_to_run = new List<string>();

            OptionSet options = new OptionSet() {
                {"v|verbose", "Display debug information while running program", delegate (string value) { if (value != null) ++verbose; } },
                {"h|?|help", "Display how to use information", delegate (string value) { display_guide = value != null; } },
                {
                    "p|program=", 
                    "Specify program(s) to run in sandbox, please provide all required arguments for the program in the same quote", 
                    delegate (string value) { programs_to_run.Add(value); }
                },
            };

            List<string> arguments;
            try
            {
                arguments = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (display_guide)
            {
                DisplayGuide(options);                
                return;
            }

            string unrecognized_arguments = "";
            if (arguments.Count > 0) {
                unrecognized_arguments = string.Join (" ", arguments.ToArray ());
                Debug ("Displaying unrecognized arguments: " + unrecognized_arguments);
            }

            if (programs_to_run.Count > 0) {             
                foreach (string program in programs_to_run) {
                    Debug ("Running program: " + program);
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
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
