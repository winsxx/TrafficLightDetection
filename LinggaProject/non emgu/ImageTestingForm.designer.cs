namespace LinggaProject
{
    partial class ImageTestingForm
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
            this.testImageButton = new System.Windows.Forms.Button();
            this.testImageBox = new System.Windows.Forms.PictureBox();
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.imageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.processedImageCursor = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.originalImageCursor = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.explanationBox = new System.Windows.Forms.RichTextBox();
            this.detectedListContainer = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.testFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.testImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testImageButton
            // 
            this.testImageButton.Location = new System.Drawing.Point(12, 12);
            this.testImageButton.Name = "testImageButton";
            this.testImageButton.Size = new System.Drawing.Size(174, 23);
            this.testImageButton.TabIndex = 5;
            this.testImageButton.Text = "Browse Image";
            this.testImageButton.UseVisualStyleBackColor = true;
            this.testImageButton.Click += new System.EventHandler(this.testImageButton_Click);
            // 
            // testImageBox
            // 
            this.testImageBox.Location = new System.Drawing.Point(0, 0);
            this.testImageBox.Name = "testImageBox";
            this.testImageBox.Size = new System.Drawing.Size(755, 583);
            this.testImageBox.TabIndex = 6;
            this.testImageBox.TabStop = false;
            this.testImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.testImageBox_MouseMove);
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(0, 0);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(755, 587);
            this.originalImageBox.TabIndex = 7;
            this.originalImageBox.TabStop = false;
            this.originalImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.originalImageBox_MouseMove);
            // 
            // imageFileDialog
            // 
            this.imageFileDialog.FileName = "imageFileDialog";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 41);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(763, 609);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.processedImageCursor);
            this.tabPage1.Controls.Add(this.testImageBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(755, 583);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Processed Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // processedImageCursor
            // 
            this.processedImageCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.processedImageCursor.AutoSize = true;
            this.processedImageCursor.Location = new System.Drawing.Point(668, 563);
            this.processedImageCursor.Name = "processedImageCursor";
            this.processedImageCursor.Size = new System.Drawing.Size(23, 13);
            this.processedImageCursor.TabIndex = 9;
            this.processedImageCursor.Text = "x, y";
            this.processedImageCursor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.originalImageCursor);
            this.tabPage2.Controls.Add(this.originalImageBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(755, 583);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Original Image";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // originalImageCursor
            // 
            this.originalImageCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.originalImageCursor.AutoSize = true;
            this.originalImageCursor.Location = new System.Drawing.Point(668, 563);
            this.originalImageCursor.Name = "originalImageCursor";
            this.originalImageCursor.Size = new System.Drawing.Size(23, 13);
            this.originalImageCursor.TabIndex = 8;
            this.originalImageCursor.Text = "x, y";
            this.originalImageCursor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.explanationBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(755, 583);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Explanation";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // explanationBox
            // 
            this.explanationBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explanationBox.Location = new System.Drawing.Point(0, 0);
            this.explanationBox.Name = "explanationBox";
            this.explanationBox.Size = new System.Drawing.Size(752, 580);
            this.explanationBox.TabIndex = 0;
            this.explanationBox.Text = "";
            // 
            // detectedListContainer
            // 
            this.detectedListContainer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.detectedListContainer.AutoScroll = true;
            this.detectedListContainer.AutoSize = true;
            this.detectedListContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.detectedListContainer.ColumnCount = 5;
            this.detectedListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.detectedListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.detectedListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.detectedListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.detectedListContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.detectedListContainer.Location = new System.Drawing.Point(36, 3);
            this.detectedListContainer.MaximumSize = new System.Drawing.Size(280, 0);
            this.detectedListContainer.MinimumSize = new System.Drawing.Size(280, 0);
            this.detectedListContainer.Name = "detectedListContainer";
            this.detectedListContainer.RowCount = 4;
            this.detectedListContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detectedListContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detectedListContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detectedListContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.detectedListContainer.Size = new System.Drawing.Size(280, 0);
            this.detectedListContainer.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.detectedListContainer);
            this.panel1.Location = new System.Drawing.Point(781, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 550);
            this.panel1.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(781, 601);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(379, 49);
            this.button1.TabIndex = 13;
            this.button1.Text = "Manual Labelling";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // testFileDialog
            // 
            this.testFileDialog.FileName = "testFileDialog";
            // 
            // ImageTestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1172, 696);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.testImageButton);
            this.Name = "ImageTestingForm";
            this.Text = "TA Lingga";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.testImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button testImageButton;
        private System.Windows.Forms.PictureBox testImageBox;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.OpenFileDialog imageFileDialog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel detectedListContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label processedImageCursor;
        private System.Windows.Forms.Label originalImageCursor;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox explanationBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog testFileDialog;
    }
}

