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
    public partial class UserControlTimVe : UserControl
    {
        KetNoiCSDL k;
        SqlDataAdapter da;
        DataTable dt;
        public UserControlTimVe()
        {
            InitializeComponent();
            k = new KetNoiCSDL();
            k.ketNoi();
        }

        private void loadCbBoxTimVe()
        {
            cbBoxLoai.Items.Add("Tất cả");
            cbBoxLoai.Items.Add("Mã Vé");            
            cbBoxLoai.SelectedIndex = 0;
        }

        private void loadDgv_Ve(string strSelect)
        {            
            da = new SqlDataAdapter(strSelect, k.Conn);
            dt = new DataTable();
            da.Fill(dt);
            Dgv_Ve.DataSource = dt;
        }

        private void UserControlTimVe_Load(object sender, EventArgs e)
        {
            loadCbBoxTimVe();
            loadDgv_Ve("select ThongTinVe.MaVe, Day, ViTriDat, Gia.Gia, MaLC, MaNV, NgayDatVe from ThongTinVe,Gia WHERE ThongTinVe.IDGia=Gia.ID");
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string strSelect = "select MaVe, Day, ViTriDat, Gia.Gia, MaLC, MaNV, NgayDatVe from ThongTinVe,Gia WHERE ThongTinVe.IDGia=Gia.ID";
            string s = cbBoxLoai.SelectedItem.ToString();
            if (cbBoxLoai.SelectedItem.ToString().CompareTo("Mã Vé") == 0)
            {
                strSelect = "select MaVe, Day, ViTriDat,  Gia.Gia, MaLC, MaNV, NgayDatVe from ThongTinVe,Gia where ThongTinVe.IDGia=Gia.ID AND MaVe = '" + txtSearch.Text.Trim() + "'";
            }           
            da = new SqlDataAdapter(strSelect, k.Conn);
            dt = new DataTable();
            da.Fill(dt);
            Dgv_Ve.DataSource = dt;
            Dgv_Ve.Refresh();
        }
    }
}
