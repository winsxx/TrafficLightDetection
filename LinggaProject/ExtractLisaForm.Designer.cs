namespace LinggaProject
{
    partial class ExtractLisaForm
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
            this.startExtractionButton = new System.Windows.Forms.Button();
            this.explanationBox = new System.Windows.Forms.RichTextBox();
            this.startTrainingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startExtractionButton
            // 
            this.startExtractionButton.Location = new System.Drawing.Point(12, 12);
            this.startExtractionButton.Name = "startExtractionButton";
            this.startExtractionButton.Size = new System.Drawing.Size(114, 23);
            this.startExtractionButton.TabIndex = 1;
            this.startExtractionButton.Text = "Start Extraction";
            this.startExtractionButton.UseVisualStyleBackColor = true;
            this.startExtractionButton.Click += new System.EventHandler(this.startExtractionButton_Click);
            // 
            // explanationBox
            // 
            this.explanationBox.Location = new System.Drawing.Point(12, 41);
            this.explanationBox.Name = "explanationBox";
            this.explanationBox.Size = new System.Drawing.Size(609, 351);
            this.explanationBox.TabIndex = 2;
            this.explanationBox.Text = "";
            // 
            // startTrainingButton
            // 
            this.startTrainingButton.Location = new System.Drawing.Point(132, 12);
            this.startTrainingButton.Name = "startTrainingButton";
            this.startTrainingButton.Size = new System.Drawing.Size(114, 23);
            this.startTrainingButton.TabIndex = 3;
            this.startTrainingButton.Text = "Start Training";
            this.startTrainingButton.UseVisualStyleBackColor = true;
            this.startTrainingButton.Click += new System.EventHandler(this.startTrainingButton_Click);
            // 
            // ExtractLisaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 404);
            this.Controls.Add(this.startTrainingButton);
            this.Controls.Add(this.explanationBox);
            this.Controls.Add(this.startExtractionButton);
            this.Name = "ExtractLisaForm";
            this.Text = "ExtractLisaForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button startExtractionButton;
        private System.Windows.Forms.RichTextBox explanationBox;
        private System.Windows.Forms.Button startTrainingButton;
    }
}