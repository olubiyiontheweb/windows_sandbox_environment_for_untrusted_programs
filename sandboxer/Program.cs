using System;
using System.Linq;

// third party library for parsing command line arguments
using CommandLine; // https://github.com/commandlineparser/commandline

using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.winsand;

// TODO: load the interactive sanboxer and run with a different appdomain

// how to run the powershell version
// https://jdhitsolutions.com/blog/powershell/7621/doing-more-with-windows-sandbox/
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-overview#:~:text=To%20enable%20Sandbox%20using%20PowerShell,it%20for%20the%20first%20time.
// https://answers.microsoft.com/en-us/windows/forum/all/windows-sandbox-failed-to-start/7dc9379a-dcfb-47f7-8c5b-3ae0002b5991
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-configure-using-wsb-file
// https://docs.microsoft.com/en-us/windows/security/threat-protection/windows-sandbox/windows-sandbox-architecture

// how to run the dotnet version
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset?view=netframework-4.7.2
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset.fromxml?view=netframework-4.7.2#system-security-permissionset-fromxml(system-security-securityelement)
// https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.networkinformationpermission?view=netframework-4.7.2

namespace sandboxer
{
    class Program
    {

        public class Options
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
        
        static void Main(string[] args)
        {
            Console.WriteLine("\nNow running sandboxer for the first time ...\n");
            string programs_to_run = string.Empty;
            string arguments_for_program = string.Empty;

            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts =>
                {
                    if (opts.DebugMode)
                    {
                        Console.WriteLine("Debug mode is on\n");
                        SandboxerGlobalSetting.DebugMode = true;
                    }
                    else if (opts.Program != null)
                    {
                        programs_to_run = opts.Program;
                    }
                    else if (opts.Arguments != null)
                    {
                        arguments_for_program = opts.Arguments;
                    }
                    else if (opts.Mode != null)
                    {
                        SandboxerGlobalSetting.RunningMode = (RunningModes)Enum.Parse(typeof(RunningModes), opts.Mode);
                    }
                    else if (opts.LogMode != null)
                    {
                        SandboxerGlobalSetting.LogMode = (LogModes)Enum.Parse(typeof(LogModes), opts.LogMode);
                    }       
                })
                .WithNotParsed<Options>((errs) =>
                {
                    if (args.Contains("--help") || args.Contains("-h") || args.Contains("-v") || args.Contains("--version"))
                    {
                        return;
                    }
                    else
                    {
                        throw new RuntimeException("Unrecognized arguments provided: " + string.Join(", ", errs));
                    }
                });

            // We know everything is ok here (the program is running fine), 
            // so let's check if any program has been loaded
            SandboxerGlobalSetting.State = States.RUNNING;

            while(SandboxerGlobalSetting.State == States.RUNNING)
            {
                // check if we're going ahead with powershell or dotnet route else open windows sandbox if we're in powershel mode
                try
                {
                    if (SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE && programs_to_run != string.Empty)
                    {
                        Console.WriteLine("\nRunning sandboxer in {0} mode", SandboxerGlobalSetting.RunningMode);
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.CONSOLE && programs_to_run != string.Empty)
                    {                        
                        Console.WriteLine("\nRunning sandboxer in {0} mode", SandboxerGlobalSetting.RunningMode);
                        LoadSandboxEnvironment(programs_to_run, arguments_for_program);
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.POWERSHELLVM && programs_to_run != string.Empty)
                    {
                        Console.WriteLine("\nRunning sandboxer in {0} mode", SandboxerGlobalSetting.RunningMode);
                        Console.WriteLine("Starting Windows Sandbox ...\n");
                        StartWindowsSandbox();
                    }
                }
                catch (Exception e)
                {
                    SandboxerGlobalSetting.State = States.ERROR;
                    RuntimeException.Debug("Error: " + e.Message);
                }

                string input = "";
                
                if(SandboxerGlobalSetting.RunningMode != RunningModes.INTERACTIVE)
                {
                    Console.WriteLine("Press the enter key to close the sandboxer or type in the path to the program you want to run in the Sandbox: ");
                    input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        SandboxerGlobalSetting.State = States.EXIT;
                    }
                    else
                    {
                        // parse the input to get the program and arguments
                        string[] input_split = input.Split(' ');
                        programs_to_run = input_split[0];

                        if (input_split.Length > 1)
                        {
                            arguments_for_program = input_split[1];
                        } 
                        else
                        {
                            arguments_for_program = string.Empty;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads and executes the windows sandbox program 
        /// </summary>        
        private static void StartWindowsSandbox()
        {
            if (WinSandboxManagager.CheckWindowsValidity())
            {
                // if windows sandbox is not enabled, we'll enable it, then run else we'll just run it directly
                if (!WinSandboxManagager.IsWindowsSandboxEnabled())
                {
                    WinSandboxManagager.InstallWindowsSandbox();
                    WinSandboxManagager.RunWindowsSandbox();
                }
                else
                {                    
                    WinSandboxManagager.RunWindowsSandbox();
                }    
            }
            else
            {
                Console.WriteLine("Windows sandbox is NOT compatible with your OS version \n");
            }
        }        

        /// <summary>
        /// Loads the sandbox environment for the dotnet route
        /// </summary>
        static void LoadSandboxEnvironment(string programs_to_run, string arguments)
        {
            SandboxEnvironment sandbox_environment = new SandboxEnvironment();
            sandbox_environment.InitalizeEnvironment(programs_to_run, arguments);
        }
    }
}
