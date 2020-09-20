using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAn
{
    public partial class UserControlHeThong : UserControl
    {
        KetNoiCSDL kn = new KetNoiCSDL();
        FrmAdmin ad = new FrmAdmin();
        FrmDoiMK frm;
        String manv = "";
        string file = "";
        DataSet ds;
        SqlDataAdapter da;
        DataTable dtb;
        public UserControlHeThong(string s)
        {
            InitializeComponent();
            kn.ketNoi();
            this.manv = s;
            loadThongTin(s);
        }
        private void loadBangGia()
        {
            string s = "select * from Gia";
            ds = new DataSet();
            dtb = new DataTable();
            DataColumn[] key = new DataColumn[1];
            da = new SqlDataAdapter(s, kn.Conn);
            da.Fill(dtb);
            datagirlGia.DataSource = dtb;
            datagirlGia.Columns["ID"].Visible = false;
        }
        public void loadThongTin(string s)
        {
            string strSql = "select * from NhanVien where manhanvien='" + s + "'";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                try
                {
                    Image img = Image.FromFile(rd["img"].ToString());
                    pbHinh.Image = img;
                }
                catch (Exception)
                {
                    MessageBox.Show("Hình bị lỗi xin cập nhật lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Image img = Image.FromFile("..\\..\\img\\hinhmacdinh.jpg");
                    pbHinh.Image = img;
                }
                //file = row.Cells[12].Value.ToString();
                txtMaNV.Text = rd["MaNhanVien"].ToString();
                txtHoTen.Text = rd["Hoten"].ToString();
                string gioitinh = rd["GioiTinh"].ToString();
                if (gioitinh == "True")
                {
                    cboGioiTinh.SelectedIndex = 1;
                }
                else
                    cboGioiTinh.SelectedIndex = 0;
                dtpNgaySinh.Text = rd["NgaySinh"].ToString();
                dtpNgayVL.Text = rd["NgayVL"].ToString();
                cboDanToc.Text = rd["DanToc"].ToString();
                txtCMND.Text = rd["SoCMND"].ToString();
                txtDiaChi.Text = rd["DiaChi"].ToString();
                txtSDT.Text = rd["SDT"].ToString();
                cboChucVu.Text = rd["ChucVu"].ToString();
                txtTaiKhoan.Text = rd["TaiKhoan"].ToString();
                txtMatKhau.Text = rd["MatKhau"].ToString();
                file = rd["Img"].ToString();
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
           
            frm = new FrmDoiMK(txtMaNV.Text);
            frm.ShowDialog();
        }
        public void xemThongTin(bool s)
        {
            txtHoTen.ReadOnly = s;
            txtCMND.ReadOnly = s;
            txtDiaChi.ReadOnly = s;
            txtSDT.ReadOnly = s;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            btnLuu.Visible = true;
            btnHuy.Visible = true;
            btnLoad.Visible = true;
            btnSua.Visible = false;
            xemThongTin(false);
            dtpNgaySinh.Enabled = true;
            cboDanToc.Enabled = true;
            cboGioiTinh.Enabled = true;
            dtpNgayVL.Enabled = true;
            btnDoiMatKhau.Visible = false;
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray(); // chuỗi thành mảng ký tự
            Array.Reverse(arr); // đảo ngược mảng
            return new string(arr); // trả về chuỗi mới sau khi đảo mảng
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {

            ofdOpenFile.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All files (*.*)|*.*";
            if (ofdOpenFile.ShowDialog() == DialogResult.OK)
            {
                file = ofdOpenFile.FileName;
                Image img = Image.FromFile(file);
                pbHinh.Image = img;
                string[] hinh = ReverseString(file).Split('\\');
                file = "..\\..\\img\\" + ReverseString(hinh[0]);
            }

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnLuu.Visible = false;
            btnHuy.Visible = false;
            btnLoad.Visible = false;
            btnSua.Visible = true;
            loadThongTin(manv);
            xemThongTin(true);
            btnSua.Focus();
            dtpNgaySinh.Enabled = false;
            cboDanToc.Enabled = false;
            cboGioiTinh.Enabled = false;
            dtpNgayVL.Enabled = false;
            btnDoiMatKhau.Visible = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

            if (txtHoTen.Text == string.Empty)
            {
                MessageBox.Show("Bạn chưa nhâp tên nhân viên", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return;
            }
            if (dtpNgaySinh.Value == dtpNgaySinh.MinDate)
            {
                MessageBox.Show("Hãy nhập ngày sinh", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return;
            }
            else if (kiemTraNgaySinh() != true)
            {
                MessageBox.Show("Ngày sinh không hợp lệ xin nhập lại", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return;
            }

            string ngayvl = "";

            if (dtpNgayVL.Value.Equals(DateTime.Now))
                ngayvl = dtpNgayVL.Value.ToShortDateString();
            else
            {
                if (kiemTraNgayVaoLam() != true)
                {
                    MessageBox.Show("Ngày vào làm không thể lớn hơn ngày hiện tại", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpNgayVL.Focus();
                    return;
                }
                if (kiemTraTuoi() != true)
                {
                    MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpNgayVL.Focus();
                    return;
                }
                ngayvl = dtpNgayVL.Value.ToShortDateString();
            }

            string strUpdate = "update NhanVien set HoTen =N'" + txtHoTen.Text + "' ,GioiTinh='" + cboGioiTinh.SelectedIndex.ToString() + "' ,NgaySinh ='" + dtpNgaySinh.Value + "' ,NgayVL='"
                + ngayvl + "' ,SoCMND='" + txtCMND.Text + "' ,DiaChi=N'" + txtDiaChi.Text + "' ,SDT='" + txtSDT.Text
                + "' ,img='" + file + "' WHERE MaNhanVien='" + txtMaNV.Text + "'";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strUpdate, kn.Conn);
            cmd.ExecuteNonQuery();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            MessageBox.Show("Sửa thành công", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnHuy.Visible = false;
            btnLuu.Visible = false;
            btnSua.Visible = true;
            btnSua.Focus();
            btnDoiMatKhau.Visible = true;
            btnLoad.Visible = false;
        }
        private void txtCMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(txtCMND, "Chứng minh nhân dân không được nhập chữ");
            }
            else
            {
                errorProvider.Clear();
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(txtSDT, "Số điện thoại không thể nhập chữ");
            }
            else
            {
                errorProvider.Clear();
            }
        }
        bool kiemTraNgaySinh()
        {
            DateTime ngaySinh = Convert.ToDateTime(dtpNgaySinh.Value);
            DateTime ngayHienTai = Convert.ToDateTime(DateTime.Now);
            TimeSpan ngay = ngayHienTai.Subtract(ngaySinh);
            double days = ngay.TotalDays;
            if (days < 0)
                return false;
            return true;
        }
        bool kiemTraNgayVaoLam()
        {
            DateTime ngayVL = Convert.ToDateTime(dtpNgayVL.Value);
            DateTime ngayHienTai = Convert.ToDateTime(DateTime.Now);
            TimeSpan ngay = ngayHienTai.Subtract(ngayVL);
            double days = ngay.TotalDays;
            if (days < 0)
                return false;
            return true;

        }
        bool kiemTraTuoi()
        {
            DateTime ngayVL = Convert.ToDateTime(dtpNgayVL.Value);
            DateTime ngaySinh = Convert.ToDateTime(dtpNgaySinh.Value);
            TimeSpan ngay = ngayVL.Subtract(ngaySinh);
            double days = ngay.TotalDays;
            string tuoi = Math.Floor(days / 365.0).ToString();
            if (int.Parse(tuoi) < 18)
                return false;
            return true;
        }
        private string Quyen(string manv)
        {
            string quyen = "";
            string strSql = "select * from NhanVien where MaNhanVien='" + manv + "'";

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
        private void UserControlHeThong_Load(object sender, EventArgs e)
        {
            loadBangGia();
            if (Quyen(manv).ToUpper().Equals("QUẢN LÝ"))
                btnSuaGia.Visible = true;
        }

        private void btnSuaGia_Click(object sender, EventArgs e)
        {
            btnLuuGia.Visible = true;
            btnHuyGia.Visible = true;
            btnSuaGia.Visible = false;
            txtGia.ReadOnly = false;
            txtTen.ReadOnly = false;
        }

        private void btnLuuGia_Click(object sender, EventArgs e)
        {
            btnLuuGia.Visible = false;
            btnHuyGia.Visible = false;
            btnSuaGia.Visible = true;
            txtTen.Focus();
                        if (txtTen.Text == "")
            {
                MessageBox.Show("Không được để chống tên đối tượng được hưởng giá đó","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtTen.Focus();
                return;
            }
            if (txtGia.Text == "")
            {
                MessageBox.Show("Giá không được để chống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGia.Focus();
                return;
            }

            string update = "update Gia set ten=N'"+txtTen.Text+"', Gia='"+txtGia.Text+"' WHERE ID='"+txtMaGia.Text+"'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(update, kn.Conn);
            cmd.ExecuteNonQuery();
           
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            MessageBox.Show("Sửa đổi thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);            
            datagirlGia.Enabled = true;
            loadBangGia();
            datagirlGia_SelectionChanged(null, null);
            txtTen.ReadOnly = true;
            txtGia.ReadOnly = true;
        }
        private void btnHuyGia_Click(object sender, EventArgs e)
        {
            btnLuuGia.Visible = false;
            btnHuyGia.Visible = false;
            btnSuaGia.Visible = true;
            datagirlGia.Enabled = false;
            txtMaGia.Clear();
            txtTen.Clear();
            txtGia.Clear();
            datagirlGia_SelectionChanged(null, null);
        }

        private void datagirlGia_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in datagirlGia.SelectedRows)
            {
                txtMaGia.Text = row.Cells[0].Value.ToString();
                txtTen.Text = row.Cells[1].Value.ToString();
                string s=row.Cells[2].Value.ToString();
                txtGia.Text =String.Format("{0:0,0}",double.Parse(s));
            }
        }

        private void txtGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                errorProvider.SetError(txtGia, "Giá không thể nhập chữ");
            }
            else
            {
                errorProvider.Clear();
            }
        }
    }
}
