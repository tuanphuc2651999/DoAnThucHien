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
    public partial class FrmAdmin : Form
    {
        public Point mouseLocation;
        UserControl us;
        FrmDangNhap dn = new FrmDangNhap();
        private string manv = "";
        public FrmAdmin()
        {
            
            InitializeComponent();
           // btnTrangChu_Click(null, null);
        }
        public FrmAdmin(string manv)
        {
            InitializeComponent();
            this.manv = manv;
            // btnTrangChu_Click(null, null);
        }
        private void click()
        {
            btnTrangChu.BackColor = colorDialog1.Color;
            btnNhanVien.BackColor = colorDialog1.Color;
            btnPhongChieu.BackColor = colorDialog1.Color;
            btnThongKe.BackColor = colorDialog1.Color;
            btnLichChieu.BackColor = colorDialog1.Color;
            btnPhim.BackColor = colorDialog1.Color;
            btnThongTin.BackColor = colorDialog1.Color;

            btnHoTro.BackColor = colorDialog1.Color;
        }

        private void showUserControl(UserControl user)
        {
            pCenter.Controls.Add(user);
            user.Dock = DockStyle.Fill;
            user.BringToFront();
        }
        private void doiTitle(Button btn)
        {
            click();
            lblTilte.Text = btn.Text;
            int size1 = pTitle.Size.Width;
            lblTilte.Location = new Point(size1 / 2 - lblTilte.Size.Width / 2, 14);
            lblTilte.ForeColor = Color.Red;
            lblTilte.Font = new Font(lblTilte.Font.FontFamily,22,FontStyle.Bold);
            timer1.Stop();
            btn.BackColor = Color.Maroon;
        }
        public void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn đăng xuất không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (r == DialogResult.Yes)
            {
                this.Close();
                dn.Visible = true;
            }
        }
        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            btnTrangChu_Click(null, null);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (r == DialogResult.Yes)
                Environment.Exit(0);
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

        private void btnLichChieu_Click(object sender, EventArgs e)
        {
            us = new UserControlLichChieu(manv);
            showUserControl(us);
            doiTitle(btnLichChieu);
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            us = new UserControlPhim();
            showUserControl(us);
            doiTitle(btnPhim);
        }
        int x = 1052, y = 13, a = 1;//hoặc x=0;
        Random random = new Random();
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                x = x - a;
                lblTilte.Location = new Point(x, y);
                if (x == -800)
                {
                    x = 1052;
                    lblTilte.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            us = new UserControlThongKe();
            showUserControl(us);
            doiTitle(btnThongKe);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            us = new UserControlNhanVien();
            showUserControl(us);
            doiTitle(btnNhanVien);
        }

        private void btnPhongChieu_Click(object sender, EventArgs e)
        {
            us = new UserControlPhongChieu();
            showUserControl(us);
            doiTitle(btnPhongChieu);
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            click();
            btnTrangChu.BackColor = Color.Maroon;
            us = new UserControlTrangChu();
            showUserControl(us);
            timer1.Start();
            lblTilte.Text = "Chào mừng bạn đến với chương trình quản lý rạp phim";
        }

        private void btnThongTin_Click(object sender, EventArgs e)
        {
            us = new UserControlHeThong(manv);
            showUserControl(us);
            doiTitle(btnThongTin);
        }

        private void btnDatVe_Click(object sender, EventArgs e)
        {
            //us = new UserControlDatVeTuAdmin(manv);
            //showUserControl(us);
        }
        private void btnHoTro_Click(object sender, EventArgs e)
        {
            FrmHoTro ht = new FrmHoTro();
            ht.ShowDialog();
        }
    }
}
