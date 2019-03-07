namespace SharpCATForms
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ComPort1Tab = new System.Windows.Forms.TabPage();
            this.ComPort2Tab = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.ComPortListBox = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.ComPort1Tab.SuspendLayout();
            this.ComPort2Tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ComPort1Tab);
            this.tabControl1.Controls.Add(this.ComPort2Tab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(364, 410);
            this.tabControl1.TabIndex = 0;
            // 
            // ComPort1Tab
            // 
            this.ComPort1Tab.Controls.Add(this.textBox1);
            this.ComPort1Tab.Location = new System.Drawing.Point(4, 22);
            this.ComPort1Tab.Name = "ComPort1Tab";
            this.ComPort1Tab.Padding = new System.Windows.Forms.Padding(3);
            this.ComPort1Tab.Size = new System.Drawing.Size(356, 384);
            this.ComPort1Tab.TabIndex = 0;
            this.ComPort1Tab.Text = "Port 1";
            this.ComPort1Tab.UseVisualStyleBackColor = true;
            // 
            // ComPort2Tab
            // 
            this.ComPort2Tab.Controls.Add(this.textBox2);
            this.ComPort2Tab.Location = new System.Drawing.Point(4, 22);
            this.ComPort2Tab.Name = "ComPort2Tab";
            this.ComPort2Tab.Padding = new System.Windows.Forms.Padding(3);
            this.ComPort2Tab.Size = new System.Drawing.Size(356, 384);
            this.ComPort2Tab.TabIndex = 1;
            this.ComPort2Tab.Text = "Port 2";
            this.ComPort2Tab.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 7);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(340, 371);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(7, 7);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(340, 371);
            this.textBox2.TabIndex = 1;
            // 
            // ComPortListBox
            // 
            this.ComPortListBox.FormattingEnabled = true;
            this.ComPortListBox.Location = new System.Drawing.Point(462, 48);
            this.ComPortListBox.Name = "ComPortListBox";
            this.ComPortListBox.Size = new System.Drawing.Size(120, 95);
            this.ComPortListBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ComPortListBox);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.ComPort1Tab.ResumeLayout(false);
            this.ComPort1Tab.PerformLayout();
            this.ComPort2Tab.ResumeLayout(false);
            this.ComPort2Tab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ComPort1Tab;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage ComPort2Tab;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ListBox ComPortListBox;
    }
}

