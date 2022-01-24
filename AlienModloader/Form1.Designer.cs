namespace AlienModloader
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
            this.button1 = new System.Windows.Forms.Button();
            this.PathBox = new System.Windows.Forms.TextBox();
            this.GenerateChecksumsBut = new System.Windows.Forms.Button();
            this.CheckSumBox = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.LoadChecksumsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Set Path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PathBox
            // 
            this.PathBox.Enabled = false;
            this.PathBox.Location = new System.Drawing.Point(105, 13);
            this.PathBox.Name = "PathBox";
            this.PathBox.Size = new System.Drawing.Size(353, 20);
            this.PathBox.TabIndex = 1;
            // 
            // GenerateChecksumsBut
            // 
            this.GenerateChecksumsBut.Location = new System.Drawing.Point(13, 42);
            this.GenerateChecksumsBut.Name = "GenerateChecksumsBut";
            this.GenerateChecksumsBut.Size = new System.Drawing.Size(445, 23);
            this.GenerateChecksumsBut.TabIndex = 2;
            this.GenerateChecksumsBut.Text = "Generate Checksums";
            this.GenerateChecksumsBut.UseVisualStyleBackColor = true;
            this.GenerateChecksumsBut.Click += new System.EventHandler(this.GenerateChecksumsBut_Click);
            // 
            // CheckSumBox
            // 
            this.CheckSumBox.FormattingEnabled = true;
            this.CheckSumBox.Items.AddRange(new object[] {
            ""});
            this.CheckSumBox.Location = new System.Drawing.Point(13, 71);
            this.CheckSumBox.Name = "CheckSumBox";
            this.CheckSumBox.Size = new System.Drawing.Size(445, 147);
            this.CheckSumBox.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 224);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(445, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Export Checksums";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LoadChecksumsButton
            // 
            this.LoadChecksumsButton.Location = new System.Drawing.Point(12, 253);
            this.LoadChecksumsButton.Name = "LoadChecksumsButton";
            this.LoadChecksumsButton.Size = new System.Drawing.Size(445, 23);
            this.LoadChecksumsButton.TabIndex = 5;
            this.LoadChecksumsButton.Text = "Load Checksums";
            this.LoadChecksumsButton.UseVisualStyleBackColor = true;
            this.LoadChecksumsButton.Click += new System.EventHandler(this.LoadChecksumsButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 450);
            this.Controls.Add(this.LoadChecksumsButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.CheckSumBox);
            this.Controls.Add(this.GenerateChecksumsBut);
            this.Controls.Add(this.PathBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox PathBox;
        private System.Windows.Forms.Button GenerateChecksumsBut;
        private System.Windows.Forms.ListBox CheckSumBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button LoadChecksumsButton;
    }
}

