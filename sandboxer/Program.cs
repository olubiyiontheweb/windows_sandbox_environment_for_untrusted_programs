using System;
using System.Linq;
using System.Reflection;

// third party library for parsing command line arguments
using CommandLine; // https://github.com/commandlineparser/commandline

using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.interactive;
using sandboxer.winsand;

namespace sandboxer
{
    partial class Program
    {        
        static void Main(string[] args)
        {
            // let's initialize instance of the interactive sandboxer by reflection before we continue
            InitializeSanboxerUI();
            
            SandboxerGlobalSetting.RedirectMessageDisplay("\nNow running sandboxer for the first time ...\n");

            CommandLine.Parser.Default.ParseArguments<OptionsManager>(args)
                .WithParsed<OptionsManager>(opts =>
                {
                    if (opts.DebugMode)
                    {
                        SandboxerGlobalSetting.RedirectMessageDisplay("Debug mode is on\n");
                        SandboxerGlobalSetting.DebugMode = true;
                    }
                    else if (opts.PassArguments)
                    {
                        SandboxerGlobalSetting.RunningMode = RunningModes.CONSOLE;
                        AskUserInteractively("all");
                    }
                })
                .WithNotParsed<OptionsManager>((errs) =>
                {
                    if (args.Contains("--help") || args.Contains("-h") || args.Contains("-v") || args.Contains("--version"))
                    {
                        return;
                    }
                    else
                    {
                        RuntimeException.Debug("Unrecognized arguments provided: " + string.Join(", ", errs));
                    }
                });

            // We know everything is ok here (the program is running fine), 
            // so let's check if any program has been loaded
            SandboxerGlobalSetting.State = States.RUNNING;

            while(SandboxerGlobalSetting.State == States.RUNNING)
            {
                // check if we're going ahead with powershell or dotnet route
                try
                {
                    if (SandboxerGlobalSetting.RunningMode == RunningModes.INTERACTIVE)
                    {
                        // hide the console window
                        ConsoleExtension.Hide();

                        // show the UI
                        SandboxerGlobalSetting.SandboxerUIInstance.ShowUI();
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.CONSOLE)
                    {
                        // ConsoleExtension.Show();
                        string message = "\nRunning sandboxer in " + SandboxerGlobalSetting.RunningMode + "mode";
                        SandboxerGlobalSetting.RedirectMessageDisplay(message);

                        if(SandboxerGlobalSetting.ProgramToRun != string.Empty)
                        {
                            LoadSandboxEnvironment();
                        }
                    }
                    else if (SandboxerGlobalSetting.RunningMode == RunningModes.POWERSHELLVM)
                    {
                        string message = "\nRunning sandboxer in " + SandboxerGlobalSetting.RunningMode + "mode";
                        SandboxerGlobalSetting.RedirectMessageDisplay(message);
                        SandboxerGlobalSetting.RedirectMessageDisplay("Starting Windows Sandbox ...\n");
                        StartWindowsSandbox();
                    }
                }
                catch (Exception e)
                {
                    SandboxerGlobalSetting.State = States.ERROR;
                    RuntimeException.Debug("Error: " + e.Message);
                }

                // null all values
                SandboxerGlobalSetting.ProgramToRun = string.Empty;
                SandboxerGlobalSetting.ArgumentsForProgram = null;
                
                if(SandboxerGlobalSetting.RunningMode != RunningModes.INTERACTIVE)
                {
                    AskUserInteractively("all");
                }

                if(SandboxerGlobalSetting.ProgramToRun == string.Empty)
                {
                    SandboxerGlobalSetting.State = States.EXIT;
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
                SandboxerGlobalSetting.RedirectMessageDisplay("Windows sandbox is NOT compatible with your OS version \n");
            }
        }        

        /// <summary>
        /// Loads the sandbox environment for the dotnet route
        /// </summary>
        static void LoadSandboxEnvironment()
        {
            SandboxEnvironment sandbox_environment = new SandboxEnvironment();
            sandbox_environment.InitalizeEnvironment();
        }

        static void InitializeSanboxerUI()
        {
            // load the sandboxer UI interface from the interactiveSandboxer.dll assembly
            // and run the program in the sandbox
            string dll_path = SandboxerGlobalSetting.WorkingDirectory + @"\interactiveSandboxer.dll";

            // load the dll
            Assembly assembly = Assembly.LoadFrom(dll_path);

            // get the ISandboxerUI type
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.GetInterface("ISandboxerUI") != null)
                {
                    // create an instance of the class
                    SandboxerGlobalSetting.SandboxerUIInstance = (ISandboxerUI)Activator.CreateInstance(type);
                }
            }
        }
    }
}
