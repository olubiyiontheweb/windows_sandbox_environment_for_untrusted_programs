using System;
using System.Security;
using System.Collections.Generic;
using System.Security.Permissions;

using sandboxer.Definitions;
using sandboxer.interactive;

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
        
    static class SandboxerGlobalSetting
    {
        #region Sandboxer global settings

        // Initializing default settings for the Sanboxer
        // private variables 
        private static bool debugmode = false;
        private static RunningModes running_mode = RunningModes.INTERACTIVE;
        private static LogModes log_mode = LogModes.CONSOLE;
        private static States state = States.INIT;
        private static string program_to_run = string.Empty;
        private static string[] arguments_for_program = null;
        private static string working_directory = Environment.CurrentDirectory;

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
            if(RunningMode == RunningModes.INTERACTIVE)
            {
                try
                {
                    // append to the console box in windows forms
                    SandboxerGlobalSetting.SandboxerUIInstance.errorMessage = custom_message;
                }
                catch (Exception e)
                {                        
                    // do nothing;
                }
            }
            else
            {
                SandboxerGlobalSetting.RedirectMessageDisplay(custom_message);
            }
        }
    }
}
