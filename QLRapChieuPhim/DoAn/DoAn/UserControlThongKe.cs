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
using System.Globalization;

namespace DoAn
{
    public partial class UserControlThongKe : UserControl
    {
        KetNoiCSDL kn;
        DataTable tbl_DSVe,dtbDT;
        DataSet ds,ds_DT;   
        SqlDataAdapter daDT;
        SqlDataAdapter da_DSVe;
        public UserControlThongKe()
        {
            InitializeComponent();
            kn = new KetNoiCSDL();
            kn.ketNoi();
        }
        private int demSoNhanVien()
        {
            int dem = 0;
            string strSql = "select * from NhanVien";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dem++;
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            return dem;
        }
        private int demSoVe()
        {
            int dem = 0;
            string strSql = "select * from ThongTinVe";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dem++;
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            return dem;
        }
        private int demSoPhim()
        {
            int dem = 0;
            string strSql = "select * from Phim";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dem++;
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            return dem;
        }
        private string doanhthu()
        {
            string s = "";
            string strSql = "select sum(Gia)as 'DoanhThu' from ThongTinVe,Gia Where Gia.ID=ThongTinVe.IdGia AND YEAR(NgayDatVe)=Year(GETDATE())";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                s = String.Format("{0:0,0}", double.Parse(rd["DoanhThu"].ToString()));
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
            return s;
        }
        private void UserControlDoanhThu_Load(object sender, EventArgs e)
        {
            string strDoanhThu = "select TenPhim as N'Tên Phim' ,Sum(Gia) N'Doanh thu',YEAR(NgayDatVe)as N'Năm'  FROM Phim,LichChieu,PhongChieu,ThongTinVe,Gia WHERE Gia.ID=ThongTinVe.IdGia AND Phim.MaPhim=LichChieu.MaPhim AND LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC Group By TenPhim,YEAR(NgayDatVe)";
            lblDT.Text = doanhthu();
            lblSoNhanVien.Text = demSoNhanVien() + "";
            lblSoVe.Text = demSoVe() + "";
            lblSoPhim.Text = demSoPhim() + "";                   
            topPhim();
            topNam();
            loadDT(strDoanhThu);
            txtDoanhThu.Text = String.Format("{0:0,0}",tongDoanhThu());
            loadDLDoanhThuThang();
        }
        private void topPhim()
        {
            string strSql = "select Top 1 TenPhim,Sum(Gia)as TongTien  FROM Phim,LichChieu,PhongChieu,ThongTinVe,Gia WHERE Phim.MaPhim=LichChieu.MaPhim AND LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC AND Gia.ID=ThongTinVe.IdGia Group By TenPhim ORDER BY TongTien DESC";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {

                txtTopPhim.Text = rd["TenPhim"].ToString();
                txtTopDTPhim.Text = String.Format("{0:0,0}", double.Parse(rd["TongTien"].ToString()));
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        private void topNam()
        {
            string strSql = "select top 1  YEAR(NgayDatVe) as nam,sum(Gia) as tong from ThongTinVe,Gia Where Gia.ID=ThongTinVe.IdGia GROUP BY YEAR(NgayDatVe) ORDER BY Sum(Gia) DESC";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                txtTopNam.Text = rd["nam"].ToString();
                txtTopDTNam.Text = String.Format("{0:0,0}", double.Parse(rd["tong"].ToString()));
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        //Load danh sachs
        public void loadDT(string s)
        {
            ds = new DataSet();
            tbl_DSVe = new DataTable();
            DataColumn[] key = new DataColumn[1];
            da_DSVe = new SqlDataAdapter(s, kn.Conn);
            da_DSVe.Fill(tbl_DSVe);
            dgvDSDT.DataSource = tbl_DSVe;
            key[0] = tbl_DSVe.Columns[0];
        }
        private double tongDoanhThu()
        {
            double kq = 0;
            for (int i = 0; i < dgvDSDT.Rows.Count; i++)
            {
                kq = kq + double.Parse(dgvDSDT.Rows[i].Cells[1].Value.ToString());
            }
            return kq;
        }
        private void cboTKDTPhim_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //if (cboTimKiemPhim.SelectedIndex != -1)
            //{
            //    string strSql = "select Sum(GiaTien) as DoanhThu FROM Phim,LichChieu,PhongChieu,ThongTinVe WHERE Phim.MaPhim=LichChieu.MaPhim AND LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC AND Phim.MaPhim='" + cboTimKiemPhim.SelectedValue + "'";
            //    if (kn.Conn.State == ConnectionState.Closed)
            //        kn.Conn.Open();
            //    SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            //    SqlDataReader rd = cmd.ExecuteReader();
            //    while (rd.Read())
            //    {
            //        if (rd["DoanhThu"].ToString() == "")
            //            txtDoanhThu.Text = "0";
            //        else
            //            txtDoanhThu.Text = String.Format("{0:0,0}", double.Parse(rd["DoanhThu"].ToString()));
            //    }
            //    rd.Close();
            //    if (kn.Conn.State == ConnectionState.Open)
            //        kn.Conn.Close();
            //}
        }
        private void btnTimTheoNam_Click(object sender, EventArgs e)
        {
            string s = "select TenPhim as N'Tên Phim' ,Sum(Gia) N'Doanh thu',YEAR(NgayDatVe)as N'Năm'  FROM Phim,LichChieu,PhongChieu,ThongTinVe,Gia WHERE Phim.MaPhim=LichChieu.MaPhim AND LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC AND Gia.ID=ThongTinVe.IdGia AND YEAR(NgayDatVe)='" + txtTKNam.Text + "'Group By TenPhim,YEAR(NgayDatVe)";
            loadDT(s);
        }
        //vẽ biểu đồ
        public void loadDLDoanhThuThang()
        {
            string s= "select Sum(Gia) N'Doanh thu'  ,Month(NgayDatVe)as N'Tháng'"
                + " FROM Phim,LichChieu,PhongChieu,ThongTinVe,Gia WHERE Phim.MaPhim=LichChieu.MaPhim AND Gia.ID=ThongTinVe.IdGia AND "
            + "LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC "
            + "AND Year(ThongTinVe.NgayDatVe)=YEAR(GETDATE()) Group By Month(NgayDatVe) ";
            ds_DT = new DataSet();
            dtbDT = new DataTable();
            DataColumn[] key = new DataColumn[1];
            daDT = new SqlDataAdapter(s, kn.Conn);
            daDT.Fill(dtbDT);
            chartDoanhThu.DataSource = dtbDT;
            chartDoanhThu.Series["Doanh thu"].XValueMember = "Tháng";
            chartDoanhThu.Series["Doanh thu"].YValueMembers = "Doanh thu";
        }

        private void cboTimKiemPhim_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtTKTenPhim_KeyPress(object sender, KeyPressEventArgs e)
        {
            string s = "select TenPhim as N'Tên Phim' ,Sum(Gia) N'Doanh thu',YEAR(NgayDatVe)as N'Năm'  FROM Phim,LichChieu,PhongChieu,ThongTinVe,Gia WHERE "
            + " Phim.MaPhim=LichChieu.MaPhim AND Gia.ID=ThongTinVe.IdGia AND LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC=ThongTinVe.MaLC AND PHIM.TenPHIM Like N'%" + txtTKTenPhim.Text + "%' "
            +" Group By TenPhim,YEAR(NgayDatVe)";
            loadDT(s);
            txtDoanhThu.Text = String.Format("{0:0,0}", tongDoanhThu());
        }
    }
}
