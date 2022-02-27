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
    class interactiveSandboxer : ISandboxerUI
    {
        #region private variables

        private string sandbox_mode = string.Empty;
        private List<string> selected_permissions = new List<string>();
        private string program_name = string.Empty;
        private string arguments = string.Empty;
        private string working_directory = string.Empty;
        private string error_message = string.Empty;

        #endregion

        #region public variables

        public string sandboxMode
        {
            get { return sandbox_mode; }
            set { sandbox_mode = value; }
        }

        public List<string> selectedPermissions
        {
            get { return selected_permissions; }
            set { selected_permissions = value; }
        }

        public string programName
        {
            get { return program_name; }
            set { program_name = value; }
        }

        public string Arguments
        {
            get { return arguments; }
            set { arguments = value; }
        }

        public string workingDirectory
        {
            get { return working_directory; }
            set { working_directory = value; }
        }

        public string errorMessage
        {
            get { return error_message; }
            set { error_message = value; }
        }

        #endregion

        #region private methods

        private void Start()
        {
            if (sandbox_mode == "dotnet")
            {                
                //sandboxer.Execute();
            }
            else if (sandbox_mode == "windows")
            {
                // sandboxer.Execute();
            }
            else
            {
                error_message = "Invalid sandbox mode";
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