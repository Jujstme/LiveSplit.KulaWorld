namespace LiveSplit.KulaWorld
{
    partial class Settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkrunStart = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkWorld = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkrunStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(455, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Options";
            // 
            // chkrunStart
            // 
            this.chkrunStart.AutoSize = true;
            this.chkrunStart.Checked = true;
            this.chkrunStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkrunStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkrunStart.Location = new System.Drawing.Point(10, 23);
            this.chkrunStart.Name = "chkrunStart";
            this.chkrunStart.Size = new System.Drawing.Size(109, 17);
            this.chkrunStart.TabIndex = 0;
            this.chkrunStart.Text = "Enable Auto Start";
            this.chkrunStart.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkWorld);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(10, 60);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(455, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Splitting Options";
            // 
            // chkWorld
            // 
            this.chkWorld.AutoSize = true;
            this.chkWorld.Checked = true;
            this.chkWorld.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWorld.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkWorld.Location = new System.Drawing.Point(10, 23);
            this.chkWorld.Name = "chkWorld";
            this.chkWorld.Size = new System.Drawing.Size(213, 17);
            this.chkWorld.TabIndex = 0;
            this.chkWorld.Text = "Split only after completion of each world";
            this.chkWorld.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(475, 500);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkrunStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkWorld;
    }
}
