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
using System.Windows.Forms.DataVisualization.Charting;

namespace DoAn
{
    public partial class UserControlTrangChu : UserControl
    {
        KetNoiCSDL kn = new KetNoiCSDL();
        DataSet ds;
        SqlDataAdapter da;
        public UserControlTrangChu()
        {
            InitializeComponent();
            kn.ketNoi();
             
        }
        public void loadDT()
        {     
            string strSql = "select sum(GiaTien) as N'Doanh thu',MONTH(NgayDatVe) as N'Tháng' from ThongTinVe GROUP BY MONTH(NgayDatVe),YEAR(NgayDatVe)";

            if (kn.Conn.State == ConnectionState.Closed)
                kn.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, kn.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
               
            }
            rd.Close();
            if (kn.Conn.State == ConnectionState.Open)
                kn.Conn.Close();
           
        }
        private void UserControlTrangChu_Load(object sender, EventArgs e)
        {                       
        }
        int i = 0; int a = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 3)
            {
                pictureBox1.Image = Image.FromFile("..\\..\\img\\banner2.jpg");
            }
                
            else if (i == 6)
            {
                pictureBox1.Image = Image.FromFile("..\\..\\img\\banner3.jpg");               
            }
            else if (i == 9)
            {
                pictureBox1.Image = Image.FromFile("..\\..\\img\\banner4.jpg");              
            }
            else if (i == 12)
            {
                pictureBox1.Image = Image.FromFile("..\\..\\img\\banner1.jpg");
                i = 0;
                timer1.Stop();
                timer1.Start();
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
           
        }
    }
}
