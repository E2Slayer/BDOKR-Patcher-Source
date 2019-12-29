namespace BDOKRPatch
{
    partial class Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Update));
            this.logTextBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // logTextBox
            // 
            // 
            // 
            // 
            this.logTextBox.CustomButton.Image = null;
            this.logTextBox.CustomButton.Location = new System.Drawing.Point(326, 2);
            this.logTextBox.CustomButton.Name = "";
            this.logTextBox.CustomButton.Size = new System.Drawing.Size(135, 135);
            this.logTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.logTextBox.CustomButton.TabIndex = 1;
            this.logTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.logTextBox.CustomButton.UseSelectable = true;
            this.logTextBox.CustomButton.Visible = false;
            this.logTextBox.Lines = new string[] {
        "Waiting..."};
            this.logTextBox.Location = new System.Drawing.Point(23, 87);
            this.logTextBox.MaxLength = 32767;
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.PasswordChar = '\0';
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.logTextBox.SelectedText = "";
            this.logTextBox.SelectionLength = 0;
            this.logTextBox.SelectionStart = 0;
            this.logTextBox.ShortcutsEnabled = true;
            this.logTextBox.Size = new System.Drawing.Size(464, 140);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "Waiting...";
            this.logTextBox.UseSelectable = true;
            this.logTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.logTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(97, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "업데이트 로그";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(190, 247);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(107, 38);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "확인";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 308);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.logTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Update";
            this.Text = "업데이트 체크";
            this.Load += new System.EventHandler(this.Update_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox logTextBox;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}