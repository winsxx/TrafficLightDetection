namespace LinggaProject
{
    partial class EmguMLForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.buildClassifierButton = new System.Windows.Forms.Button();
            this.generateLisaDatasetButton = new System.Windows.Forms.Button();
            this.videoTestButton = new System.Windows.Forms.Button();
            this.automatedLisaTestButton = new System.Windows.Forms.Button();
            this.imageTestButton = new System.Windows.Forms.Button();
            this.nonEmguButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Penyederhanaan algoritma seluruhnya menggunakan EmguCV";
            // 
            // buildClassifierButton
            // 
            this.buildClassifierButton.Location = new System.Drawing.Point(15, 26);
            this.buildClassifierButton.Name = "buildClassifierButton";
            this.buildClassifierButton.Size = new System.Drawing.Size(206, 23);
            this.buildClassifierButton.TabIndex = 1;
            this.buildClassifierButton.Text = "Build SVM Classifier Using Manual Data";
            this.buildClassifierButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buildClassifierButton.UseVisualStyleBackColor = true;
            this.buildClassifierButton.Click += new System.EventHandler(this.buildClassifierButton_Click);
            // 
            // generateLisaDatasetButton
            // 
            this.generateLisaDatasetButton.Location = new System.Drawing.Point(15, 113);
            this.generateLisaDatasetButton.Name = "generateLisaDatasetButton";
            this.generateLisaDatasetButton.Size = new System.Drawing.Size(132, 23);
            this.generateLisaDatasetButton.TabIndex = 2;
            this.generateLisaDatasetButton.Text = "Generate LISA Test Set";
            this.generateLisaDatasetButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generateLisaDatasetButton.UseVisualStyleBackColor = true;
            this.generateLisaDatasetButton.Click += new System.EventHandler(this.generateLisaDatasetButton_Click);
            // 
            // videoTestButton
            // 
            this.videoTestButton.Location = new System.Drawing.Point(15, 84);
            this.videoTestButton.Name = "videoTestButton";
            this.videoTestButton.Size = new System.Drawing.Size(84, 23);
            this.videoTestButton.TabIndex = 3;
            this.videoTestButton.Text = "Video Testing";
            this.videoTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.videoTestButton.UseVisualStyleBackColor = true;
            this.videoTestButton.Click += new System.EventHandler(this.videoTestButton_Click);
            // 
            // automatedLisaTestButton
            // 
            this.automatedLisaTestButton.Location = new System.Drawing.Point(15, 142);
            this.automatedLisaTestButton.Name = "automatedLisaTestButton";
            this.automatedLisaTestButton.Size = new System.Drawing.Size(132, 23);
            this.automatedLisaTestButton.TabIndex = 4;
            this.automatedLisaTestButton.Text = "Automated LISA Testing";
            this.automatedLisaTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.automatedLisaTestButton.UseVisualStyleBackColor = true;
            // 
            // imageTestButton
            // 
            this.imageTestButton.Location = new System.Drawing.Point(15, 55);
            this.imageTestButton.Name = "imageTestButton";
            this.imageTestButton.Size = new System.Drawing.Size(84, 23);
            this.imageTestButton.TabIndex = 5;
            this.imageTestButton.Text = "Image Testing";
            this.imageTestButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imageTestButton.UseVisualStyleBackColor = true;
            this.imageTestButton.Click += new System.EventHandler(this.imageTestButton_Click);
            // 
            // nonEmguButton
            // 
            this.nonEmguButton.Location = new System.Drawing.Point(15, 171);
            this.nonEmguButton.Name = "nonEmguButton";
            this.nonEmguButton.Size = new System.Drawing.Size(84, 23);
            this.nonEmguButton.TabIndex = 6;
            this.nonEmguButton.Text = "Non Emgu";
            this.nonEmguButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nonEmguButton.UseVisualStyleBackColor = true;
            this.nonEmguButton.Click += new System.EventHandler(this.nonEmguButton_Click);
            // 
            // EmguMLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 209);
            this.Controls.Add(this.nonEmguButton);
            this.Controls.Add(this.imageTestButton);
            this.Controls.Add(this.automatedLisaTestButton);
            this.Controls.Add(this.videoTestButton);
            this.Controls.Add(this.generateLisaDatasetButton);
            this.Controls.Add(this.buildClassifierButton);
            this.Controls.Add(this.label1);
            this.Name = "EmguMLForm";
            this.Text = "EmguMLForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buildClassifierButton;
        private System.Windows.Forms.Button generateLisaDatasetButton;
        private System.Windows.Forms.Button videoTestButton;
        private System.Windows.Forms.Button automatedLisaTestButton;
        private System.Windows.Forms.Button imageTestButton;
        private System.Windows.Forms.Button nonEmguButton;
    }
}