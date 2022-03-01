using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sandboxer;
using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.interactive;
using sandboxer.winsand;

namespace interactiveSandboxer
{
    public static class Variables
    {        
        #region private variables

        public static RunningModes sandbox_mode = RunningModes.CONSOLE;
        public static List<string> selected_permissions = new List<string>();
        public static string program_name = string.Empty;
        public static string arguments = string.Empty;
        public static string error_message = string.Empty;
        public static PermissionDict available_permissions = new PermissionDict();
        public static string custom_permissions = "";
        public static string working_directory = Environment.CurrentDirectory;


        #endregion
    }
    public class interactiveSandboxer : ISandboxerUI
    {
        #region public variables

        public RunningModes sandboxMode
        {
            get { return Variables.sandbox_mode; }
            set { Variables.sandbox_mode = value; }
        }

        public List<string> selectedPermissions
        {
            get { return Variables.selected_permissions; }
            set { Variables.selected_permissions = value; }
        }

        public string programName
        {
            get { return Variables.program_name; }
            set { Variables.program_name = value; }
        }

        public string Arguments
        {
            get { return Variables.arguments; }
            set { Variables.arguments = value; }
        }

        public string workingDirectory
        {
            get { return Variables.working_directory; }
            set { Variables.working_directory = value; }
        }

        public string errorMessage
        {
            get { return Variables.error_message; }
            set { Variables.error_message = value; }
        }

        #endregion

        #region private methods

        private void Start()
        {
            if (Variables.sandbox_mode == RunningModes.CONSOLE)
            {                
                //sandboxer.Execute();
            }
            else if (Variables.sandbox_mode == RunningModes.INTERACTIVE)
            {
                // sandboxer.Execute();
            }
        }

        #endregion

        #region public methods

        public void ShowUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SandboxerUI());
        }

        #endregion
    }
}