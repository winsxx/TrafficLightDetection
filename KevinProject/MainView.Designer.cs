namespace WinsonProject
{
    partial class MainView
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
            if (disposing && (components != null))
            {
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
            this.ib1 = new Emgu.CV.UI.ImageBox();
            this.ib2 = new Emgu.CV.UI.ImageBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tb1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ib1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ib2)).BeginInit();
            this.SuspendLayout();
            // 
            // ib1
            // 
            this.ib1.Location = new System.Drawing.Point(12, 12);
            this.ib1.Name = "ib1";
            this.ib1.Size = new System.Drawing.Size(640, 480);
            this.ib1.TabIndex = 2;
            this.ib1.TabStop = false;
            // 
            // ib2
            // 
            this.ib2.Location = new System.Drawing.Point(668, 12);
            this.ib2.Name = "ib2";
            this.ib2.Size = new System.Drawing.Size(640, 480);
            this.ib2.TabIndex = 2;
            this.ib2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(230, 498);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 122);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pause";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(411, 498);
            this.tb1.Multiline = true;
            this.tb1.Name = "tb1";
            this.tb1.ReadOnly = true;
            this.tb1.Size = new System.Drawing.Size(691, 122);
            this.tb1.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(12, 498);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(212, 33);
            this.button2.TabIndex = 5;
            this.button2.Text = "Video Drive Singapore";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1223, 501);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 30);
            this.label1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1223, 545);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 30);
            this.label2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1223, 590);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 30);
            this.label3.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(12, 537);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(212, 36);
            this.button3.TabIndex = 9;
            this.button3.Text = "Video Jalan Raya";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(12, 579);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(212, 36);
            this.button4.TabIndex = 10;
            this.button4.Text = "Camera";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 632);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ib2);
            this.Controls.Add(this.ib1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ib1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ib2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox ib1;
        private Emgu.CV.UI.ImageBox ib2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

