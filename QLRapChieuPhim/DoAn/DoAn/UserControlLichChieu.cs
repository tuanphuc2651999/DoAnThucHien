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
    public partial class UserControlLichChieu : UserControl
    {
        ImageList lstviewItemImageList = new ImageList();
        KetNoiCSDL kn = new KetNoiCSDL();
        Button btn;
        String ngayChieu = "";
        String manv = "";
        UserControlPhimDangChieu us;
        public UserControlLichChieu()
        {
            InitializeComponent();
            kn.ketNoi();
        }
        public UserControlLichChieu(string manv)
        {
            InitializeComponent();
            kn.ketNoi();
            this.manv=manv;
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
        private void taoCacNgayCoLich()
        {
            string strSql = "select DISTINCT MONTH(NgayChieu) as N'Tháng',DAY(NgayChieu)as N'Ngày',NgayChieu From LichChieu where Month(LichChieu.NgayChieu)>=MONTH(GETDATE()) AND Day(LichChieu.NgayChieu)>=DAY(GETDATE()) AND YEAR(LichChieu.NgayChieu)>=YEAR(GETDATE())";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string ten = rd["Ngày"].ToString() + "/" + rd["Tháng"].ToString();
                ngayChieu = rd["NgayChieu"].ToString();
                taoBT(ten, ngayChieu);
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        //
        private void loadDSPhimChieuTrongNgay(string s)
        {
            string strSql = "select DISTINCT TenPhim From LichChieu,Phim where Phim.MaPhim=LichChieu.MaPhim AND LichChieu.NgayChieu='" + s + "'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                us = new UserControlPhimDangChieu(s, rd["TenPhim"].ToString(),manv);
                flowLayoutPanel1.Controls.Add(us);
                us.ngayChieu = ngayChieu;
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        private void btnchon_Click(object sender, System.EventArgs e)
        {
            foreach (Control item in flowLayoutPanel2.Controls)
            {
                if (item != lblTitle)
                    item.BackColor = Color.Thistle;
            }
            Button ctr = (Button)sender;
            ctr.BackColor = Color.Violet;
            flowLayoutPanel1.Controls.Clear();
            string[] layngay = ctr.Name.Split(' ');     
            ngayChieu=layngay[0];
            loadDSPhimChieuTrongNgay(ctr.Name);
           
        }

        public void taoBT(string text, string ten)
        {
            btn = new Button();
            btn.Text = text;
            btn.Name = ten;
            btn.Font = new Font(btn.Font.FontFamily, 12);
            btn.Size = new System.Drawing.Size(100, 50);
            btn.Click += new EventHandler(btnchon_Click);//Thêm các sự kiên click
            btn.FlatAppearance.BorderSize = 1;
            btn.BackColor = Color.Thistle;
            btn.FlatAppearance.MouseOverBackColor = Color.Plum;
            btn.FlatStyle = FlatStyle.Flat;
            flowLayoutPanel2.Controls.Add(btn);
        }
        private void UserControlDatVeTuAdmin_Load(object sender, EventArgs e)
        {
            taoCacNgayCoLich();
            if (Quyen(manv).ToUpper().Equals("QUẢN LÝ"))
                btnThem.Visible = true;
            foreach (Control item in flowLayoutPanel2.Controls)
            {
                if (item != lblTitle)
                {
                    btnchon_Click(item, null);
                    return;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemLC themlc = new FrmThemLC();
            themlc.ShowDialog();
        }
    }
}
