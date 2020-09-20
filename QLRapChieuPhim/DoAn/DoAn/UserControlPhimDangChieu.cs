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

    public partial class UserControlPhimDangChieu : UserControl
    {
        Button btn;
        KetNoiCSDL kn = new KetNoiCSDL();
        Image img;
        public string ngayChieu = "";
        public string maPhim = "";
        public string tenphim = "";
        string manv = "";
        DateTime now = DateTime.Now;  
        public UserControlPhimDangChieu(string ngaychieu, string tenphim, string manv)
        {
            InitializeComponent();
            kn.ketNoi();
            loadPhim(ngaychieu, tenphim);
            loadLich(ngaychieu, tenphim);
            this.manv = manv;
            this.ngayChieu = ngaychieu;

            this.tenphim = tenphim;
        }
        //Load ds phim
        public void loadPhim(string ngaychieu, string tenphim)
        {
            string sql = "Select * from Phim,TheLoaiPhim,NhaSanXuat,LichChieu WHERE Phim.MaTheLoai=TheLoaiPhim.MaTheLoai AND Phim.MaNSX=NhaSanXuat.MaNSX AND LichChieu.MaPhim=Phim.MaPhim AND LichChieu.NgayChieu='" + ngaychieu + "' AND Phim.TenPhim=N'" + tenphim + "'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(sql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                try
                {
                    img = Image.FromFile(rd["Img"].ToString());
                    pbPhim.Image = img;

                }
                catch (Exception)
                {
                    img = Image.FromFile("..\\..\\img\\10.jpg");
                    pbPhim.Image = img;
                }
                maPhim = (rd["MaPhim"].ToString());
                lblTenPhim.Text = (rd["TenPhim"].ToString());
                lblTheLoai.Text = (rd["TenTheLoai"].ToString());
                lblQuocGia.Text = (rd["TenNSX"].ToString());
                lblThoiLuong.Text = (rd["ThoiLuong"].ToString());
                //lblNgayCongChieu.Text = (rd["NgayCongChieu"]).ToString().Substring(0, 10);
                lblNoiDung.Text = (rd["NoiDung"].ToString());
                string ht = rd["HienTrang"].ToString();
            }
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }

        //Load Lich
        public void loadLich(string ngaychieu, string tenphim)
        {
            string sql = "select  * From LichChieu,Phim where Phim.MaPhim=LichChieu.MaPhim AND LichChieu.NgayChieu='" + ngaychieu + "' AND Phim.TenPhim=N'" + tenphim + "'";
            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(sql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                DateTimePicker giochieu = new DateTimePicker();
                giochieu.Text = rd["GioBatDau"].ToString();
                DateTime now = DateTime.Now;               
                if (DateTime.Compare(DateTime.Parse(now.ToString()), DateTime.Parse(giochieu.Value.ToString())) < 0)                   
                taoButron(giochieu.Value.ToShortTimeString(), rd["MaLC"].ToString());
            }
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
        }
        private void taoButron(string text, string ten)
        {
            btn = new Button();
            btn.Text = text;
            btn.Name = ten;
            foreach (Control item in flowLayoutPanel1.Controls)
            {
                if (item.Text == btn.Text)
                    return;
            }
            btn.Font = new Font(btn.Font.FontFamily, 10);
            btn.BackColor = Color.Transparent;
            btn.Size = new System.Drawing.Size(100, 46);
            btn.Click += new EventHandler(btnchon_Click);//Thêm các sự kiên click
            flowLayoutPanel1.Controls.Add(btn);
        }
        //Sự kiên btnchon
        private void btnchon_Click(object sender, System.EventArgs e)
        {
            Button ctr = (Button)sender;
            FrmVe Booking = new FrmVe(manv);
            Booking.ngayChieu = ngayChieu;
            Booking.gioChieu = ctr.Text;
            Booking.AnhPhim = pbPhim.Image;
            Booking.MaPhim = maPhim;
            Booking.ShowDialog();
        }

        private void UserControlPhimDangChieu_Load(object sender, EventArgs e)
        {
        }
    }
}
