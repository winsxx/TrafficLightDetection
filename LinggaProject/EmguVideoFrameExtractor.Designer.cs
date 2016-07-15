namespace LinggaProject
{
    partial class EmguVideoFrameExtractor
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
            this.explanationText = new System.Windows.Forms.RichTextBox();
            this.browseVideoButton = new System.Windows.Forms.Button();
            this.videoDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // explanationText
            // 
            this.explanationText.Location = new System.Drawing.Point(12, 41);
            this.explanationText.Name = "explanationText";
            this.explanationText.Size = new System.Drawing.Size(679, 331);
            this.explanationText.TabIndex = 0;
            this.explanationText.Text = "";
            // 
            // browseVideoButton
            // 
            this.browseVideoButton.Location = new System.Drawing.Point(12, 12);
            this.browseVideoButton.Name = "browseVideoButton";
            this.browseVideoButton.Size = new System.Drawing.Size(106, 23);
            this.browseVideoButton.TabIndex = 1;
            this.browseVideoButton.Text = "Browse Video";
            this.browseVideoButton.UseVisualStyleBackColor = true;
            this.browseVideoButton.Click += new System.EventHandler(this.browseVideoButton_Click);
            // 
            // EmguVideoFrameExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 384);
            this.Controls.Add(this.browseVideoButton);
            this.Controls.Add(this.explanationText);
            this.Name = "EmguVideoFrameExtractor";
            this.Text = "EmguVideoFrameExtractor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox explanationText;
        private System.Windows.Forms.Button browseVideoButton;
        private System.Windows.Forms.OpenFileDialog videoDialog;
    }
}