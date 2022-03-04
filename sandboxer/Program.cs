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

            SandboxerGlobals.RedirectMessageDisplay("\nNow running sandboxer for the first time ...\n");

            CommandLine.Parser.Default.ParseArguments<OptionsManager>(args)
                .WithParsed<OptionsManager>(opts =>
                {                    
                    if (opts.PassArguments)
                    {
                        SandboxerGlobals.RunningMode = RunningModes.CONSOLE;
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
                        return;
                    }
                });

            // We know everything is ok here (the program is running fine), 
            // so let's check if any program has been loaded
            SandboxerGlobals.State = States.RUNNING;

            while(SandboxerGlobals.State == States.RUNNING)
            {
                // check if we're going ahead with powershell or dotnet route
                try
                {
                    if (SandboxerGlobals.RunningMode == RunningModes.CONSOLE)
                    {
                        // ConsoleExtension.Show();
                        string message = "\nRunning sandboxer in " + SandboxerGlobals.RunningMode + " mode";
                        SandboxerGlobals.RedirectMessageDisplay(message);

                        if(SandboxerGlobals.ProgramToRun != string.Empty)
                        {
                            SandboxerGlobals.LoadSandboxEnvironment();
                        }
                    }
                    else if (SandboxerGlobals.RunningMode == RunningModes.POWERSHELLVM)
                    {
                        string message = "\nRunning sandboxer in " + SandboxerGlobals.RunningMode + " mode";
                        SandboxerGlobals.RedirectMessageDisplay(message);
                        SandboxerGlobals.RedirectMessageDisplay("Starting Windows Sandbox ...\n");
                        SandboxerGlobals.StartWindowsSandbox();
                    }
                    else  if (SandboxerGlobals.RunningMode == RunningModes.INTERACTIVE)
                    {

                        string message = "\nRunning sandboxer in " + SandboxerGlobals.RunningMode + " mode";
                        SandboxerGlobals.RedirectMessageDisplay(message);
                        
                        // hide the console window
                        ConsoleExtension.Hide();

                        // show the UI
                        SandboxerGlobals.SandboxerUIInstance.ShowUI();
                    }
                }
                catch (Exception e)
                {
                    SandboxerGlobals.State = States.ERROR;
                    RuntimeException.Debug("Error: " + e.Message);
                }

                // null all values
                SandboxerGlobals.ProgramToRun = string.Empty;
                SandboxerGlobals.ArgumentsForProgram = null;
                
                if(SandboxerGlobals.RunningMode != RunningModes.INTERACTIVE)
                {
                    AskUserInteractively("all");
                }

                if(SandboxerGlobals.ProgramToRun == string.Empty)
                {
                    SandboxerGlobals.State = States.EXIT;
                }
            }
        }

        static void InitializeSanboxerUI()
        {
            // load the sandboxer UI interface from the interactiveSandboxer.dll assembly
            // and run the program in the sandbox 
            string dll_path = SandboxerGlobals.WorkingDirectory + @"\interactiveSandboxer.dll";

            try
            {
                // load the dll
                Assembly assembly = Assembly.LoadFrom(dll_path);
                // get the ISandboxerUI type
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.GetInterface("ISandboxerUI") != null)
                    {
                        // create an instance of the class
                        SandboxerGlobals.SandboxerUIInstance = (ISandboxerUI)Activator.CreateInstance(type);
                    }
                }
            }
            catch (Exception e)
            {
                RuntimeException.Debug("Error: " + e.Message);
            }
        }
    }
}
