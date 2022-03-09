using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sandboxer;
using sandboxer.AppLoader;
using sandboxer.Definitions;
using sandboxer.winsand;

namespace sandboxer.interactive
{
    public partial class SandboxerUI : Form
    {
        public SandboxerUI()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textBox1.AppendText(SandboxerGlobals.error_message);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            SandboxerGlobals.WorkingDirectory = this.workingdirectory.Text;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // Only one sandbox mode can be selected, so let's uncheck windows sandbox if it's already checked 

            if (this.windowssandbox.Checked)
            {
                this.windowssandbox.Checked = false;
            }

            if (this.dotnet.Checked)
            {
                SandboxerGlobals.SandboxMode = RunningModes.CONSOLE;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update available permissions by checked items
            for (int i = 0; i < (this.checkedpermissions.Items.Count); i++)  
            {  
                if (checkedpermissions.GetItemChecked(i))  
                {
                    if(checkedpermissions.Items[i].ToString() == "FileSystem Access")
                    {
                        SandboxerGlobals.PermissionSelections.FileSystemAcess = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        SandboxerGlobals.PermissionSelections.Networking = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Execution = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "User Interface(UI) Permission")
                    {
                        SandboxerGlobals.PermissionSelections.UserInterface = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Reflection Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Reflection = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Non-DotNet Program")
                    {
                        SandboxerGlobals.PermissionSelections.NoneDotNet = true;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Security Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Security = true;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Audio Access")
                    {
                        SandboxerGlobals.PermissionSelections.AudioAccess = true;
                    }                    
                    else if(checkedpermissions.Items[i].ToString() == "Printing Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Printing = true;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Web Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Web = true;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "SMTP Permission")
                    {
                        SandboxerGlobals.PermissionSelections.SMTP = true;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Registry Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Registry = true;
                    }
                }
                else
                {
                    if (checkedpermissions.Items[i].ToString() == "FileSystem Access")
                    {
                        SandboxerGlobals.PermissionSelections.FileSystemAcess = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        SandboxerGlobals.PermissionSelections.Networking = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Execution = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "User Interface(UI) Permission")
                    {
                        SandboxerGlobals.PermissionSelections.UserInterface = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Reflection")
                    {
                        SandboxerGlobals.PermissionSelections.Reflection = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Non-DotNet Program")
                    {
                        SandboxerGlobals.PermissionSelections.NoneDotNet = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Security Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Security = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Audio Access")
                    {
                        SandboxerGlobals.PermissionSelections.AudioAccess = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Printing Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Printing = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Web Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Web = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "SMTP Permission")
                    {
                        SandboxerGlobals.PermissionSelections.SMTP = false;
                    }
                    else if(checkedpermissions.Items[i].ToString() == "Registry Permission")
                    {
                        SandboxerGlobals.PermissionSelections.Registry = false;
                    }
                }
            }            
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
        }

        private void programname_TextChanged(object sender, EventArgs e)
        {
            SandboxerGlobals.ProgramToRun = this.programname.Text;
        }

        private void arguments_TextChanged(object sender, EventArgs e)
        {
            SandboxerGlobals.ArgumentsForProgram = this.arguments.Text.Split(' ');
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            
        }

        private void windowssandbox_CheckedChanged(object sender, EventArgs e)
        {
            // Only one sandbox mode can be selected, so let's uncheck dot net mode if it's already checked 
            if (this.dotnet.Checked)
            {
                this.dotnet.Checked = false;                
            }

            if(this.windowssandbox.Checked)
            {
                SandboxerGlobals.SandboxMode = RunningModes.POWERSHELLVM;
            }
        }

        private void SandboxerUI_Load(object sender, EventArgs e)
        {
            // Initializing default settings for the Sanboxer UI

            this.workingdirectory.Text = SandboxerGlobals.WorkingDirectory;
            this.programname.Text = SandboxerGlobals.ProgramToRun;

            // join string array to a single string with comma separator
            this.arguments.Text = string.Join(",", SandboxerGlobals.ArgumentsForProgram);

            this.networkaddress.Text = SandboxerGlobals.NetworkAddress;
            // add error messages to the listbox
            for (int i = 0; i < SandboxerGlobals.ErrorMessage.Count; i++)
            {
                consolelog.Items.Add(SandboxerGlobals.ErrorMessage[i]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Only one sandbox mode can be selected, so let's uncheck dot net mode if it's already checked 
            if (this.dotnet.Checked)
            {
                this.dotnet.Checked = false;
            }

            if (this.windowssandbox.Checked)
            {
                SandboxerGlobals.SandboxMode = RunningModes.POWERSHELLVM;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SandboxerGlobals.SandboxMode == RunningModes.NONE)
            {
                RuntimeException.Debug("Please select a sandbox mode");
                return;
            }

            if (!string.IsNullOrWhiteSpace(workingdirectory.Text))
            {
                SandboxerGlobals.WorkingDirectory = workingdirectory.Text;
            }
            else
            {
                SandboxerGlobals.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            // get the current values from the UI
            if (!string.IsNullOrWhiteSpace(programname.Text))
            {
                string program_path = Path.Combine(SandboxerGlobals.WorkingDirectory, programname.Text);
                // check if file exists
                if (File.Exists(program_path))
                {
                    // set the program name
                    SandboxerGlobals.ProgramToRun = programname.Text;
                }
                else
                {
                    // file doesn't exist
                    SandboxerGlobals.RedirectMessageDisplay("The program File doesn't exist in the working directory");
                    return;
                }
            }
            
            // checking the sandbox mode selected so we can run it accordingly
            if (SandboxerGlobals.SandboxMode == RunningModes.POWERSHELLVM)
            {
                try
                {
                    SandboxerGlobals.StartWindowsSandbox();
                }
                catch (Exception ex)
                {
                    RuntimeException.Debug(ex.Message);
                }
                return;
            }
            else
            {
                try
                {
                    SandboxerGlobals.LoadSandboxEnvironment();
                }
                catch (Exception ex)
                {
                    RuntimeException.Debug(ex.Message);
                }
                return;
            }
        }

        private void arguments_TextChanged_1(object sender, EventArgs e)
        {
            SandboxerGlobals.ArgumentsForProgram = this.arguments.Text.Split(' ');
        }

        private void SandboxerUI_Load_2(object sender, EventArgs e)
        {
            // add error messages to the listbox
            for (int i = 0; i < SandboxerGlobals.ErrorMessage.Count; i++)
            {
                consolelog.Items.Add(SandboxerGlobals.ErrorMessage[i]);
            }
        }

        private void programname_TextChanged_1(object sender, EventArgs e)
        {
            SandboxerGlobals.ProgramToRun = this.programname.Text;
        }

        private void consolelog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void networkaddress_TextChanged(object sender, EventArgs e)
        {
            SandboxerGlobals.NetworkAddress = networkaddress.Text;
        }

        private void custompermissions_TextChanged_1(object sender, EventArgs e)
        {
            SandboxerGlobals.CustomPermissions.Add(custompermissions.Text);
        }
    }
}
