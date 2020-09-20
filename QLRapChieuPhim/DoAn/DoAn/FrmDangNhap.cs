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
    public partial class FrmDangNhap : Form
    {
        public Point mouseLocation;
        KetNoiCSDL kn = new KetNoiCSDL();
        Form frm;
        string manv="";
        public FrmDangNhap()
        {
            InitializeComponent();
            kn.ketNoi();
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
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (r == DialogResult.Yes)
                Environment.Exit(0);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        int i=0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 3)
            {
                pictureBox2.Image = Image.FromFile("..\\..\\img\\imgdn2.jpg");
              
            }
            else if (i == 6)
            {
                pictureBox2.Image = Image.FromFile("..\\..\\img\\imgdn3.jpg");
               
            }
            else if (i == 9)
            {
                pictureBox2.Image = Image.FromFile("..\\..\\img\\imgdn4.jpg");
                
            }
            else if (i == 12)
            {
                pictureBox2.Image = Image.FromFile("..\\..\\img\\imgdn1.jpg");
                i = 0;
                timer1.Stop();
                timer1.Start();
            }
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
           
        }

        //private void DangNhap_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    DialogResult r;
        //    r = MessageBox.Show("Bạn có chắc muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        //    if (r == DialogResult.No)
        //        e.Cancel = true;
        //    else
        //        Environment.Exit(0);
        //}

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenTK.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên đăng nhâp !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenTK.Focus();
                return;
            }
            if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }
            string sql = "select * from NhanVien where TaiKhoan='" + txtTenTK.Text + "' AND MatKhau='" + txtMatKhau.Text + "' ";
            SqlDataAdapter da = new SqlDataAdapter(sql, kn.Conn);
            DataTable dtb = new DataTable();
            da.Fill(dtb);
            if (dtb.Rows.Count > 0)
            {
                if (String.Compare(dtb.Rows[0]["ChucVu"].ToString(), "Quản lý", true) == 0)
                {
                    frm = new FrmAdmin(dtb.Rows[0][0].ToString());                
                    frm.Visible = true;
                    
                    this.Visible = false;
                }
                else if (String.Compare(dtb.Rows[0]["ChucVu"].ToString(), "Nhân viên", true) == 0)
                {
                    frm = new FrmDatVe(dtb.Rows[0][0].ToString());
                    frm.Visible = true;
                    this.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Clear();
            }
        }
       
        private void txtTenTK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnDangNhap_Click(null, null);
            }
        }
    }
}
