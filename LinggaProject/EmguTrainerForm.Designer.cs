namespace LinggaProject
{
    partial class EmguTrainerForm
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
            this.selectManualTrainingFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.selectManualTrainingFolderButton = new System.Windows.Forms.Button();
            this.explanationText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // selectManualTrainingFolderButton
            // 
            this.selectManualTrainingFolderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectManualTrainingFolderButton.Location = new System.Drawing.Point(12, 12);
            this.selectManualTrainingFolderButton.Name = "selectManualTrainingFolderButton";
            this.selectManualTrainingFolderButton.Size = new System.Drawing.Size(85, 23);
            this.selectManualTrainingFolderButton.TabIndex = 0;
            this.selectManualTrainingFolderButton.Text = "Select Folder";
            this.selectManualTrainingFolderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.selectManualTrainingFolderButton.UseVisualStyleBackColor = true;
            this.selectManualTrainingFolderButton.Click += new System.EventHandler(this.selectManualTrainingFolderButton_Click);
            // 
            // explanationText
            // 
            this.explanationText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explanationText.Location = new System.Drawing.Point(13, 42);
            this.explanationText.Name = "explanationText";
            this.explanationText.Size = new System.Drawing.Size(863, 271);
            this.explanationText.TabIndex = 1;
            this.explanationText.Text = "";
            // 
            // EmguTrainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 325);
            this.Controls.Add(this.explanationText);
            this.Controls.Add(this.selectManualTrainingFolderButton);
            this.Name = "EmguTrainerForm";
            this.Text = "TrainerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog selectManualTrainingFolderDialog;
        private System.Windows.Forms.Button selectManualTrainingFolderButton;
        private System.Windows.Forms.RichTextBox explanationText;
    }
}