using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class FrmDatVe : Form
    {
        ImageList lstviewItemImageList = new ImageList();
         KetNoiCSDL kn = new KetNoiCSDL();
        UserControl us;
        FrmDangNhap dn = new FrmDangNhap();
        public Point mouseLocation;
        string manv = "";
        public FrmDatVe()
        {
            InitializeComponent();
             kn.ketNoi();           
        }
        public FrmDatVe(string manv)
        {
            InitializeComponent();
            this.manv = manv;
            kn.ketNoi();
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
            lblTilte.Font = new Font(lblTilte.Font.FontFamily, 22, FontStyle.Bold);
            timer1.Stop();
            btn.BackColor = Color.Maroon;
        }
        private void click()
        {
            btnTrangChu.BackColor = colorDialog1.Color;            
            btnThongTin.BackColor = colorDialog1.Color;
            btnDatVe.BackColor = colorDialog1.Color;
            btnHoTro.BackColor = colorDialog1.Color;
            btnTimVe.BackColor = colorDialog1.Color;
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
            }
        }

        private void FrmThemLichChieu_Load(object sender, EventArgs e)
        {
            btnTrangChu_Click(null,null);     
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

        private void pHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mous = Control.MousePosition;
                mous.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mous;
            }
        }

        private void pHeader_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }
        private string Quyen(string manv)
        {
            string quyen = "";
            string strSql = "select * from NhanVien where MaNhanVien='"+manv+"'";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                quyen = rd["ChucVu"].ToString();
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            return quyen;
        }
        private void btnDatVe_Click(object sender, EventArgs e)
        {
            //Truyền vào mã nv 
            us = new UserControlLichChieu(manv);
            showUserControl(us);
            doiTitle(btnDatVe);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn đăng xuất không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (r == DialogResult.Yes)
            {
                this.Close();
                dn.Visible = true;
            }
        }

        private void btnThongTin_Click(object sender, EventArgs e)
        {
            us = new UserControlHeThong(manv);
            showUserControl(us);
            doiTitle(btnThongTin);
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            us = new UserControlTrangChu();
            showUserControl(us);
            doiTitle(btnTrangChu);
            timer1.Start();
            lblTilte.Text = "Chào mừng bạn đến với chương trình quản lý rạp phim";
        }

        private void btnTimVe_Click(object sender, EventArgs e)
        {
            us = new UserControlTimVe();
            showUserControl(us);
            doiTitle(btnTimVe);
        }

        private void btnHoTro_Click(object sender, EventArgs e)
        {
            FrmHoTro ht = new FrmHoTro();
            ht.ShowDialog();
        }
    }
}
