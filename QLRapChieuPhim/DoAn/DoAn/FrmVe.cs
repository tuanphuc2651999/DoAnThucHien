using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class FrmVe : Form
    {
        public Point mouseLocation;
        public string maLichChieu = "";
        public string ngayChieu = "";
        public string gioChieu = "";
        public string MaPhim = "";
        string maNV = "";
        public Image AnhPhim;
        public FrmVe()
        {
            InitializeComponent();                  
        }
        public FrmVe(string manv)
        {
            InitializeComponent();
            this.maNV = manv;
        }
       
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            //dn.Visible = true;
        }

        private void FrmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No)
                e.Cancel = true;
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            UserControlBookVe us = new UserControlBookVe(maNV);
            us.MaLichChieu = maLichChieu;
            us.ngayChieu = ngayChieu;
            us.gioChieu = gioChieu;
            us.AnhPhim = AnhPhim;
            us.MaPhim = MaPhim;
            pCenter.Controls.Add(us);
            us.Dock = DockStyle.Fill;
            us.BringToFront();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnRestore.Visible = true;
            btnMaximizar.Visible = false;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestore.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pHeader_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void pHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mous = Control.MousePosition;
                mous.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mous;
            }
        }

    }
}
