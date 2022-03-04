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

        // set sandbox mode to none
        public static RunningModes sandbox_mode = RunningModes.NONE;
        public static string program_name = string.Empty;
        public static string[] arguments = new string[0];
        public static List<string> error_message = new List<string>();
        public static PermissionDict available_permissions = new PermissionDict()
        {
            Networking = false,
            FileSystemAcess = false,
            Execution = false,
        };
        
        public static List<string> custom_permissions = new List<string>();
        public static string working_directory = Environment.CurrentDirectory;

        public static SandboxerUI sandboxer_ui = null;

        #endregion

        #region private methods

        public static void Execute()
        {
            error_message.Add(" ");
            error_message.Add("Execution from UI started");
            error_message.Add(" ");

            ExecuteFromUI.PopulateGlobalVariables();

            error_message.Add("Execution from UI completed");
        }

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

        public PermissionDict availablePermissions
        {
            get { return Variables.available_permissions; }
            set { Variables.available_permissions = value; }
        }

        public string programName
        {
            get { return Variables.program_name; }
            set { Variables.program_name = value; }
        }

        public string[] Arguments
        {
            get { return Variables.arguments; }
            set { Variables.arguments = value; }
        }

        public string workingDirectory
        {
            get { return Variables.working_directory; }
            set { Variables.working_directory = value; }
        }

        public List<string> errorMessage
        {
            get { return Variables.error_message; }
            set {Variables.error_message = value;}
        }

        public List<string> customPermissions
        {
            get { return Variables.custom_permissions; }
            set { Variables.custom_permissions = value; }
        }

        #endregion

        #region public methods

        public void ShowUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Variables.sandboxer_ui = new SandboxerUI();
            Application.Run(Variables.sandboxer_ui);            
        }

        public void GetCurrentPermissions()
        {
            // update available permissions by checked items
            for (int i = 0; i < (Variables.sandboxer_ui.checkedpermissions.Items.Count); i++)  
            {  
                if (Variables.sandboxer_ui.checkedpermissions.GetItemChecked(i))  
                {
                    if(Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Directory Access")
                    {
                        Variables.available_permissions.FileSystemAcess = true;
                    }
                    else if (Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        Variables.available_permissions.Networking = true;
                    }
                    else if (Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        Variables.available_permissions.Execution = true;
                    }
                }
                else
                {
                    if (Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Directory Access")
                    {
                        Variables.available_permissions.FileSystemAcess = false;
                    }
                    else if (Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        Variables.available_permissions.Networking = false;
                    }
                    else if (Variables.sandboxer_ui.checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        Variables.available_permissions.Execution = false;
                    }
                }
            }
        }

        // this would help us update the log box for new messages about the state of the sandbox
        public void RefreshConsoleLog()
        {
            if(Variables.sandboxer_ui != null)
            {                
                Variables.sandboxer_ui.consolelog.Items.Clear();

                // add error messages to the listbox
                for (int i = 0; i < Variables.error_message.Count; i++)
                {
                    Variables.sandboxer_ui.consolelog.Items.Add(Variables.error_message[i]);
                }

                Variables.sandboxer_ui.consolelog.Refresh();
            }
        }

        #endregion
    }
}