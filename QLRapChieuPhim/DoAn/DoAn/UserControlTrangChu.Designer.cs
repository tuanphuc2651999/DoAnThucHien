namespace DoAn
{
    partial class UserControlTrangChu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTrangChu));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pRight = new System.Windows.Forms.Panel();
            this.pLeft = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pChinhLeft = new System.Windows.Forms.Panel();
            this.pChinh = new System.Windows.Forms.Panel();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pChinhLeft.SuspendLayout();
            this.pChinh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // colorDialog2
            // 
            this.colorDialog2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(50, 697);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1432, 50);
            this.panel3.TabIndex = 4;
            // 
            // pRight
            // 
            this.pRight.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(1482, 0);
            this.pRight.Name = "pRight";
            this.pRight.Size = new System.Drawing.Size(50, 747);
            this.pRight.TabIndex = 3;
            // 
            // pLeft
            // 
            this.pLeft.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 0);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(50, 747);
            this.pLeft.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 747);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1532, 106);
            this.panel5.TabIndex = 1;
            // 
            // pChinhLeft
            // 
            this.pChinhLeft.BackColor = System.Drawing.Color.White;
            this.pChinhLeft.Controls.Add(this.pictureBox1);
            this.pChinhLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pChinhLeft.Location = new System.Drawing.Point(50, 0);
            this.pChinhLeft.Name = "pChinhLeft";
            this.pChinhLeft.Size = new System.Drawing.Size(1432, 697);
            this.pChinhLeft.TabIndex = 5;
            // 
            // pChinh
            // 
            this.pChinh.Controls.Add(this.pChinhLeft);
            this.pChinh.Controls.Add(this.panel3);
            this.pChinh.Controls.Add(this.pRight);
            this.pChinh.Controls.Add(this.pLeft);
            this.pChinh.Controls.Add(this.panel5);
            this.pChinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pChinh.Location = new System.Drawing.Point(0, 0);
            this.pChinh.Name = "pChinh";
            this.pChinh.Size = new System.Drawing.Size(1532, 853);
            this.pChinh.TabIndex = 10;
            // 
            // ofdOpenFile
            // 
            this.ofdOpenFile.FileName = "openFileDialog1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1432, 697);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // UserControlTrangChu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pChinh);
            this.Name = "UserControlTrangChu";
            this.Size = new System.Drawing.Size(1532, 853);
            this.Load += new System.EventHandler(this.UserControlTrangChu_Load);
            this.pChinhLeft.ResumeLayout(false);
            this.pChinh.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pChinhLeft;
        private System.Windows.Forms.Panel pChinh;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
        private System.Windows.Forms.PictureBox pictureBox1;





    }
}
