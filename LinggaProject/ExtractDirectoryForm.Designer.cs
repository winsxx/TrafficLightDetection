namespace LinggaProject
{
    partial class ExtractDirectoryForm
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
            this.explanationBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // explanationBox
            // 
            this.explanationBox.Location = new System.Drawing.Point(12, 41);
            this.explanationBox.Name = "explanationBox";
            this.explanationBox.Size = new System.Drawing.Size(609, 351);
            this.explanationBox.TabIndex = 3;
            this.explanationBox.Text = "";
            // 
            // ExtractDirectoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 404);
            this.Controls.Add(this.explanationBox);
            this.Name = "ExtractDirectoryForm";
            this.Text = "ExtractDirectoryForm";
            this.Load += new System.EventHandler(this.ExtractDirectoryForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox explanationBox;
    }
}