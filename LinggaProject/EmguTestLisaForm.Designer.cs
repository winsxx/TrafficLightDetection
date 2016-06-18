namespace LinggaProject
{
    partial class EmguTestLisaForm
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
            if (disposing && (components != null)) {
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
            this.selectLisaFolderButton = new System.Windows.Forms.Button();
            this.lisaFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // selectLisaFolderButton
            // 
            this.selectLisaFolderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectLisaFolderButton.Location = new System.Drawing.Point(12, 12);
            this.selectLisaFolderButton.Name = "selectLisaFolderButton";
            this.selectLisaFolderButton.Size = new System.Drawing.Size(109, 23);
            this.selectLisaFolderButton.TabIndex = 2;
            this.selectLisaFolderButton.Text = "Test From Folder";
            this.selectLisaFolderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectLisaFolderButton.UseVisualStyleBackColor = true;
            this.selectLisaFolderButton.Click += new System.EventHandler(this.selectLisaFolderButton_Click);
            // 
            // EmguTestLisaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.selectLisaFolderButton);
            this.Name = "EmguTestLisaForm";
            this.Text = "EmguTestLisaForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button selectLisaFolderButton;
        private System.Windows.Forms.FolderBrowserDialog lisaFolderDialog;
    }
}