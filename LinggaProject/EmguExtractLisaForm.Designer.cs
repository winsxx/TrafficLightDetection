namespace LinggaProject
{
    partial class EmguExtractLisaForm
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
            this.trainExtractedButton = new System.Windows.Forms.Button();
            this.lisaFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.nbInstances = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectLisaFolderButton
            // 
            this.selectLisaFolderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectLisaFolderButton.Location = new System.Drawing.Point(12, 12);
            this.selectLisaFolderButton.Name = "selectLisaFolderButton";
            this.selectLisaFolderButton.Size = new System.Drawing.Size(109, 23);
            this.selectLisaFolderButton.TabIndex = 1;
            this.selectLisaFolderButton.Text = "Extract From Folder";
            this.selectLisaFolderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectLisaFolderButton.UseVisualStyleBackColor = true;
            this.selectLisaFolderButton.Click += new System.EventHandler(this.selectLisaFolderButton_Click);
            // 
            // trainExtractedButton
            // 
            this.trainExtractedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trainExtractedButton.Location = new System.Drawing.Point(12, 41);
            this.trainExtractedButton.Name = "trainExtractedButton";
            this.trainExtractedButton.Size = new System.Drawing.Size(97, 23);
            this.trainExtractedButton.TabIndex = 2;
            this.trainExtractedButton.Text = "Train Extracted";
            this.trainExtractedButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trainExtractedButton.UseVisualStyleBackColor = true;
            // 
            // nbInstances
            // 
            this.nbInstances.Location = new System.Drawing.Point(128, 14);
            this.nbInstances.Name = "nbInstances";
            this.nbInstances.Size = new System.Drawing.Size(43, 20);
            this.nbInstances.TabIndex = 3;
            this.nbInstances.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Instances";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EmguExtractLisaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nbInstances);
            this.Controls.Add(this.trainExtractedButton);
            this.Controls.Add(this.selectLisaFolderButton);
            this.Name = "EmguExtractLisaForm";
            this.Text = "EmguExtractLisaForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectLisaFolderButton;
        private System.Windows.Forms.Button trainExtractedButton;
        private System.Windows.Forms.FolderBrowserDialog lisaFolderDialog;
        private System.Windows.Forms.TextBox nbInstances;
        private System.Windows.Forms.Label label1;
    }
}