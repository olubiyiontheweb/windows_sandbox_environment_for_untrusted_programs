using System;
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
using sandboxer.interactive;
using sandboxer.winsand;

namespace interactiveSandboxer
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
            //textBox1.AppendText(Variables.error_message);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Variables.working_directory = this.workingdirectory.Text;
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
                Variables.sandbox_mode = RunningModes.CONSOLE;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update available permissions by checked items
            for (int i = 0; i < (checkedpermissions.Items.Count); i++)  
            {  
                if (checkedpermissions.GetItemChecked(i))  
                {
                    if(checkedpermissions.Items[i].ToString() == "Directory Access")
                    {
                        Variables.available_permissions.FileSystemAcess = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        Variables.available_permissions.Networking = true;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        Variables.available_permissions.Execution = true;
                    }
                }
                else
                {
                    if (checkedpermissions.Items[i].ToString() == "Directory Access")
                    {
                        Variables.available_permissions.FileSystemAcess = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Network Access")
                    {
                        Variables.available_permissions.Networking = false;
                    }
                    else if (checkedpermissions.Items[i].ToString() == "Execution Permission")
                    {
                        Variables.available_permissions.Execution = false;
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
            Variables.program_name = this.programname.Text;
        }

        private void arguments_TextChanged(object sender, EventArgs e)
        {
            Variables.arguments = this.arguments.Text.Split(' ');
        }

        private void custompermissions_TextChanged(object sender, EventArgs e)
        {
            Variables.custom_permissions = this.custompermissions.Text.Split(',').ToList();
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            Variables.Execute();
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
                Variables.sandbox_mode = RunningModes.POWERSHELLVM;
            }
        }

        private void SandboxerUI_Load(object sender, EventArgs e)
        {
            // add error messages to the listbox
            for (int i = 0; i < Variables.error_message.Count; i++)
            {
                consolelog.Items.Add(Variables.error_message[i]);
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
                Variables.sandbox_mode = RunningModes.POWERSHELLVM;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Variables.Execute();
        }

        private void arguments_TextChanged_1(object sender, EventArgs e)
        {
            Variables.arguments = this.arguments.Text.Split(' ');
        }

        private void SandboxerUI_Load_2(object sender, EventArgs e)
        {
            // add error messages to the listbox
            for (int i = 0; i < Variables.error_message.Count; i++)
            {
                consolelog.Items.Add(Variables.error_message[i]);
            }
        }

        private void custompermissions_TextChanged_1(object sender, EventArgs e)
        {
            Variables.custom_permissions = this.custompermissions.Text.Split(',').ToList();
        }

        private void programname_TextChanged_1(object sender, EventArgs e)
        {
            Variables.program_name = this.programname.Text;
        }

        private void consolelog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
