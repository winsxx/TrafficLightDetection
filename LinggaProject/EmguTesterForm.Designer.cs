namespace LinggaProject
{
    partial class EmguTesterForm
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
            this.explanationText = new System.Windows.Forms.RichTextBox();
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
            // explanationText
            // 
            this.explanationText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explanationText.Location = new System.Drawing.Point(13, 42);
            this.explanationText.Name = "explanationText";
            this.explanationText.Size = new System.Drawing.Size(863, 271);
            this.explanationText.TabIndex = 3;
            this.explanationText.Text = "";
            // 
            // EmguTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 325);
            this.Controls.Add(this.explanationText);
            this.Controls.Add(this.selectLisaFolderButton);
            this.Name = "EmguTesterForm";
            this.Text = "Automatic Testing";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button selectLisaFolderButton;
        private System.Windows.Forms.FolderBrowserDialog lisaFolderDialog;
        private System.Windows.Forms.RichTextBox explanationText;
    }
}