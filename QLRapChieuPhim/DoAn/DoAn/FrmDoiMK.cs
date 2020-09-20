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
    public partial class FrmDoiMK : Form
    {
        public Point mouseLocation;
        KetNoiCSDL kn = new KetNoiCSDL();
        public FrmDoiMK()
        {
            InitializeComponent();
            kn.ketNoi();
        }
        public FrmDoiMK(string manv)
        {
            InitializeComponent();
            kn.ketNoi();
            txtTenDN.Text = manv;          
        }
        private void doiMK(string s)
        {
            string strUpdate = "update NhanVien set MatKhau='" + txtMKMoi.Text + "' WHERE MaNhanVien='" + txtTenDN.Text + "'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strUpdate, kn.Conn);
            cmd.ExecuteNonQuery();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        private bool kiemTraMKCu(string s)
        {
            try
            {
                string str = "select count(*) from nhanvien WHERE taikhoan='" + txtTenDN.Text + "' AND MatKhau='" + s + "'";
                if (kn.Conn.State == ConnectionState.Closed)
                    kn.Conn.Open();
                SqlCommand cmd = new SqlCommand(str, kn.Conn);
                int count = (int)cmd.ExecuteScalar();

                if (kn.Conn.State == ConnectionState.Open)
                    kn.Conn.Close();
                if (count > 0)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        private void btnDongY_Click(object sender, EventArgs e)
        {
            if (txtMKCu.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu cũ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKCu.Focus();
                return;
            }
            if (txtMKMoi.Text == "")
            {
                MessageBox.Show("Chưa nhập mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKMoi.Focus();
                return;
            }
            if (txtXacNhanMK.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtXacNhanMK.Focus();
                return;
            }
            if (kiemTraMKCu(txtMKCu.Text))
            {
                MessageBox.Show("Mật khẩu cũ không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKCu.Clear();
                txtMKMoi.Clear();
                txtXacNhanMK.Clear();
                txtMKCu.Focus();
                return;
            }
            if (txtMKMoi.Text.Equals(txtXacNhanMK.Text) == false)
            {
                MessageBox.Show("Xác nhân mật khẩu không đúng với mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKCu.Clear();
                txtMKMoi.Clear();
                txtXacNhanMK.Clear();
                txtMKCu.Focus();
                return;
            }
            if (txtMKMoi.TextLength < 6)
            {
                MessageBox.Show("Mật khẩu mới phải từ 6 kí tự trờ lên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMKCu.Clear();
                txtMKMoi.Clear();
                txtXacNhanMK.Clear();
                txtMKCu.Focus();
                return;
            }
            doiMK(txtMKMoi.Text);
            MessageBox.Show("Đổi mật khẩu thành công quay lại From quản lý", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();          
        }

        private void txtMKCu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnDongY_Click(null, null);
            }
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            btnExit_Click(null, null); 
        }


        private void FrmDoiMK_Load(object sender, EventArgs e)
        {
            txtMKCu.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn trở về From quản lý không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
            {
                this.Close();
            }
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
