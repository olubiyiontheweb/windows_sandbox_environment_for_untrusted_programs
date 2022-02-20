using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Input;
using System.Security;
using System.IO;
using System.Net.NetworkInformation;
using System.Management.Automation;

using NDesk.Options; // https://github.com/Latency/NDesk.Options/
using sandboxer.AppLoader;
using sandboxer.Definitions;

// TODO: load the interactive sanboxer and run with a different appdomain

// how to run the powershell version
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-overview#:~:text=To%20enable%20Sandbox%20using%20PowerShell,it%20for%20the%20first%20time.
// https://answers.microsoft.com/en-us/windows/forum/all/windows-sandbox-failed-to-start/7dc9379a-dcfb-47f7-8c5b-3ae0002b5991
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-configure-using-wsb-file
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-architecture

// how to run the dotnet version
// https://docs.microsoft.com/en-us/previous-versions/dotnet/framework/code-access-security/how-to-run-partially-trusted-code-in-a-sandbox?redirectedfrom=MSDN
// https://github.com/zippy1981/AppDomainArguments
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset?view=netframework-4.7.2
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset.fromxml?view=netframework-4.7.2#system-security-permissionset-fromxml(system-security-securityelement)
// https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.networkinformationpermission?view=netframework-4.7.2

namespace sandboxer
{
    class Program
    {
        static int verbose = 0;

        // default running mode is interactive opening dialog box
        static RunningModes running_mode = RunningModes.CONSOLE;
        static LogModes log_mode = LogModes.CONSOLE;
        static LogLevels log_level = LogLevels.INFO;
        static SecurityLevels security_level = SecurityLevels.DEFAULT;
        private static States state;

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
                {"r|running-mode=", "Specify running mode, default is CONSOLE", delegate (string value) { running_mode = (RunningModes)Enum.Parse(typeof(RunningModes), value); } },
                {"l|log-mode=", "Specify log mode, default is CONSOLE", delegate (string value) { log_mode = (LogModes)Enum.Parse(typeof(LogModes), value); } },
                {"ll|log-level=", "Specify log level, default is INFO", delegate (string value) { log_level = (LogLevels)Enum.Parse(typeof(LogLevels), value); } },
                {"s|security-level=", "Specify security level, default is DEFAULT", delegate (string value) { security_level = (SecurityLevels)Enum.Parse(typeof(SecurityLevels), value); } },
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

            state = States.INIT;

            if (display_guide)
            {
                DisplayGuide(options);
                state = States.EXIT;
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            string unrecognized_arguments = "";
            if (arguments.Count > 0)
            {
                unrecognized_arguments = string.Join(" ", arguments.ToArray());
                Debug("Displaying unrecognized arguments: " + unrecognized_arguments);
            }

            // We know everything is ok here (the program is running fine), so let's check if any program has been loaded
            state = States.RUNNING;

            while(state == States.RUNNING)
            {
                // check if we're going ahead with powershell or dotnet route else open windows sandbox if we're in powershel mode
                if (programs_to_run.Count > 0 && !(running_mode == RunningModes.POWERSHELLVM))
                {
                    LoadSandboxEnvironment(programs_to_run);
                }
                else if (running_mode == RunningModes.POWERSHELLVM)
                {
                    StartWindowsSandbox();
                }
                
                // if we're running in console mode, we'll wait for user input
                if (running_mode == RunningModes.CONSOLE)
                {
                    Console.WriteLine("Press the escape key to close the sandboxer or type in the path to the program you want to run in the Sandbox: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        state = States.EXIT;
                    }
                    else
                    {
                        programs_to_run.Clear();
                        programs_to_run.Add(input);
                    }                  
                }
            }
        }

        private static void StartWindowsSandbox()
        {
            Console.WriteLine(Environment.OSVersion);
        }

        static void DisplayGuide (OptionSet options)
        {
            Console.WriteLine("Usage: Program [OPTIONS]+");
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        static void LoadSandboxEnvironment(List<string> programs_to_run)
        {
            SandboxEnvironment sandbox_environment = new SandboxEnvironment(running_mode, log_mode, log_level, security_level);
            foreach (string program in programs_to_run)
            {
                Debug("Running program: " + program);
                sandbox_environment.LoadApplication(program);
            }
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
