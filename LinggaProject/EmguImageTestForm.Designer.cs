namespace LinggaProject
{
    partial class EmguImageTestForm
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
            this.browseImageButton = new System.Windows.Forms.Button();
            this.testImageBox = new Emgu.CV.UI.ImageBox();
            this.testImageDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.testImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // browseImageButton
            // 
            this.browseImageButton.Location = new System.Drawing.Point(13, 13);
            this.browseImageButton.Name = "browseImageButton";
            this.browseImageButton.Size = new System.Drawing.Size(75, 23);
            this.browseImageButton.TabIndex = 0;
            this.browseImageButton.Text = "Browse Image";
            this.browseImageButton.UseVisualStyleBackColor = true;
            this.browseImageButton.Click += new System.EventHandler(this.browseImageButton_Click);
            // 
            // testImageBox
            // 
            this.testImageBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            this.testImageBox.Location = new System.Drawing.Point(13, 43);
            this.testImageBox.Name = "testImageBox";
            this.testImageBox.Size = new System.Drawing.Size(902, 578);
            this.testImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.testImageBox.TabIndex = 2;
            this.testImageBox.TabStop = false;
            // 
            // EmguImageTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 633);
            this.Controls.Add(this.testImageBox);
            this.Controls.Add(this.browseImageButton);
            this.Name = "EmguImageTestForm";
            this.Text = "Image Testing";
            ((System.ComponentModel.ISupportInitialize)(this.testImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button browseImageButton;
        private Emgu.CV.UI.ImageBox testImageBox;
        private System.Windows.Forms.OpenFileDialog testImageDialog;
    }
}