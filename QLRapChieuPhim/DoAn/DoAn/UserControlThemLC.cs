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
    public partial class UserControlThemLC : UserControl
    {
        UserControlPhim us = new UserControlPhim();
        KetNoiCSDL kn;
        DataSet ds;
        DataSet ds_DGV;
        DataSet ds_Phim;
        DataTable dt_LichChieu;
        SqlDataAdapter da_PC;
        SqlDataAdapter da_LC;
        SqlDataAdapter da_LichChieu;
        SqlDataAdapter da_TLPhim;
        SqlDataAdapter da_Phim;
        SqlDataAdapter da;
        DataColumn[] key = new DataColumn[1];
        DateTime dateNow = DateTime.Now;
        ImageList lstviewItemImageList = new ImageList();
        SqlCommand cmd;
        SqlDataReader rd;
        public UserControlThemLC()
        {
            InitializeComponent();
            kn = new KetNoiCSDL();
            kn.ketNoi();
            dt_LichChieu = new DataTable("LichChieu");
            String strQuery = "Select * from LichChieu";
            da_LichChieu = new SqlDataAdapter(strQuery, kn.Conn);
            da_LichChieu.Fill(dt_LichChieu);
            key[0] = dt_LichChieu.Columns[0];
            dt_LichChieu.PrimaryKey = key;
            dateTimeStart.CustomFormat = "HH:mm";
            cbbPhongChieu.FormatString = "HH:mm";

        }
        //Load thể loại
        public void loadTheLoai()
        {
            ds = new DataSet();
            string strSql = "select * from TheLoaiPhim";
            da = new SqlDataAdapter(strSql, kn.Conn);
            da.Fill(ds);
            DataRow newrow = ds.Tables[0].NewRow();
            newrow["TenTheLoai"] = "Tất cả";
            newrow["MaTheLoai"] = "0";
            ds.Tables[0].Rows.Add(newrow);
            cboTheLoai.DataSource = ds.Tables[0];
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";
        }
        // load quốc gia
        public void loadQuocGia()
        {
            ds = new DataSet();
            string strSql = "select * from NhaSanXuat";
            da = new SqlDataAdapter(strSql, kn.Conn);
            da.Fill(ds);
            DataRow newrow = ds.Tables[0].NewRow();
            newrow["TenNSX"] = "Tất cả";
            newrow["MaNSX"] = "0";
            ds.Tables[0].Rows.Add(newrow);
            cboQuocGia.DataSource = ds.Tables[0];
            cboQuocGia.DisplayMember = "TenNSX";
            cboQuocGia.ValueMember = "MaNSX";
        }
        public void loadHinh(string s)
        {
            lstImg.Items.Clear();
            if (kn.Conn.State == ConnectionState.Closed)
            {
                kn.Conn.Open();
            }
            cmd = new SqlCommand(s, kn.Conn);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string ha = rd["Img"].ToString();
                //string tenud = rd["TENUD"].ToString();
                lstviewItemImageList.ImageSize = new Size(115, 150);
                ListViewItem item = new ListViewItem(new[] { ha });
                item.Text = rd["TenPhim"].ToString();
                //string path = "..\\..\\Pic\\30ungdung\\" + ha;
                lstImg.LargeImageList = lstviewItemImageList;
                item.ImageIndex = lstviewItemImageList.Images.Add(Image.FromFile(ha), Color.Transparent);
                lstImg.Items.Add(item);
            }
            if (kn.Conn.State == ConnectionState.Open)
            {
                kn.Conn.Close();
            }
        }
        private void Load_DataGridView(string s)
        {
            ds_DGV = new DataSet();
            da_LC = new SqlDataAdapter(s, kn.Conn);
            da_LC.Fill(ds_DGV, "LichChieu_Dgv");
            Dgv_LichChieu.DataSource = ds_DGV.Tables["LichChieu_Dgv"];
            Dgv_LichChieu.ForeColor = Color.Black;
            Dgv_LichChieu.Columns[2].DefaultCellStyle.Format = "HH:mm ";
            Dgv_LichChieu.Columns[3].DefaultCellStyle.Format = "HH:mm ";
            Dgv_LichChieu.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";
            Dgv_LichChieu.AllowUserToAddRows = false;
        }
        private void LoadCbb_PhongChieu()
        {
            ds = new DataSet();
            DataTable PhongChieu = new DataTable("PhongChieu");
            String strQuery = "Select * from PhongChieu";
            da_PC = new SqlDataAdapter(strQuery, kn.Conn);
            da_PC.Fill(PhongChieu);
            ds.Tables.Add(PhongChieu);
            cbbPhongChieu.DataSource = ds.Tables["PhongChieu"];
            cbbPhongChieu.DisplayMember = "TenPhong";
            cbbPhongChieu.ValueMember = "MaPC";
        }
        private void LoadCbb_Phim()
        {
            ds_Phim = new DataSet();
            String strQuery = String.Format("Select * from Phim where hientrang=1");
            da_Phim = new SqlDataAdapter(strQuery, kn.Conn);
            da_Phim.Fill(ds_Phim, "Phim");
            cbbTenPhim.DataSource = ds_Phim.Tables["Phim"];
            cbbTenPhim.DisplayMember = "TenPhim";
            cbbTenPhim.ValueMember = "MaPhim";
        }
        //Frm load
        private void UserControlLichChieu_Load(object sender, EventArgs e)
        {

            LoadCbb_PhongChieu();
            LoadCbb_Phim();
            string s = "select * from phim where HienTrang=1";

            loadHinh(s);
            cbbPhongChieu.SelectedIndex = -1;
            loadQuocGia();
            loadTheLoai();
            cboQuocGia.Text = "Tất cả";
            cboTheLoai.Text = "Tất cả";
            cbbTenPhim.SelectedIndex = -1;

        }
        //Kiểm tra lịch
        private bool CheckMaLichChieu(String ma)
        {
            foreach (DataRow row in dt_LichChieu.Rows)
            {
                if (row["MaLC"].ToString().Trim().ToLower().Equals(ma) == true)
                    return false;
            }
            return true;
        }
        private void click()
        {
            btnThem.BackColor = colorDialog1.Color;
            btnXoa.BackColor = colorDialog1.Color;
            btnSua.BackColor = colorDialog1.Color;
        }

        private bool check_Phong_Time(String maPC, DateTime start, DateTime end, DateTime date)
        {
            foreach (DataRow row in dt_LichChieu.Rows)
            {
                DateTime _start = DateTime.Parse(row["GioBatDau"].ToString());
                DateTime _end = DateTime.Parse(row["GioKetThuc"].ToString());
                DateTime _date = DateTime.Parse(row["NgayChieu"].ToString());
                _date.ToShortDateString();
                if (row["MaPC"].ToString().Trim().ToLower().Equals(maPC) == true)
                {
                    if (_date.Subtract(date).Days == 0)
                    {
                        if (start.Hour == _end.Hour)
                        {
                            if (start.Minute <= _end.Minute)
                            {
                                return false;
                            }
                        }
                        if (start.Hour == _start.Hour)
                        {
                            if (start.Minute >= _start.Minute)
                            {
                                return false;
                            }
                        }
                        else if ((start.Hour == _start.Hour && start.Minute <= _start.Minute) || start.Hour < _start.Hour)
                        {
                            if (end.Hour > _start.Hour)
                            {
                                {
                                    return false;
                                }
                            }
                            if (end.Hour == _start.Hour && end.Minute >= _start.Minute)
                            {
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {

            click();
            btnThem.BackColor = Color.Maroon;
            btnLuu.Visible = true;
            btnThem.Visible = false;
            btnXoa.Visible = false;
            btnSua.Visible = false;
            btnHuy.Visible = true;
            cbbPhongChieu.Enabled = true;
            dateTimeStart.Enabled = true;
            txtMaLC.Enabled = true;
            cbbTenPhim.Enabled = true;
            txtMaLC.Text = "";
            dateTimeStart.Text = "";
            cbbPhongChieu.SelectedIndex = -1;
            cbbTenPhim.SelectedIndex = -1;
            lstImg.Enabled = false;
        }
        //Khiểm tra xem bảng thonong tin vé có chưa
        bool kiemTraKhoa(string s)
        {
            try
            {
                string strSql = "select count(*) from ThongTinVe where MaLC='" + s + "'";
                if (kn.Conn.State == ConnectionState.Closed)
                    kn.Conn.Open();
                SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
                int count = (int)cmd.ExecuteScalar();
                if (kn.Conn.State == ConnectionState.Open)
                    kn.Conn.Close();
                if (count >= 1)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            click();
            btnXoa.BackColor = Color.Maroon;

            if (Dgv_LichChieu.SelectedRows.Count > 0)
            {

                if (MessageBox.Show("Bạn có muốn xóa không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    string s = Dgv_LichChieu.CurrentRow.Cells[5].Value.ToString();
                    int index = Dgv_LichChieu.CurrentCell.RowIndex;
                    DataRow rowIndex = ds_DGV.Tables["LichChieu_Dgv"].Rows[index];
                    if (kiemTraKhoa(rowIndex["MaLC"].ToString()))
                    {
                        MessageBox.Show("Bảng thông tin có chưa lịch phim này không thể xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        DataRow dr = dt_LichChieu.Rows.Find(rowIndex["MaLC"].ToString());
                        if (dr != null)
                            dr.Delete();
                        SqlCommandBuilder cB = new SqlCommandBuilder(da_LichChieu);
                        da_LichChieu.Update(dt_LichChieu);
                        String strQuery = "SELECT MaLC,TenPhong,GioBatDau,GioKetThuc,NgayChieu,TenPhim FROM LichChieu l,Phim p, TheLoaiPhim t, PhongChieu pc WHERE l.MaPhim = p.MaPhim and p.MaTheLoai = t.MaTheLoai and pc.MaPC = l.MaPC and p.HienTrang=1 and p.TenPhim=N'" + s + "'";
                        Load_DataGridView(strQuery);
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dòng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            click();
            btnSua.BackColor = Color.Maroon;
            if (Dgv_LichChieu.SelectedRows.Count > 0)
            {
                txtMaLC.Enabled = false;
                btnThem.Visible = false;
                btnXoa.Visible = false;
                btnLuu.Visible = true;
                btnHuy.Visible = true;
                btnSua.Visible = false;
                cbbTenPhim.Enabled = true;
                cbbPhongChieu.Enabled = true;
                dateTimeStart.Enabled = true;

            }
            else
            {
                MessageBox.Show("Chưa chọn dữ liệu để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void lstImg_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string s = lstImg.SelectedItems[0].Text;
                String strQuery = "SELECT MaLC,TenPhong,GioBatDau,GioKetThuc,NgayChieu,TenPhim FROM LichChieu l,Phim p, TheLoaiPhim t, PhongChieu pc WHERE l.MaPhim = p.MaPhim and p.MaTheLoai = t.MaTheLoai and pc.MaPC = l.MaPC and Day(l.NgayChieu)>=DAY(GETDATE()) and Month(l.NgayChieu)>=Month(GETDATE()) and Year(l.NgayChieu)>=Year(GETDATE()) AND p.TenPhim=N'" + s + "'";
                Load_DataGridView(strQuery);
                if (btnThem.Visible == false)
                    cbbTenPhim.Text = s;
                if (Dgv_LichChieu.Rows.Count > 0)
                    Dgv_LichChieu_CellClick(null, null);
                else
                {
                    txtMaLC.Text = "";
                    cbbPhongChieu.SelectedIndex = -1;
                    cbbTenPhim.SelectedIndex = -1;
                    dateTimeStart.Text = "";
                }
            }
            catch (Exception)
            {


            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {           
            string tenpc = cbbPhongChieu.Text;
            if (cbbPhongChieu.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn chưa chọn phòng chiếu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbbPhongChieu.Focus();
                return;
            }
            if (cbbTenPhim.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng Chọn Phim", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dateTimeStart.Value.Hour >= 22 || dateTimeStart.Value.Hour < 6)
            {
                MessageBox.Show("Không thể đặt xuất chiếu từ 22h --> 6h !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtMaLC.Text == "")//Thêm
            {
                if (dateTimeStart.Value < dateNow)
                {
                    MessageBox.Show("Ngày Đặt Lịch Phải Từ Hôm Nay Trở Về Sau", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string giochieu=dateTimeStart.Value.ToShortDateString() + " " + dateTimeStart.Text;
                DataRow newRow = dt_LichChieu.NewRow();
                newRow["GioBatDau"] = giochieu;
                foreach (DataRow item in ds_Phim.Tables["Phim"].Rows)
                {
                    if (item["MaPhim"].ToString().Trim().Equals(cbbTenPhim.SelectedValue) == true)
                    {
                        newRow["GioKetThuc"] = dateTimeStart.Value.Add(TimeSpan.Parse((item["ThoiLuong"].ToString())));
                    }
                }


                if (check_Phong_Time(cbbPhongChieu.SelectedValue.ToString().ToLower().Trim(), dateTimeStart.Value,
                                DateTime.Parse(newRow["GioKetThuc"].ToString()), dateTimeStart.Value.Date) == false)
                {
                    MessageBox.Show("Phòng Này Đang Có Xuất Chiếu Trong Thời Gian Này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }                
                newRow["NgayChieu"] = dateTimeStart.Value.Date;
                newRow["MaPhim"] = cbbTenPhim.SelectedValue;
                newRow["MaPC"] = cbbPhongChieu.SelectedValue.ToString();
                newRow["MaLC"] = layMaLC();
                dt_LichChieu.Rows.Add(newRow);
                SqlCommandBuilder cB = new SqlCommandBuilder(da_LichChieu);
                da_LichChieu.Update(dt_LichChieu);
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtMaLC.Enabled == false && txtMaLC.Text != "") //sửa
            {
                DataRow dr = dt_LichChieu.Rows.Find(txtMaLC.Text);
                if (dr != null)
                {
                    dr["GioBatDau"] = dateTimeStart.Value;
                    foreach (DataRow item in ds_Phim.Tables["Phim"].Rows)
                    {
                        if (item["MaPhim"].ToString().Trim().Equals(cbbTenPhim.SelectedValue) == true)
                        {
                            dr["GioKetThuc"] = dateTimeStart.Value.Add(TimeSpan.Parse((item["ThoiLuong"].ToString())));
                        }
                    }
                    if (check_Phong_Time(cbbPhongChieu.SelectedValue.ToString().ToLower().Trim(), dateTimeStart.Value,
                                DateTime.Parse(dr["GioKetThuc"].ToString()), dateTimeStart.Value.Date) == false)
                    {
                        MessageBox.Show("Phòng Này Đang Có Xuất Chiếu Trong Thời Gian Này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    dr["NgayChieu"] = dateTimeStart.Value;
                    dr["MaPhim"] = cbbTenPhim.SelectedValue;
                    dr["MaPC"] = cbbPhongChieu.SelectedValue;
                    SqlCommandBuilder cB = new SqlCommandBuilder(da_LichChieu);
                    da_LichChieu.Update(dt_LichChieu);

                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            ds_DGV.Tables["LichChieu_Dgv"].Clear();
            btnThem.Visible = true;
            btnXoa.Visible = true;
            btnSua.Visible = true;
            btnLuu.Visible = false;
            btnHuy.Visible = false;
            txtMaLC.Enabled = false;
            cbbPhongChieu.Enabled = false;
            cbbTenPhim.Enabled = false;
            dateTimeStart.Enabled = false;
            lstImg.Enabled = true;
            txtMaLC.Text = "";
            cbbPhongChieu.SelectedIndex = -1;
            cbbTenPhim.SelectedIndex = -1;
            ds_DGV.Tables["LichChieu_Dgv"].Clear();
            DateTime da = DateTime.Now;
            String strQuery = "SELECT MaLC,TenPhong,GioBatDau,GioKetThuc,NgayChieu,TenPhim FROM LichChieu l,Phim p, TheLoaiPhim t, PhongChieu pc WHERE l.MaPhim = p.MaPhim and p.MaTheLoai = t.MaTheLoai and pc.MaPC = l.MaPC and p.HienTrang=1 and tenphong=N'" + tenpc + "' AND NgayChieu>='" + da.ToShortDateString() + "'";
            Load_DataGridView(strQuery);
            Dgv_LichChieu.DataSource = ds_DGV.Tables["LichChieu_Dgv"];
        }
        private void cbbPhongChieu_SelectedValueChanged(object sender, EventArgs e)
        {
            DateTime da = DateTime.Now;
            if (btnThem.Visible == false)
            {
                String strQuery = "SELECT MaLC,TenPhong,GioBatDau,GioKetThuc,NgayChieu,TenPhim FROM LichChieu l,Phim p, TheLoaiPhim t, PhongChieu pc WHERE l.MaPhim = p.MaPhim and p.MaTheLoai = t.MaTheLoai and pc.MaPC = l.MaPC and p.HienTrang=1 and pc.TenPhong=N'" + cbbPhongChieu.Text + "' AND NgayChieu>='"+da.ToShortDateString()+"'";
                Load_DataGridView(strQuery);
            }

        }
        private void btnTim_Click(object sender, EventArgs e)
        {
            string s = "";
            if (cboTheLoai.Text == "Tất cả" && cboQuocGia.Text == "Tất cả")
            {
                s = "select * from phim where HienTrang='1'";

            }
            else if (cboTheLoai.Text == "Tất cả" && cboQuocGia.Text != "Tất cả")
            {
                s = "select * from phim where HienTrang='1' AND MaNSX='" + cboQuocGia.SelectedValue.ToString() + "'";
            }
            else if (cboTheLoai.Text != "Tất cả" && cboQuocGia.Text == "Tất cả")
            {
                s = "select * from phim where HienTrang='1' AND MaTheLoai='" + cboTheLoai.SelectedValue.ToString() + "'";
            }
            else
                if (cboTheLoai.Text != "Tất cả" && cboQuocGia.Text != "Tất cả")
                    s = "select * from phim where HienTrang='1' AND MaTheLoai='" + cboTheLoai.SelectedValue.ToString() + "'AND MaNSX='" + cboQuocGia.SelectedValue.ToString() + "'";
            loadHinh(s);

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnHuy.Visible = false;
            btnLuu.Visible = false;
            btnThem.Visible = true;
            btnXoa.Visible = true;
            btnSua.Visible = true;
            cbbPhongChieu.Enabled = false;
            dateTimeStart.Enabled = false;
            txtMaLC.Text = "";
            txtMaLC.Enabled = false;
            cbbPhongChieu.SelectedIndex = -1;
            cbbTenPhim.SelectedIndex = -1;
            cbbTenPhim.Enabled = false;
            Dgv_LichChieu_CellClick(null, null);
            lstImg.Enabled = true;
        }

        private void Dgv_LichChieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Dgv_LichChieu.SelectedRows.Count > 0)
            {
                int index = Dgv_LichChieu.CurrentCell.RowIndex;
                DataRow rowIndex = ds_DGV.Tables["LichChieu_Dgv"].Rows[index];
                txtMaLC.Text = rowIndex["MaLC"].ToString();
                cbbPhongChieu.Text = rowIndex["TenPhong"].ToString();
                cbbTenPhim.Text = rowIndex["TenPhim"].ToString();
                dateTimeStart.Value = DateTime.Parse(rowIndex["GioBatDau"].ToString());
            }
        }





        //Lấy mã tự động 
        private int layMaLC()
        {
            int ma = 0;
            string strSql = "SELECT MAX(MALC) as 'MALC' FROM LichChieu ";
            try
            {
                if (kn.Conn.State == ConnectionState.Closed)
                    kn.Conn.Open();
                SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    ma = int.Parse(rd["MALC"].ToString());
                }
                rd.Close();
                if (kn.Conn.State == ConnectionState.Open)
                    kn.Conn.Close();
                return ma + 1;
            }
            catch (Exception)
            {
                if (kn.Conn.State == ConnectionState.Open)
                    kn.Conn.Close();
                return ma;
            }
        }
    }
}
