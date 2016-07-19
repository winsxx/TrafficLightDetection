namespace LinggaProject
{
    partial class EmguVideoTestForm
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
            this.components = new System.ComponentModel.Container();
            this.browseVideoButton = new System.Windows.Forms.Button();
            this.testVideoDialog = new System.Windows.Forms.OpenFileDialog();
            this.testVideoBox = new Emgu.CV.UI.ImageBox();
            this.explanationText = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.testVideoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // browseVideoButton
            // 
            this.browseVideoButton.Location = new System.Drawing.Point(12, 12);
            this.browseVideoButton.Name = "browseVideoButton";
            this.browseVideoButton.Size = new System.Drawing.Size(75, 23);
            this.browseVideoButton.TabIndex = 3;
            this.browseVideoButton.Text = "Browse Image";
            this.browseVideoButton.UseVisualStyleBackColor = true;
            this.browseVideoButton.Click += new System.EventHandler(this.browseVideoButton_Click);
            // 
            // testVideoBox
            // 
            this.testVideoBox.Location = new System.Drawing.Point(12, 41);
            this.testVideoBox.Name = "testVideoBox";
            this.testVideoBox.Size = new System.Drawing.Size(903, 503);
            this.testVideoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.testVideoBox.TabIndex = 4;
            this.testVideoBox.TabStop = false;
            // 
            // explanationText
            // 
            this.explanationText.Location = new System.Drawing.Point(12, 551);
            this.explanationText.Name = "explanationText";
            this.explanationText.Size = new System.Drawing.Size(903, 96);
            this.explanationText.TabIndex = 5;
            this.explanationText.Text = "";
            // 
            // EmguVideoTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 658);
            this.Controls.Add(this.explanationText);
            this.Controls.Add(this.testVideoBox);
            this.Controls.Add(this.browseVideoButton);
            this.Name = "EmguVideoTestForm";
            this.Text = "Video Testing";
            this.Load += new System.EventHandler(this.EmguVideoTestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testVideoBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button browseVideoButton;
        private System.Windows.Forms.OpenFileDialog testVideoDialog;
        private Emgu.CV.UI.ImageBox testVideoBox;
        private System.Windows.Forms.RichTextBox explanationText;
    }
}