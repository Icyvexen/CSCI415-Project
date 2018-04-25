namespace MainForm
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
            this.searchButton = new System.Windows.Forms.Button();
            this.DisplayBox = new System.Windows.Forms.TextBox();
            this.batchOrderCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(15, 16);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(122, 52);
            this.searchButton.TabIndex = 0;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // DisplayBox
            // 
            this.DisplayBox.Location = new System.Drawing.Point(181, 33);
            this.DisplayBox.Multiline = true;
            this.DisplayBox.Name = "DisplayBox";
            this.DisplayBox.Size = new System.Drawing.Size(355, 379);
            this.DisplayBox.TabIndex = 1;
            // 
            // batchOrderCheck
            // 
            this.batchOrderCheck.AutoSize = true;
            this.batchOrderCheck.Location = new System.Drawing.Point(32, 100);
            this.batchOrderCheck.Name = "batchOrderCheck";
            this.batchOrderCheck.Size = new System.Drawing.Size(60, 17);
            this.batchOrderCheck.TabIndex = 2;
            this.batchOrderCheck.Text = "Batch?";
            this.batchOrderCheck.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.batchOrderCheck);
            this.Controls.Add(this.DisplayBox);
            this.Controls.Add(this.searchButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox DisplayBox;
        private System.Windows.Forms.CheckBox batchOrderCheck;
    }
}

