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
    public partial class UserControlNhanVien : UserControl
    {
        KetNoiCSDL kn;
        DataSet ds_DS;
        SqlDataAdapter daNhanVien;
        DataTable dtNhanVien;
        string file;
        public UserControlNhanVien()
        {
            InitializeComponent();
            kn = new KetNoiCSDL();
            kn.ketNoi();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            click();
            clearThongTin();
            xemThongTin(false);
            pictureBox1.Image = null;
            datagirlDSNV.Enabled = false;
            file = "";
            //Ẩn hết các button và hiện button cần
            anButton();
            dtpNgaySinh.Value = dtpNgaySinh.MinDate;
            txtTaiKhoan.ReadOnly = true;
            cboChucVu.SelectedIndex = 1;
            btnLuu.Visible = true;
            btnHuy.Visible = true;
            btnLoad.Visible = true;
            txtTaiKhoan.Enabled = false;
            txtMatKhau.Enabled = false;
            ktkcbo(true);
            cboChucVu.Enabled = false;
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            click();
            if (MessageBox.Show("Bạn có muốn xóa không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                DataRow update = dtNhanVien.Rows.Find(txtMaNV.Text);
                if (update != null)
                    update.Delete();
                SqlCommandBuilder cmb = new SqlCommandBuilder(daNhanVien);
                daNhanVien.Update(dtNhanVien);
                MessageBox.Show("Bạn đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            datagirlDSNV.Focus();

        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            click();
            btnHuy.BackColor = Color.Maroon;
            datagirlDSNV.Enabled = false;
            xemThongTin(false);
            txtMaNV.Enabled = false;
            txtTaiKhoan.Enabled = false;
            //Ẩn hết button hiện những button cần
            anButton();
            btnHuy.Visible = true;
            btnLuu.Visible = true;
            btnLoad.Visible = true;
            ktkcbo(true);
            txtHoTen.Focus();
            //FrmSuaNV nv = new FrmSuaNV();
            //nv.ShowDialog();
        }
        public void ktkcbo(bool s)
        {
            cboGioiTinh.Enabled = s;
            dtpNgaySinh.Enabled = s;
            dtpNgayVL.Enabled = s;
            cboDanToc.Enabled = s;
            cboChucVu.Enabled = s;
        }
        public void loadDSNV(string s)
        {
            ds_DS = new DataSet();
            dtNhanVien = new DataTable();
            DataColumn[] key = new DataColumn[1];
            daNhanVien = new SqlDataAdapter(s, kn.Conn);
            daNhanVien.Fill(dtNhanVien);
            datagirlDSNV.DataSource = dtNhanVien;
            key[0] = dtNhanVien.Columns["MaNhanVien"];
            dtNhanVien.PrimaryKey = key;
            datagirlDSNV.Columns["NgaySinh"].Visible = false;
            datagirlDSNV.Columns["NgayVL"].Visible = false;
            datagirlDSNV.Columns["DanToc"].Visible = false;
            datagirlDSNV.Columns["SoCMND"].Visible = false;
            datagirlDSNV.Columns["DiaChi"].Visible = false;
            datagirlDSNV.Columns["SDT"].Visible = false;
            datagirlDSNV.Columns["img"].Visible = false;
            datagirlDSNV.Columns["GioiTinh"].Visible = false;
        }
        private void UserControlNhanVien_Load(object sender, EventArgs e)
        {
            string s = "SELECT * From NhanVien";
            loadDSNV(s);
            xemThongTin(true);
            ktkcbo(false);
            // Databingding(dtSinhVien);  
            datagirlDSNV_SelectionChanged(null, null);
            cboTimKiemChucVu.SelectedIndex = 0;
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
                pictureBox1.Image = img;
            }
            string[] hinh = ReverseString(file).Split('\\');
            file = ReverseString(hinh[0]);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text;
            string matKhau = txtMatKhau.Text;
            string cmnd = txtCMND.Text;
            string chucVu = cboChucVu.Text;
            string danToc = cboDanToc.Text;
            string SDT = txtSDT.Text;
            string diaChi = txtDiaChi.Text;
            string gioiTinh = cboGioiTinh.Text;
            if (txtMaNV.Text == string.Empty)
            {
                MessageBox.Show("Bạn chưa nhập mã nhân viên", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNV.Focus();
                return;
            }
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

            if (txtMaNV.Enabled == true)//Thêm
            {
                if (taiKhoan == "" || matKhau == "" || cmnd == "" || chucVu == "" || danToc == "" || SDT == "" || diaChi == "")//Nếu một trong những đk xẩy ra sẽ hỏi 
                {
                    DialogResult r;
                    r = MessageBox.Show("Thông tin chưa đầy đủ bạn có chắc muốn lưu không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (r == DialogResult.Yes)
                    {
                        if (kiemTraMaNV(txtMaNV.Text))
                        {
                            MessageBox.Show("Đã có mã nhân viên rồi !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMaNV.Focus();
                            return;
                        }
                        DataRow newrow = dtNhanVien.NewRow();
                        newrow["MaNhanVien"] = txtMaNV.Text;
                        newrow["HoTen"] = txtHoTen.Text;
                        newrow["TaiKhoan"] = txtTaiKhoan.Text;
                        newrow["MatKhau"] = dtpNgaySinh.Text.Substring(0, 10).Replace("-", "");
                        if (chucVu == "")
                            newrow["ChucVu"] = "Nhân viên";
                        else
                            newrow["ChucVu"] = cboChucVu.SelectedItem;


                        if (diaChi == "")
                            newrow["DiaChi"] = "Chưa rõ";
                        else
                            newrow["DiaChi"] = txtDiaChi.Text;
                        if (cmnd == "")
                            newrow["SoCMND"] = "123456789";
                        else
                        {
                            if (kiemtraCMND(txtCMND.Text) != true)
                            {
                                MessageBox.Show("Chứng minh nhân dân phải 9 hoặc 13 số", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtCMND.Focus();
                                return;
                            }
                            newrow["SoCMND"] = txtCMND.Text;
                        }

                        if (SDT == "")
                            newrow["SDT"] = "123456789";
                        else
                            newrow["SDT"] = txtSDT.Text;

                        if (gioiTinh == "")
                            newrow["GioiTinh"] = 1;
                        else
                            newrow["GioiTinh"] = cboGioiTinh.SelectedIndex;

                        if (danToc == "")
                        {                           
                            newrow["DanToc"] = "Kinh";
                        }
                        else
                            newrow["DanToc"] = cboDanToc.SelectedItem;

                        newrow["NgaySinh"] = dtpNgaySinh.Value;

                        if (dtpNgayVL.Value.Equals(DateTime.Now))
                            newrow["NgayVL"] = dtpNgayVL.Value;
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
                            newrow["NgayVL"] = dtpNgayVL.Value;
                        }
                        if (file == "")
                            newrow["img"] = "..\\..\\img\\hinhmacdinh.jpg";
                        else
                            newrow["img"] = "..\\..\\img\\"+file;
                        dtNhanVien.Rows.Add(newrow);
                    }
                    else
                    {
                        if (taiKhoan == "")
                        {
                            txtTaiKhoan.Focus();
                            return;
                        }
                        if (matKhau == "")
                        {
                            txtMatKhau.Focus();
                            return;
                        }
                        if (chucVu == "")
                        {
                            cboChucVu.Focus();
                            return;
                        }
                        if (diaChi == "")
                        {
                            txtDiaChi.Focus();
                            return;
                        }
                        if (cmnd == "")
                        {
                            txtCMND.Focus();
                            return;
                        }

                        if (gioiTinh == "")
                        {
                            cboGioiTinh.Focus();
                            return;
                        }
                        if (danToc == "")
                        {
                            cboDanToc.Focus();
                            return;
                        }
                    }
                }
            }
            else //Chỉnh sửa
            {
                DataRow update = dtNhanVien.Rows.Find(txtMaNV.Text);
                if (update != null)
                {
                    if (kiemtraCMND(txtCMND.Text) != true)
                    {
                        MessageBox.Show("Chứng minh nhân dân phải 9 hoặc 13 số", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCMND.Focus();
                        return;
                    }
                    if (kiemTraNgaySinh() != true)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ xin nhập lại", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpNgaySinh.Focus();
                        return;
                    }
                    if (kiemTraNgayVaoLam() != true)
                    {
                        MessageBox.Show("Ngày vào làm không thể lớn hơn ngày hiện tại", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpNgayVL.Focus();
                        return;
                    }
                    if (kiemTraTuoi() != true)
                    {
                        MessageBox.Show("Bạn chưa đủ tuổi", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpNgayVL.Focus();
                        return;
                    }
                    update["HoTen"] = txtHoTen.Text;
                    update["GioiTinh"] = cboGioiTinh.SelectedIndex;
                    update["NgaySinh"] = dtpNgaySinh.Value;
                    update["NgayVL"] = dtpNgayVL.Value;
                    update["DanToc"] = cboDanToc.SelectedItem;
                    update["SoCMND"] = txtCMND.Text;
                    update["DiaChi"] = txtDiaChi.Text;
                    update["SDT"] = txtSDT.Text;
                    update["ChucVu"] = cboChucVu.SelectedItem;
                    update["TaiKhoan"] = txtTaiKhoan.Text;
                    update["MatKhau"] = txtMatKhau.Text;
                    update["img"] = "..\\..\\img\\"+file;
                }
            }
            SqlCommandBuilder cmb = new SqlCommandBuilder(daNhanVien);
            daNhanVien.Update(dtNhanVien);
            MessageBox.Show("Bạn đã lưu thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            datagirlDSNV.Enabled = true;
            datagirlDSNV_SelectionChanged(null, null);

            xemThongTin(true);
            //Ẩn các button và hiện button cần
            anButton();
            btnThem.Visible = true;
            btnXoa.Visible = true;
            btnSua.Visible = true;
            btnLoad.Visible = false;
            txtTaiKhoan.Enabled = true;
            txtMaNV.Enabled = true;

            ktkcbo(false);//Enabled các cbo khi lưu xong
            datagirlDSNV.Focus();
        }
        private void datagirlDSNV_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in datagirlDSNV.SelectedRows)
            {
                try
                {
                    Image img = Image.FromFile(row.Cells["img"].Value.ToString());
                    pictureBox1.Image = img;
                }
                catch (Exception)
                {
                    MessageBox.Show("Hình bị lỗi xin cập nhật lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Image img = Image.FromFile("..\\..\\img\\hinhmacdinh.jpg");
                    pictureBox1.Image = img;
                }
                file = row.Cells[12].Value.ToString();
                txtMaNV.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                string gioitinh = row.Cells[2].Value.ToString();
                if (gioitinh == "True")
                {
                    cboGioiTinh.SelectedIndex = 1;
                }
                else
                    cboGioiTinh.SelectedIndex = 0;
                dtpNgaySinh.Text = row.Cells[3].Value.ToString();
                dtpNgayVL.Text = row.Cells[4].Value.ToString();
                cboDanToc.Text = row.Cells[5].Value.ToString();
                txtCMND.Text = row.Cells[6].Value.ToString();
                txtDiaChi.Text = row.Cells[7].Value.ToString();
                txtSDT.Text = row.Cells[8].Value.ToString();
                cboChucVu.Text = row.Cells[9].Value.ToString();
                txtTaiKhoan.Text = row.Cells[10].Value.ToString();
                txtMatKhau.Text = row.Cells[11].Value.ToString();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            clearThongTin();
            datagirlDSNV_SelectionChanged(null, null);
            datagirlDSNV.Enabled = true;
            xemThongTin(true);
            //Ẩn hêt button và hiện button nào cần 
            anButton();
            btnThem.Visible = true;
            btnXoa.Visible = true;
            btnSua.Visible = true;
            btnLoad.Visible = false;
            txtTaiKhoan.Enabled = true;
            txtMatKhau.Enabled = true;
            txtMaNV.Enabled = true;
            ktkcbo(false);
            datagirlDSNV.Focus();
        }
        private void click()
        {
            btnXoa.BackColor = colorDialog1.Color;
            btnSua.BackColor = colorDialog1.Color;
            btnHuy.BackColor = colorDialog1.Color;
        }
        //Chỉ cho xem thông tin không cho sửa
        public void xemThongTin(bool s)
        {
            txtMaNV.ReadOnly = s;
            txtHoTen.ReadOnly = s;
            txtCMND.ReadOnly = s;
            txtDiaChi.ReadOnly = s;
            txtSDT.ReadOnly = s;
            txtMatKhau.ReadOnly = s;
        }

        //Xóa hết thông tin trên bảng thông tin
        public void clearThongTin()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            cboGioiTinh.SelectedIndex = -1;
            dtpNgaySinh.Text = "";
            dtpNgayVL.Text = "";
            cboDanToc.SelectedIndex = -1;
            txtCMND.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            cboChucVu.SelectedIndex = -1;
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
        }
        //Ẩn hết các button
        public void anButton()
        {
            btnThem.Visible = false;
            btnXoa.Visible = false;
            btnSua.Visible = false;
            btnLuu.Visible = false;
            btnHuy.Visible = false;
        }
        //Kiểm tra xem trung mã nhân viên chưa
        bool kiemTraMaNV(string maNV)
        {
            foreach (DataGridViewRow row in datagirlDSNV.Rows)
            {

                if (String.Compare(row.Cells[0].Value.ToString(), (maNV), true) == 0)
                    return true;
            }
            return false;
        }
        //Kiểm tra chứng mình nhân đân 9 hoặc 13 số
        bool kiemtraCMND(string cmnd)
        {
            if (cmnd.Length == 9 || cmnd.Length == 13)
                return true;
            return false;
        }
        //kiểm tra chức vụ

        //Kiểm tra ngày sinh
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
        //Kiểm tra ngày vào làm
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
        //Kiểm tra tuổi
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

        private void btnTim_Click(object sender, EventArgs e)
        {
            string s = "SELECT * From NhanVien WHERE MaNhanVien='" + txtTimKiem.Text + "'";
            string s2 = "SELECT * From NhanVien ";
            if (txtTimKiem.Text == "")
                loadDSNV(s2);
            else
            {

                loadDSNV(s);
                if (datagirlDSNV.RowCount < 1)
                    MessageBox.Show("Không tìm thấy nhân viên có Mã Số là : " + txtTimKiem.Text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnTimChucVu_Click(object sender, EventArgs e)
        {
            if (cboTimKiemChucVu.SelectedIndex == 0)
            {
                string s = "SELECT * From NhanVien ";
                loadDSNV(s);
            }
            else
            {
                string s = "SELECT * From NhanVien WHERE ChucVu=N'" + cboTimKiemChucVu.SelectedItem + "'";
                loadDSNV(s);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string s = "SELECT * From NhanVien WHERE MaNhanVien like'" + txtTimKiem.Text + "%'";
            loadDSNV(s);
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
            txtTaiKhoan.Text = txtMaNV.Text;
        }

        private void txtCMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                ErrorProvider.SetError(txtCMND, "Số điện thoại không thể nhập chữ");
            }
            else
            {
                ErrorProvider.Clear();
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                ErrorProvider.SetError(txtSDT, "Số điện thoại không thể nhập chữ");
            }
            else
            {
                ErrorProvider.Clear();
            }
        }
    }
}
