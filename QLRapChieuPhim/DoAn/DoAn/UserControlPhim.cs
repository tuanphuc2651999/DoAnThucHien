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
    public partial class UserControlPhim : UserControl
    {
        KetNoiCSDL kn = new KetNoiCSDL();
        DataSet ds;
        SqlDataAdapter da;
        DataTable dtPhim;
        SqlDataReader rd;
        SqlCommand cmd;
        ImageList lstviewItemImageList = new ImageList();
        public UserControlPhim()
        {
            InitializeComponent();
            kn.ketNoi();
        }
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
                lstviewItemImageList.ImageSize = new Size(100, 130);
                ListViewItem item = new ListViewItem(new[] { ha });
                item.Text = rd["TenPhim"].ToString();
                lstImg.LargeImageList = lstviewItemImageList;
                item.ImageIndex = lstviewItemImageList.Images.Add(Image.FromFile(ha), Color.Transparent);
                lstImg.Items.Add(item);
            }
            if (kn.Conn.State == ConnectionState.Open)
            {
                kn.Conn.Close();
            }
        }
        private void UserControlPhim_Load(object sender, EventArgs e)
        {
            string s = "select * from phim";
            loadHinh(s);
            loadQuocGia();
            loadTheLoai();
            cboQuocGia.Text = "Tất cả";
            cboTheLoai.Text = "Tất cả";
            cboHienTrang.SelectedIndex = 0;
            //int vt =lstImg.SelectedItems[0].Index;
            // MessageBox.Show(lstImg.SelectedItems[0].Text);
        }
        public void thongTinPhim(string s)
        {
            string sql = "Select * from Phim,TheLoaiPhim,NhaSanXuat WHERE Phim.MaTheLoai=TheLoaiPhim.MaTheLoai AND Phim.MaNSX=NhaSanXuat.MaNSX AND Phim.TenPhim=N'" + s + "'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(sql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                try
                {
                    Image img = Image.FromFile(rd["Img"].ToString());
                    pbPhim.Image = img;

                }
                catch (Exception)
                {
                    Image img = Image.FromFile("..\\..\\img\\10.jpg");
                    pbPhim.Image = img;
                }
                lblTenPhim.Text = (rd["TenPhim"].ToString());
                lblTheLoai.Text = (rd["TenTheLoai"].ToString());
                lblQuocGia.Text = (rd["TenNSX"].ToString());
                lblThoiLuong.Text = (rd["ThoiLuong"].ToString());
                lblNgayCongChieu.Text = (rd["NgayCongChieu"]).ToString().Substring(0, 10);
                lblNoiDung.Text = (rd["NoiDung"].ToString());
                string ht = rd["HienTrang"].ToString();
                if (ht == "True")
                {
                    lblTrangThai.Text = "Đang chiếu";
                    lblTrangThai.ForeColor = Color.Lime;
                }
                else
                {
                    lblTrangThai.Text = "Ngưng chiếu";
                    lblTrangThai.ForeColor = Color.Red;
                }
            }
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }

        private void lstImg_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string s = lstImg.SelectedItems[0].Text;
                thongTinPhim(s);
            }
            catch (Exception)
            {
                            
            }
           
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            int kt = -1;
            string s = "";
            string maTL=cboTheLoai.SelectedValue.ToString();
            string maQG=cboQuocGia.SelectedValue.ToString();
            if (cboHienTrang.Text == "Đang chiếu")
            {
                kt = 1;
            }
            else if (cboHienTrang.Text == "Ngưng chiếu")
            {
                kt = 0;
            }
            if(kt==-1 &&maTL=="0"&&maQG=="0")
            {
                s = "select * from phim";
               loadHinh(s);
                return;
            }
            if (kt == -1)
            {
                s = "select * from phim WHERE MATheLoai=N'" + cboTheLoai.SelectedValue.ToString() + "' AND MaNSX='" + cboQuocGia.SelectedValue.ToString() + "'";
            }
            else
            {              
                if (cboTheLoai.SelectedValue.ToString() == "0" && cboQuocGia.SelectedValue.ToString() == "0")
                {
                    s = " select * from phim WHERE HienTrang='" + kt + "'";
                }
                else if (cboTheLoai.SelectedValue.ToString() == "0")
                {
                    s = "select * from phim WHERE HienTrang='" + kt+ "' AND MaNSX='" + cboQuocGia.SelectedValue.ToString() + "'";
                
                }
                else if (cboQuocGia.SelectedValue.ToString() == "0")
                {
                    s = "select * from phim WHERE HienTrang='" + kt + "'AND MATheLoai=N'" + cboTheLoai.SelectedValue.ToString() + "'";
                }
                else
                {
                    s = "select * from phim WHERE HienTrang='" + kt + "'AND MATheLoai=N'" + cboTheLoai.SelectedValue.ToString() + "' AND MaNSX='" + cboQuocGia.SelectedValue.ToString() + "'";
                }

                }

            loadHinh(s);           
        }

        private void cboHienTrang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHienTrang.Text == "Đang chiếu")
            {
                string s = "select * from phim where hientrang='1'";
                loadHinh(s);
            }
            else if (cboHienTrang.Text == "Ngưng chiếu")
            {
                string s = "select * from phim where hientrang='0'";
                loadHinh(s);
            }
            else
            {
                string s = "select * from phim";
                loadHinh(s);
                return;
            }

        }
    }
}
