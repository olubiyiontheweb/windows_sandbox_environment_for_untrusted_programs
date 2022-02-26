using System;
using System.Collections.Generic;
using System.Windows.Input;

// third party library for parsing command line arguments
using NDesk.Options; // https://github.com/Latency/NDesk.Options/

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
// https://docs.microsoft.com/en-us/previous-versions/dotnet/framework/code-access-security/how-to-run-partially-trusted-code-in-a-sandbox?redirectedfrom=MSDN
// https://github.com/zippy1981/AppDomainArguments
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset?view=netframework-4.7.2
// https://docs.microsoft.com/en-us/dotnet/api/system.security.permissionset.fromxml?view=netframework-4.7.2#system-security-permissionset-fromxml(system-security-securityelement)
// https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.networkinformationpermission?view=netframework-4.7.2

namespace sandboxer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nNow running sandboxer for the first time ...\n");
            bool display_guide = false;
            List<string> programs_to_run = new List<string>();

            OptionSet options = new OptionSet() {
                {"v|verbose", "Display debug information while running program", delegate (string value) { if (value != null) ++SandboxerGlobalSetting.Verbose; } },
                {
                    "p|program=",
                    "Specify program(s) to run in sandbox, please provide all required arguments for the program in the same quote",
                    delegate (string value) { programs_to_run.Add(value); }
                },
                {"r|running-mode=", "Specify running mode, default is CONSOLE", delegate (string value) { SandboxerGlobalSetting.RunningMode = (RunningModes)Enum.Parse(typeof(RunningModes), value, true); } },
                {"l|log-mode=", "Specify log mode, default is CONSOLE", delegate (string value) { SandboxerGlobalSetting.LogMode = (LogModes)Enum.Parse(typeof(LogModes), value, true); } },
                {"s|security-level=", "Specify security level, default is DEFAULT", delegate (string value) { SandboxerGlobalSetting.SecurityLevel = (SecurityLevels)Enum.Parse(typeof(SecurityLevels), value, true); } },
            };

            // Parse the command line arguments provided by the user
            List<string> arguments;
            try
            {
                arguments = options.Parse(args);
            }
            catch (Exception e)
            {
                RuntimeException.Debug("Error: " + e.Message);
                SandboxerGlobalSetting.State = States.ERROR;
                return;
            }

            SandboxerGlobalSetting.State = States.INIT;

            if (display_guide)
            {
                DisplayGuide(options);
                SandboxerGlobalSetting.State = States.EXIT;
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            string unrecognized_arguments = "";
            if (arguments.Count > 0)
            {
                unrecognized_arguments = string.Join(" ", arguments.ToArray());
                throw new RuntimeException("Displaying unrecognized arguments: " + unrecognized_arguments);
            }

            // We know everything is ok here (the program is running fine), 
            // so let's check if any program has been loaded
            SandboxerGlobalSetting.State = States.RUNNING;

            while(SandboxerGlobalSetting.State == States.RUNNING)
            {
                // check if we're going ahead with powershell or dotnet route else open windows sandbox if we're in powershel mode
                try
                {
                    if (SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE && programs_to_run.Count > 0)
                    {
                        Console.WriteLine("\nRunning sandboxer in {0} mode", SandboxerGlobalSetting.RunningMode);
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.CONSOLE && programs_to_run.Count > 0)
                    {                        
                        Console.WriteLine("\nRunning sandboxer in {0} mode", SandboxerGlobalSetting.RunningMode);
                        LoadSandboxEnvironment(programs_to_run);
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.POWERSHELLVM && programs_to_run.Count > 0)
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

                programs_to_run.Clear();
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
                        // save the raw input to the list
                        programs_to_run.Add(input);
                    }
                }
            }
        }

        /// <summary>
        /// Display how to use guide for the sandboxer
        /// </summary>
        static void DisplayGuide (OptionSet options)
        {
            Console.WriteLine("Usage: Program [OPTIONS]+");
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
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
        static void LoadSandboxEnvironment(List<string> programs_to_run)
        {
            SandboxEnvironment sandbox_environment = null;
            foreach (string program in programs_to_run)
            {
                sandbox_environment = new SandboxEnvironment();
                sandbox_environment.InitalizeEnvironment(program);
            }
        }
    }
}
