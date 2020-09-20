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
using System.Data;

namespace DoAn
{
    public partial class UserControlDatVe : UserControl
    {
        KetNoiCSDL c;
        DataTable dt;
        SqlDataAdapter da;
        DataColumn[] key = new DataColumn[1];
        List<Button> lstButton;
        List<int> lstMaVe;
        Int32 maKH;
        public UserControlDatVe()
        {
            InitializeComponent();
            c = new KetNoiCSDL();
            lstButton = new List<Button>();
            lstMaVe = new List<int>();
        }

        private void LoadPhim()
        {
            da = new SqlDataAdapter("select * from Phim where DATEDIFF(DAY,ngayCongChieu,GETDATE()) <= 7", c.conn);
            dt = new DataTable();
            da.Fill(dt);
            txtGioKetThuc.Enabled = false;
            cbBoxNgayChieu.Enabled = false;
            cbBoxGioChieu.Enabled = false;
            if (dt.Rows.Count != 0)
            {
                cbBoxTenPhim.DisplayMember = dt.Columns[1].ToString();
                cbBoxTenPhim.ValueMember = dt.Columns[0].ToString();
                cbBoxTenPhim.DataSource = dt;
            }
            else
            {
                cbBoxTenPhim.Enabled = false;
                cbBoxPhongChieu.Enabled = false;
                cbBoxNgayChieu.Enabled = false;
                cbBoxGioChieu.Enabled = false;
                btnDatVe.Enabled = false;
                MessageBox.Show("Không có phim nào để chiếu");
            }

        }

        private void LoadPhongChieu()
        {
            string strSelect = "select distinct PhongChieu.MaPC, PhongChieu.TenPhong from PhongChieu, LichChieu where PhongChieu.MaPC = LichChieu.MaPC and MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and DATEDIFF(DAY,GETDATE(),NgayChieu) >= 0";
            da = new SqlDataAdapter(strSelect, c.conn);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                cbBoxPhongChieu.Enabled = true;
                cbBoxPhongChieu.DisplayMember = dt.Columns[1].ToString();
                cbBoxPhongChieu.ValueMember = dt.Columns[0].ToString();
                cbBoxPhongChieu.DataSource = dt;
            }
            else
            {
                cbBoxPhongChieu.DataSource = null;
                cbBoxPhongChieu.Items.Clear();
                cbBoxPhongChieu.Enabled = false;
            }

        }

        private void LoadNgayChieu()
        {
            if (cbBoxPhongChieu.DataSource != null)
            {
                string strSelect = "select distinct NgayChieu from LichChieu where MaPC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "' and MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and DATEDIFF(DAY,GETDATE(),NgayChieu) >= 0";
                da = new SqlDataAdapter(strSelect, c.conn);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    cbBoxNgayChieu.Enabled = true;
                    cbBoxNgayChieu.DisplayMember = dt.Columns[0].ToString();
                    cbBoxNgayChieu.ValueMember = dt.Columns[0].ToString();
                    cbBoxNgayChieu.DataSource = dt;
                    btnDatVe.Enabled = true;
                }
                else
                {
                    cbBoxNgayChieu.Enabled = false;
                    cbBoxNgayChieu.DataSource = null;
                    cbBoxNgayChieu.Items.Clear();

                    cbBoxGioChieu.Enabled = false;
                    cbBoxGioChieu.DataSource = null;
                    cbBoxGioChieu.Items.Clear();

                    btnDatVe.Enabled = false;
                }
            }
            else
            {
                cbBoxNgayChieu.Enabled = false;
                cbBoxNgayChieu.DataSource = null;
                cbBoxNgayChieu.Items.Clear();
                cbBoxGioChieu.Enabled = false;
                cbBoxGioChieu.DataSource = null;
                cbBoxGioChieu.Items.Clear();

                btnDatVe.Enabled = false;
            }
        }

        private void loadGioChieu()
        {
            if (cbBoxNgayChieu.DataSource != null)
            {
                string strSelect = "select GioBatDau from Phim, LichChieu where Phim.MaPhim = LichChieu.MaPhim and Phim.MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and MaPC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "' and NgayChieu = '" + cbBoxNgayChieu.SelectedValue.ToString() + "'";
                da = new SqlDataAdapter(strSelect, c.conn);
                dt = new DataTable();
                da.Fill(dt);
                cbBoxGioChieu.Enabled = true;
                cbBoxGioChieu.FormatString = "HH:mm:tt";
                cbBoxGioChieu.DisplayMember = dt.Columns[0].ToString();
                cbBoxGioChieu.ValueMember = dt.Columns[0].ToString();
                cbBoxGioChieu.DataSource = dt;
            }
        }


        private void LoadGhe()
        {
            int gheTrong = 0;
            int gheDaDat = 0;
            lblGheTrong.Text = "Ghế Trống";
            lblDaDat.Text = "Đã Đặt";
            da = new SqlDataAdapter("select * from PhongChieu where MaPC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "'", c.conn);
            dt = new DataTable();
            da.Fill(dt);
            DataRow dr = dt.Rows[0];
            int soDay = int.Parse(dr["SoDay"].ToString());
            int vtGhe = int.Parse(dr["SoLuong1Day"].ToString());
            rdNormal.Checked = true;
            lstButton.Clear();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 1; j <= 10; j++)
                {

                    int so = 65 + i;
                    char tenGhe = Convert.ToChar(so);
                    string buttonName = "btn" + tenGhe.ToString() + j + "";
                    Button btn = this.Controls.Find(buttonName, true).FirstOrDefault() as Button;
                    if (i < soDay && j <= vtGhe)
                    {
                        btn.Visible = true;
                        btn.Enabled = true;
                        if (checkGhe(btn))
                        {
                            btn.BackColor = System.Drawing.Color.Red;
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderColor = Color.Black;
                            btn.FlatAppearance.BorderSize = 1;
                            gheDaDat++;
                            

                        }
                        else //if (btn.BackColor != Color.Yellow && btn.BackColor != Color.LightYellow)
                        {
                            btn.BackColor = System.Drawing.Color.Green;
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderColor = Color.White;
                            btn.FlatAppearance.BorderSize = 1;
                            gheTrong++;
                        }
                        btn.Click -= new EventHandler(btnChon_Click);
                        btn.Click += new EventHandler(btnChon_Click);
                    }
                    else if (btn != null)
                    {
                        btn.Visible = false;
                    }

                }
            }
            lblDaDat.Text = lblDaDat.Text + "  ("+ gheDaDat + ")";
            lblGheTrong.Text = lblGheTrong.Text + "  (" +gheTrong+ ")";
        }

        private Boolean checkGhe(Button btn)
        {
            if (String.Compare(cbBoxNgayChieu.SelectedValue.ToString(), "System.Data.DataRowView") == 0)
                return false;
            string strSelect = "select * from LichChieu, ThongTinVe where LichChieu.MaLC = ThongTinVe.MaLC and  MaPC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "' and MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and NgayChieu = '" + cbBoxNgayChieu.SelectedValue.ToString() + "'";
            da = new SqlDataAdapter(strSelect, c.conn);
            dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow r in dt.Rows)
            {
                Button bt = new Button();
                bt.Text = r["Day"].ToString() + r["ViTriDat"].ToString();

                if (String.Compare(bt.Text, btn.Text, true) == 0)
                    return true;
            }
            return false;
        }




        private void btnChon_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackColor != Color.Red)
            {
                if (btn.BackColor == Color.Yellow && rdStudent.Checked == true)
                {
                    lstButton.Remove(btn);
                    btn.BackColor = Color.LightYellow;
                    lstButton.Add(btn);

                }
                else if (btn.BackColor == Color.LightYellow && rdNormal.Checked == true)
                {
                    lstButton.Remove(btn);
                    btn.BackColor = Color.Yellow;
                    lstButton.Add(btn);
                }
                else if (btn.BackColor == Color.LightYellow || btn.BackColor == Color.Yellow)
                {
                    btn.BackColor = Color.Green;
                    lstButton.Remove(btn);
                }
                else
                {
                    if (rdStudent.Checked == true)
                        btn.BackColor = Color.LightYellow;
                    if (rdNormal.Checked == true)
                        btn.BackColor = Color.Yellow;
                    lstButton.Add(btn);
                }
            }


        }

        

        private void datGhe()
        {

            string strSelect = "select MaLC from Phim, LichChieu where Phim.MaPhim = LichChieu.MaPhim and Phim.MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and MaPC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "' and NgayChieu = '" + cbBoxNgayChieu.SelectedValue.ToString() + "'";
            dt = new DataTable();
            DataRow dr;
            da = new SqlDataAdapter(strSelect, c.conn);
            da.Fill(dt);
            dr = dt.Rows[0];
            string maLC = dr[0].ToString();
            if (c.conn != null && c.conn.State != ConnectionState.Open)
                c.conn.Open();
            string strSelect1 = "select count(*) from ThongTinVe";
            SqlCommand cmd = new SqlCommand(strSelect1, c.conn);
            Int32 maVe = Convert.ToInt32(cmd.ExecuteScalar());            
            string strSelect2 = "select count(*) from KhachHang";
            cmd = new SqlCommand(strSelect2, c.conn);
            maKH = Convert.ToInt32(cmd.ExecuteScalar());
            foreach (Button b in lstButton)
            {
                char[] charac = b.Text.ToCharArray();
                string Day = charac[0].ToString();
                string ViTriDat = "";
                if (charac.Length > 2)
                    ViTriDat = charac[1].ToString() + charac[2].ToString();
                else
                    ViTriDat = charac[1].ToString();
                maVe++;
                maKH++;
                try
                {
                    double tienVe = 99000;
                    if (b.BackColor == Color.LightYellow)
                        tienVe = 45000;
                    b.BackColor = Color.Red;
                    string strInsert = "insert into ThongTinVe values('" + maVe.ToString() + "','" + Day + "','" + ViTriDat + "'," + tienVe.ToString() + "," + maLC + ",NULL,'" + DateTime.Now + "')";

                    lstMaVe.Add(maVe);
                    SqlCommand cmd1 = new SqlCommand(strInsert, c.conn);
                    cmd1.ExecuteNonQuery();

                    string strInsertKH = "insert into KhachHang values(" + maKH + ",N'" + txtTenKhachhang.Text.Trim() + "'," + maVe.ToString() + ")";
                    SqlCommand cmd2 = new SqlCommand(strInsertKH, c.conn);
                    cmd2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    
                    //MessageBox.Show("Lỗi ở hàm đặt ghế");
                }
            }
            c.conn.Close();
        }


        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTimVe_Click_1(object sender, EventArgs e)
        {
            FormTimVe FTimVe = new FormTimVe();
            FTimVe.Show();
        }

        private void btnInVe_Click(object sender, EventArgs e)
        {
            if (lstMaVe.Count != 0)
            {
                FormInVe InVe = new FormInVe();
                InVe.lstMaVe = new List<int>(lstMaVe);
                InVe.maKH = maKH;
                InVe.Show();
                lstMaVe.Clear();
            }
            else
                MessageBox.Show("Vui lòng đặt vé trước khi in!!!");
        }

        private void cbBoxNgayChieu_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbBoxNgayChieu.SelectedValue != null)
            {
                tblpGhe.Enabled = true;
                LoadGhe();
                loadGioChieu();
            }
            else
            {
                tblpGhe.Enabled = false;
            }
        }

        private void cbBoxGioChieu_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbBoxGioChieu.DataSource != null)
            {
                string strSelect = "select GioKetThuc from Phim, LichChieu where Phim.MaPhim = LichChieu.MaPhim and Phim.MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and NgayChieu = '" + cbBoxNgayChieu.SelectedValue.ToString() + "' and GioBatDau = '" + cbBoxGioChieu.SelectedValue.ToString() + "'";
                da = new SqlDataAdapter(strSelect, c.conn);
                dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.Rows[0];
                txtGioKetThuc.Text = String.Format("{0:t}", dr["GioKetThuc"]);
            }
            else
            {
                txtGioKetThuc.Text = "";
            }
        }


        private void cbBoxTenPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPhongChieu();
        }


        private void UserControl1_Load(object sender, EventArgs e)
        {
            LoadPhim();
        }

        private void cbBoxPhongChieu_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadNgayChieu();
        }

        private void btnInVe_Click_1(object sender, EventArgs e)
        {
            if (lstMaVe.Count != 0)
            {
                FormInVe InVe = new FormInVe();
                InVe.lstMaVe = new List<int>(lstMaVe);
                InVe.maKH = maKH;
                InVe.Show();
                lstMaVe.Clear();
            }
            else
                MessageBox.Show("Vui lòng đặt vé trước khi in!!!");
        }

        private void btnTimVe_Click(object sender, EventArgs e)
        {
            FormTimVe FTimVe = new FormTimVe();
            FTimVe.Show();
        }

        private void btnDatVe_Click(object sender, EventArgs e)
        {
            double dThanhTien = 0;
            string strSelected = "";
            foreach (Button btn in lstButton)
            {
                if (btn.BackColor == Color.Yellow)
                    dThanhTien += 99000;
                if (btn.BackColor == Color.LightYellow)
                    dThanhTien += 45000;
                strSelected += btn.Text + ",";
            }
            if (lstButton.Count != 0)
            {
                datGhe();
                MessageBox.Show("Bạn đã đặt những ghế: " + strSelected + "Tổng tiền: " + dThanhTien);
                lstButton.Clear();
                txtTenKhachhang.Clear();
            }
            else
                MessageBox.Show("Vui lòng chọn vị trí còn trống trước khi đặt");
        }

        private void cbBoxNgayChieu_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if (cbBoxNgayChieu.SelectedValue != null)
            {
                tblpGhe.Enabled = true;
                LoadGhe();
                loadGioChieu();
            }
            else
            {
                tblpGhe.Enabled = false;
            }
        }

        private void cbBoxGioChieu_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if (cbBoxGioChieu.DataSource != null)
            {
                string strSelect = "select GioKetThuc from Phim, LichChieu where Phim.MaPhim = LichChieu.MaPhim and Phim.MaPhim = '" + cbBoxTenPhim.SelectedValue.ToString() + "' and NgayChieu = '" + cbBoxNgayChieu.SelectedValue.ToString() + "' and GioBatDau = '" + cbBoxGioChieu.SelectedValue.ToString() + "'";
                da = new SqlDataAdapter(strSelect, c.conn);
                dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.Rows[0];
                txtGioKetThuc.Text = String.Format("{0:t}", dr["GioKetThuc"]);
            }
            else
            {
                txtGioKetThuc.Text = "";
            }
        }

       

    }
}
