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
    public partial class UserControlPhongChieu : UserControl
    {
        KetNoiCSDL kn;
        DataSet ds;
        SqlDataAdapter da;
        public UserControlPhongChieu()
        {
            InitializeComponent();
            //connect
            kn = new KetNoiCSDL();
            kn.ketNoi();
            String strQuery = "SELECT * FROM PHONGCHIEU";

            //khoi tao dataset
            ds = new DataSet();

            //new
            da = new SqlDataAdapter(strQuery, kn.Conn);

            da.Fill(ds, "PhongChieu");

            cbbMaPC.DataSource = ds.Tables["PhongChieu"];
            cbbMaPC.ValueMember = "MaPC";
        }

        private void UserControlPhongChieu_Load(object sender, EventArgs e)
        {
            List<String> str = new  List<string>{"2D","3D","4D"};
            cbbManHinh.DataSource = str;
            cbbManHinh.SelectedIndex = 0;
            cbbMaPC.SelectedIndex = 0;
        }

        private void btnRap1_Click(object sender, EventArgs e)
        {
            
            Control ctr = (Control)sender;
            foreach(DataRow item in ds.Tables["PhongChieu"].Rows)
            {
                if (ctr.Text.Trim() == item["TenPhong"].ToString())
                {
                    cbbMaPC.Text = item["MaPC"].ToString();
                    txtTenPC.Text = item["TenPhong"].ToString();
                    txtSoDay.Text = item["SoDay"].ToString();
                    txtSoGheDay.Text = item["SoLuong1Day"].ToString();
                    cbbManHinh.Text = item["ManHinh"].ToString();
                }
                
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtTenPC.Text.Length <= 0)
            {
                MessageBox.Show("Vui lòng Chọn Phòng Chiếu");
                txtTenPC.Focus();
                return;
            }
            if (txtSoDay.Text.Length <= 0)
            {
                MessageBox.Show("Vui lòng nhập Số Dãy");
                txtSoDay.Focus();
                return;
            }
            if (int.Parse(txtSoDay.Text) > 6)
            {
                    MessageBox.Show("Số dãy không được quá 6");
                    txtSoDay.Focus();
                    return;                
            }

            if (txtSoGheDay.Text.Length <= 0)
            {
                MessageBox.Show("Vui lòng nhập Số Ghế Trên Dãy");
                txtSoGheDay.Focus();
                return;
            }
            if (int.Parse(txtSoGheDay.Text) > 10)
            {
                MessageBox.Show("1 dãy không được nhiếu hơn 10 ghế");
                txtSoGheDay.Focus();
                return;
            }
            
            bool flag = false;
            foreach (DataRow item in ds.Tables["PhongChieu"].Rows)
            {
                if (cbbMaPC.Text == item["MaPC"].ToString())
                {
                    item["SoDay"] = txtSoDay.Text;
                    item["SoLuong1Day"] = txtSoGheDay.Text;
                    item["ManHinh"] = cbbManHinh.SelectedValue;
                    flag = true;
                }
            }
            if(flag == true)
                MessageBox.Show("Cập Nhật Thành Công !");
            else
                MessageBox.Show("Cập Nhật Thất Bại !");

            SqlCommandBuilder cB = new SqlCommandBuilder(da);

            da.Update(ds, "PhongChieu");

        }



    }
}
