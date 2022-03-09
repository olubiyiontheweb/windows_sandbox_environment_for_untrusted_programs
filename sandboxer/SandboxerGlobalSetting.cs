using System;
using System.Collections.Generic;
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
        public bool UserInterface { get; set; }
        public bool Reflection { get; set; }
        public bool NoneDotNet { get; set; }
        public bool Security { get; set; }
        public bool AudioAccess { get; set; }
        public bool Printing { get; set; }
        public bool Web { get; set; }
        public bool SMTP { get; set; }
        public bool Registry { get; set; }
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
        private static string[] arguments_for_program = new string[0];
        private static string working_directory = AppDomain.CurrentDomain.BaseDirectory;
        private static List<string> error_message = new List<string>();

        // set sandbox mode to none
        private static RunningModes sandbox_mode = RunningModes.NONE;
        private static string network_address = "";
        private static PermissionDict permission_selections = new PermissionDict()
        {
            Networking = false,
            FileSystemAcess = false,
            Execution = false,
            UserInterface = false,
            Reflection = false,
            NoneDotNet = false,
            Security = false,
            AudioAccess = false,
            Printing = false,
            Web = false,
            SMTP = false,
            Registry = false,
        };
        private static List<string> custom_permissions = new List<string>();

        private static SandboxerUI sandboxer_ui_instance = null;

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

        public static SandboxerUI SandboxerUIInstance
        {
            get { return sandboxer_ui_instance; }
            set { sandboxer_ui_instance = value; }
        }

        public static string NetworkAddress
        {
            get { return network_address; }
            set { network_address = value; }
        }

        public static List<string> ErrorMessage
        {
            get { return error_message; }
            set { error_message = value; }
        }

        public static RunningModes SandboxMode
        {
            get { return sandbox_mode; }
            set { sandbox_mode = value; }
        }

        #endregion

        // this would help us update the log box for new messages about the state of the sandbox
        public static void RefreshConsoleLog()
        {
            if(SandboxerGlobals.SandboxerUIInstance != null)
            {                
                SandboxerGlobals.SandboxerUIInstance.consolelog.Items.Clear();

                // add error messages to the listbox
                for (int i = 0; i < ErrorMessage.Count; i++)
                {
                    SandboxerGlobals.SandboxerUIInstance.consolelog.Items.Add(ErrorMessage[i]);
                }

                SandboxerGlobals.SandboxerUIInstance.consolelog.Refresh();
            }
        }

        public static void RedirectMessageDisplay(string custom_message)
        {
            // append to the console box in windows forms
            if(running_mode == RunningModes.INTERACTIVE)
            {
                try
                {
                    // append to the console box in windows forms
                    error_message.Add(custom_message);
                    if (sandboxer_ui_instance != null)
                    {
                        RefreshConsoleLog();
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
