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
    public partial class UserControlBookVe : UserControl
    {
        KetNoiCSDL c;
        DataTable dt;
        SqlDataAdapter da;
        DataColumn[] key = new DataColumn[1];
        List<Button> lstButton;
        List<int> lstMaVe;
        string tienVe = "";
        Int32 maKH;
        public string MaLichChieu = "";
        public string ngayChieu = "";
        public string gioChieu = "";
        public string MaPhim = "";
        public Image AnhPhim;
        string maNV = "";
        string idgiathuong;
        string idgiahs;
        double giahs;
        double giathuong;
        public UserControlBookVe(string manv)
        {
            InitializeComponent();
            c = new KetNoiCSDL();
            c.ketNoi();
            lstButton = new List<Button>();
            lstMaVe = new List<int>();
            tblpGhe.Enabled = false;
            this.maNV = manv;
            loadDLCheckBox();
        }

        private void LoadPhongChieu()
        {
            string strSelect = "select TenPhong,MaLC  from LichChieu, PhongChieu where LichChieu.MaPC = PhongChieu.MaPC and LichChieu.GioBatDau='" + ngayChieu + " " + gioChieu + "' AND LichChieu.MaPhim='" + MaPhim + "'";
            da = new SqlDataAdapter(strSelect, c.Conn);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                cbBoxPhongChieu.Enabled = true;
                string s = dt.Columns[0].ToString();
                string s1 = dt.Columns[1].ToString();
                cbBoxPhongChieu.DataSource = dt;
                cbBoxPhongChieu.DisplayMember = s;
                cbBoxPhongChieu.ValueMember = s1;
                cbBoxPhongChieu.SelectedIndex = 0;

            }
            else
            {
                cbBoxPhongChieu.DataSource = null;
                cbBoxPhongChieu.Items.Clear();
                cbBoxPhongChieu.Enabled = false;
            }
        }
        private void LoadGhe()
        {
            int gheTrong = 0;
            int gheDaDat = 0;
            lblGheTrong.Text = "Ghế Trống";
            lblDaDat.Text = "Đã Đặt";
            if (cbBoxPhongChieu.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                tblpGhe.Enabled = true;
                string strSelect = "select * from PhongChieu,LichChieu where LichChieu.MaPC=PhongChieu.MaPC AND LichChieu.MaLC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "'";
                da = new SqlDataAdapter(strSelect, c.Conn);
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
                lblDaDat.Text = lblDaDat.Text + "  (" + gheDaDat + ")";
                lblGheTrong.Text = lblGheTrong.Text + "  (" + gheTrong + ")";
            }

        }
        private void layTenNV()
        {
            string strSql = "select * from NhanVien WHERE MaNhanVien='"+maNV+"'";

            if (c.Conn.State == ConnectionState.Closed)
                c.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, c.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lblTenNhanVien.Text = lblTenNhanVien.Text+ " :" + rd["HoTen"].ToString();
            }
            rd.Close();
            if (c.Conn.State == ConnectionState.Open)
                c.Conn.Close();
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
            string s = string.Format("{0:d}", ngayChieu);
            lblNgayChieu.Text = s;
            lblXuatChieu.Text = gioChieu;
            try
            {
                imgAnhPhim.Image = AnhPhim;
            }
            catch (Exception)
            {
                Image img = Image.FromFile("..\\..\\img\\10.jpg");
                imgAnhPhim.Image = img;
            }
            LoadPhongChieu();
            LoadGhe();
            layTenNV();
        }
        private Boolean checkGhe(Button btn)
        {
            string strSelect = "select * from LichChieu, ThongTinVe where LichChieu.MaLC = ThongTinVe.MaLC and  ThongTinVe.MaLC = '" + cbBoxPhongChieu.SelectedValue.ToString() + "'";
            da = new SqlDataAdapter(strSelect, c.Conn);
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
            if (c.Conn != null && c.Conn.State != ConnectionState.Open)
                c.Conn.Open();
            string strSelect1 = "select count(*) from ThongTinVe";
            SqlCommand cmd = new SqlCommand(strSelect1, c.Conn);
            Int32 maVe = Convert.ToInt32(cmd.ExecuteScalar());
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
                    tienVe = idgiathuong;
                    if (b.BackColor == Color.LightYellow)
                        tienVe = idgiahs;
                    b.BackColor = Color.Red;                  
                    string strInsert = "insert into ThongTinVe values('" + maVe.ToString() + "','" + Day + "','" + ViTriDat + "'," + cbBoxPhongChieu.SelectedValue.ToString() + ",'"+maNV+"','" + DateTime.Now + "','"+tienVe+"')";
                    lstMaVe.Add(maVe);
                    SqlCommand cmd1 = new SqlCommand(strInsert, c.Conn);
                    cmd1.ExecuteNonQuery();

                    string strSelect2 = "select * from ThongTinVe, Gia where Gia.id = ThongTinVe.idGia and MaVe = " + maVe.ToString() + " and ID =" + tienVe;
                    if (c.Conn.State == ConnectionState.Closed)
                        c.Conn.Open();
                    SqlCommand cmd2 = new SqlCommand(strSelect2, c.Conn);
                    SqlDataReader rd = cmd2.ExecuteReader();
                    while (rd.Read())
                    {
                        tienVe = rd["Gia"].ToString();                        
                    }
                    rd.Close();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Data.ToString());
                }
            }
            if (c.Conn.State == ConnectionState.Open)
                c.Conn.Close();
        }
        private void btnInVe_Click(object sender, EventArgs e)
        {
            if (lstMaVe.Count != 0)
            {
                FormInVe InVe = new FormInVe();
                InVe.lstMaVe = new List<int>(lstMaVe);
                InVe.maKH = maKH;
                InVe.giaVe = tienVe;
                InVe.Show();
                
                lstMaVe.Clear();
            }
            else
                MessageBox.Show("Vui lòng đặt vé trước khi in!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void loadDLCheckBox()
        {
            int i = 0;
            string strSql = "select * from Gia";
            if (c.Conn.State == ConnectionState.Closed)
                c.Conn.Open();
            SqlCommand cmd = new SqlCommand(strSql, c.Conn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                i++;
                if (i == 1)
                {
                    rdStudent.Text = rd["Ten"].ToString();
                    idgiahs = rd["ID"].ToString();
                    giahs=double.Parse(rd["Gia"].ToString());
                }
                else
                {
                    idgiathuong = rd["ID"].ToString();
                    giathuong = double.Parse(rd["Gia"].ToString());
                    rdNormal.Text = rd["Ten"].ToString();
                }
            }
            rd.Close();
            if (c.Conn.State == ConnectionState.Open)
                c.Conn.Close();
        }
        private void btnDatVe_Click(object sender, EventArgs e)
        {
            double dThanhTien = 0;
            string strSelected = "";
            foreach (Button btn in lstButton)
            {
                if (btn.BackColor == Color.Yellow)
                    dThanhTien += giathuong;
                if (btn.BackColor == Color.LightYellow)
                    dThanhTien += giahs;
                strSelected += btn.Text + ",";
            }
            if (lstButton.Count != 0)
            {
                datGhe();
                DialogResult r;
                r = MessageBox.Show("Bạn đã đặt những ghế: " + strSelected + "Tổng tiền: " + dThanhTien, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                lstButton.Clear();
            }
            else
                MessageBox.Show("Vui lòng chọn vị trí còn trống trước khi đặt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void cbBoxPhongChieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGhe();           
        }
    }
}
