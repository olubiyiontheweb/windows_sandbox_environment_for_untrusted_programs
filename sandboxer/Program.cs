using System;
using System.Linq;
using System.Windows.Forms;
using sandboxer.Definitions;
using sandboxer.interactive;

namespace sandboxer
{
    partial class Program : MarshalByRefObject
    {        
        static void Main(string[] args)
        {
            SandboxerGlobals.RedirectMessageDisplay("\nNow running sandboxer for the first time ...\n");

            if(args.Length > 0)
            { 
                // if the user provided some arguments, let's try to load the program
                if(args.Contains("-i"))
                {
                    // if the user provided the -i argument, let's run the interactive sandboxer
                    SandboxerGlobals.RunningMode = RunningModes.CONSOLE;
                    AskUserInteractively("all");
                    SandboxerGlobals.RedirectMessageDisplay("\nRunning interactive sandboxer ...\n");
                }

                // show version number
                if(args.Contains("-v"))
                {
                    SandboxerGlobals.RunningMode = RunningModes.CONSOLE;
                    SandboxerGlobals.RedirectMessageDisplay("\nSandboxer version: " + "1.0.0.0" + "\n");
                }
            }

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
                        // let's initialize instance of the interactive sandboxer before we continue
                        try
                        {
                            // let's initialize instance of the interactive sandboxer before we continue
                            InitializeSanboxerUI();                         
                            string message = "\nRunning sandboxer in " + SandboxerGlobals.RunningMode + " mode";                            
                            SandboxerGlobals.RedirectMessageDisplay(message);                            
                        }
                        catch (Exception ex)
                        {
                            // do nothing
                            Console.WriteLine("Couldn't launch the Sandboxer UI");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Press any key to exit");
                            Console.ReadLine();
                        }
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
            // load the sandboxer UI interface and run the program in the sandbox 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SandboxerGlobals.SandboxerUIInstance = new SandboxerUI();
            Application.Run(SandboxerGlobals.SandboxerUIInstance);   
        }
    }
}
