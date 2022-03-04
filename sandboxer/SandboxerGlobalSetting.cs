using System;
using System.IO;
using System.Security;
using System.Collections.Generic;
using System.Security.Permissions;

using sandboxer;
using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.interactive;
using sandboxer.winsand;

namespace sandboxer
{

    /// <summary>
    /// Type for configuring supported permissions
    /// </summary>
    public class PermissionDict
    {
        public bool Networking { get; set; }
        public bool FileSystemAcess { get; set; }
        public bool Execution { get; set; }
    }

    public static class ExecuteFromUI
    {
        /// <summary>
        /// get current variables from the UI instance once the start button has been clicked
        /// </summary>
        public static void PopulateGlobalVariables()
        {       
            if(SandboxerGlobals.SandboxerUIInstance.sandboxMode == RunningModes.NONE )
            {
                RuntimeException.Debug("Please select a sandbox mode");
                return;
            }

            if (!string.IsNullOrWhiteSpace(SandboxerGlobals.SandboxerUIInstance.workingDirectory))
            {
                SandboxerGlobals.WorkingDirectory = SandboxerGlobals.SandboxerUIInstance.workingDirectory;
            }
            else
            {
                SandboxerGlobals.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            // get the current values from the UI
            if (!string.IsNullOrWhiteSpace(SandboxerGlobals.SandboxerUIInstance.programName))
            {
                string program_path = Path.Combine(SandboxerGlobals.WorkingDirectory, SandboxerGlobals.SandboxerUIInstance.programName);
                // check if file exists
                if (File.Exists(program_path))
                {
                    // set the program name
                    SandboxerGlobals.ProgramToRun = SandboxerGlobals.SandboxerUIInstance.programName;
                }
                else
                {
                    // file doesn't exist
                    SandboxerGlobals.RedirectMessageDisplay("The program File doesn't exist in the working directory");
                    return;
                }
            }

            SandboxerGlobals.ArgumentsForProgram = SandboxerGlobals.SandboxerUIInstance.Arguments;

            SandboxerGlobals.PermissionSelections = SandboxerGlobals.SandboxerUIInstance.availablePermissions;

            SandboxerGlobals.CustomPermissions = SandboxerGlobals.SandboxerUIInstance.customPermissions;

            // checking the sandbox mode selected so we can run it accordingly
            if (SandboxerGlobals.SandboxerUIInstance.sandboxMode == RunningModes.POWERSHELLVM)
            {
                SandboxerGlobals.StartWindowsSandbox();
                return;
            }
            else
            {
                SandboxerGlobals.LoadSandboxEnvironment();
                return;
            }
        }
    }

    static class SandboxerGlobals
    {
        #region Sandboxer global settings

        // Initializing default settings for the Sanboxer
        // private variables 
        private static bool debugmode = true;
        private static RunningModes running_mode = RunningModes.INTERACTIVE;
        private static LogModes log_mode = LogModes.CONSOLE;
        private static States state = States.INIT;
        private static string program_to_run = string.Empty;
        private static string[] arguments_for_program = null;
        private static string working_directory = AppDomain.CurrentDomain.BaseDirectory;
        private static PermissionDict permission_selections = new PermissionDict()
        {
            Networking = false,
            FileSystemAcess = false,
            Execution = false,
        };
        private static List<string> custom_permissions = new List<string>();

        private static ISandboxerUI sandboxer_ui_instance = null;

        // public getter and setter methods
        public static bool DebugMode
        {
            get { return debugmode; }
            set { debugmode = value; }
        }
        public static RunningModes RunningMode
        {
            get { return running_mode; }
            set { running_mode = value; }
        }
        public static LogModes LogMode
        {
            get { return log_mode; }
            set { log_mode = value; }
        }

        public static States State
        {
            get { return state; }
            set { state = value; }
        }

        public static string ProgramToRun
        {
            get { return program_to_run; }
            set { program_to_run = value; }
        }

        public static string[] ArgumentsForProgram
        {
            get { return arguments_for_program; }
            set { arguments_for_program = value; }
        }

        public static string WorkingDirectory
        {
            get { return working_directory; }
            set { working_directory = value; }
        }

        public static PermissionDict PermissionSelections
        {
            get { return permission_selections; }
            set { permission_selections = value; }
        }

        public static List<string> CustomPermissions
        {
            get { return custom_permissions; }
            set { custom_permissions = value; }
        }

        public static ISandboxerUI SandboxerUIInstance
        {
            get { return sandboxer_ui_instance; }
            set { sandboxer_ui_instance = value; }
        }

        #endregion

        public static void RedirectMessageDisplay(string custom_message)
        {
            // append to the console box in windows forms
            if(running_mode == RunningModes.INTERACTIVE)
            {
                try
                {
                    // append to the console box in windows forms
                    sandboxer_ui_instance.errorMessage.Add(custom_message);
                    if (sandboxer_ui_instance != null)
                    {
                        sandboxer_ui_instance.RefreshConsoleLog();
                    }
                }
                catch (Exception)
                {                        
                    // do nothing;
                }
            }
            else
            {
                Console.WriteLine(custom_message);
            }
        }

        /// <summary>
        /// Loads and executes the windows sandbox program 
        /// </summary>        
        public static void StartWindowsSandbox()
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
                SandboxerGlobals.RedirectMessageDisplay("Windows sandbox is NOT compatible with your OS version \n");
            }
        }

        /// <summary>
        /// Loads the sandbox environment for the dotnet route
        /// </summary>
        public static void LoadSandboxEnvironment()
        {
            SandboxEnvironment sandbox_environment = new SandboxEnvironment();
            sandbox_environment.InitalizeEnvironment();
        }
    }
}
