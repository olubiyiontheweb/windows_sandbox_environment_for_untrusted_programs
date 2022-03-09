
namespace sandboxer.interactive
{
    partial class SandboxerUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedpermissions = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.programname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.arguments = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.workingdirectory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.windowssandbox = new System.Windows.Forms.CheckBox();
            this.dotnet = new System.Windows.Forms.CheckBox();
            this.startbutton = new System.Windows.Forms.Button();
            this.consolelog = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.networkaddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.custompermissions = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkedpermissions
            // 
            this.checkedpermissions.CheckOnClick = true;
            this.checkedpermissions.FormattingEnabled = true;
            this.checkedpermissions.Items.AddRange(new object[] {
            "Audio Access",
            "Execution Permission",
            "FileSystem Access",
            "Network Access",
            "Non-DotNet Program",
            "Printing Permission",
            "Reflection Permission",
            "Registry Permission",
            "Security Permission",
            "SMTP Permission",
            "User Interface(UI) Permission",
            "Web Permission"});
            this.checkedpermissions.Location = new System.Drawing.Point(56, 84);
            this.checkedpermissions.Name = "checkedpermissions";
            this.checkedpermissions.Size = new System.Drawing.Size(268, 79);
            this.checkedpermissions.TabIndex = 1;
            this.checkedpermissions.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Permissions";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(113, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 34);
            this.label2.TabIndex = 3;
            this.label2.Text = "Please select the permissions you want to allow in the sandbox environment";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Console Logs";
            // 
            // programname
            // 
            this.programname.Location = new System.Drawing.Point(494, 23);
            this.programname.Name = "programname";
            this.programname.Size = new System.Drawing.Size(240, 20);
            this.programname.TabIndex = 6;
            this.programname.TextChanged += new System.EventHandler(this.programname_TextChanged_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(393, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "File name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(393, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Arguments";
            // 
            // arguments
            // 
            this.arguments.Location = new System.Drawing.Point(494, 63);
            this.arguments.Name = "arguments";
            this.arguments.Size = new System.Drawing.Size(240, 20);
            this.arguments.TabIndex = 8;
            this.arguments.TextChanged += new System.EventHandler(this.arguments_TextChanged_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(393, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Sandbox Type";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // workingdirectory
            // 
            this.workingdirectory.Location = new System.Drawing.Point(396, 159);
            this.workingdirectory.Name = "workingdirectory";
            this.workingdirectory.Size = new System.Drawing.Size(338, 20);
            this.workingdirectory.TabIndex = 10;
            this.workingdirectory.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(393, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Working Directory";
            // 
            // windowssandbox
            // 
            this.windowssandbox.AutoSize = true;
            this.windowssandbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowssandbox.Location = new System.Drawing.Point(510, 102);
            this.windowssandbox.Name = "windowssandbox";
            this.windowssandbox.Size = new System.Drawing.Size(142, 21);
            this.windowssandbox.TabIndex = 14;
            this.windowssandbox.Text = "Windows Sandbox";
            this.windowssandbox.UseVisualStyleBackColor = true;
            this.windowssandbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // dotnet
            // 
            this.dotnet.AutoSize = true;
            this.dotnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dotnet.Location = new System.Drawing.Point(665, 103);
            this.dotnet.Name = "dotnet";
            this.dotnet.Size = new System.Drawing.Size(69, 21);
            this.dotnet.TabIndex = 15;
            this.dotnet.Text = "Dotnet";
            this.dotnet.UseVisualStyleBackColor = true;
            this.dotnet.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // startbutton
            // 
            this.startbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.startbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startbutton.Location = new System.Drawing.Point(659, 253);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(75, 32);
            this.startbutton.TabIndex = 16;
            this.startbutton.Text = "Start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // consolelog
            // 
            this.consolelog.FormattingEnabled = true;
            this.consolelog.HorizontalScrollbar = true;
            this.consolelog.Location = new System.Drawing.Point(56, 304);
            this.consolelog.Name = "consolelog";
            this.consolelog.Size = new System.Drawing.Size(678, 134);
            this.consolelog.TabIndex = 20;
            this.consolelog.SelectedIndexChanged += new System.EventHandler(this.consolelog_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(393, 192);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Network Address";
            // 
            // networkaddress
            // 
            this.networkaddress.Location = new System.Drawing.Point(396, 217);
            this.networkaddress.Name = "networkaddress";
            this.networkaddress.Size = new System.Drawing.Size(338, 20);
            this.networkaddress.TabIndex = 21;
            this.networkaddress.TextChanged += new System.EventHandler(this.networkaddress_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(53, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "Custom Permissions";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(91, 192);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(233, 34);
            this.label9.TabIndex = 19;
            this.label9.Text = "Please refer to the guide on how to add custom permissions for your prefered sand" +
    "box type";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // custompermissions
            // 
            this.custompermissions.Location = new System.Drawing.Point(56, 229);
            this.custompermissions.Multiline = true;
            this.custompermissions.Name = "custompermissions";
            this.custompermissions.Size = new System.Drawing.Size(268, 38);
            this.custompermissions.TabIndex = 17;
            this.custompermissions.TextChanged += new System.EventHandler(this.custompermissions_TextChanged_1);
            // 
            // SandboxerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.networkaddress);
            this.Controls.Add(this.consolelog);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.custompermissions);
            this.Controls.Add(this.startbutton);
            this.Controls.Add(this.dotnet);
            this.Controls.Add(this.windowssandbox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.workingdirectory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.arguments);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.programname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedpermissions);
            this.Controls.Add(this.label2);
            this.Name = "SandboxerUI";
            this.Text = "Sandboxer UI";
            this.Load += new System.EventHandler(this.SandboxerUI_Load_2);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckedListBox checkedpermissions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox programname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox arguments;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox workingdirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox windowssandbox;
        private System.Windows.Forms.CheckBox dotnet;
        private System.Windows.Forms.Button startbutton;
        public System.Windows.Forms.ListBox consolelog;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox networkaddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox custompermissions;
    }
}

