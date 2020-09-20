using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DoAn
{
    public partial class FormInVe : Form
    {
        public List<int> lstMaVe;
        public int maKH;
        public string giaVe;
        KetNoiCSDL k = new KetNoiCSDL();
        public FormInVe()
        {
            InitializeComponent();
            k.ketNoi();
        }          
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

            CrystalReport1 print = new CrystalReport1();
            print.Load(@"..\\..\\DoAn\\DoAn\\CrystalReport1.rpt");
            print.SetDatabaseLogon("sa", "33615755", "DESKTOP-JT2VOVA\\SQLEXPRESS", "dbQLRapPhim");
            SqlDataAdapter da = new SqlDataAdapter("getThongTinVe",k.Conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            da.Fill(ds,"ThongTinVe");
            print.SetDataSource(ds);
            foreach (int MaVe in lstMaVe)
            {
                print.Parameter_InHoaDonTheoVe.CurrentValues.AddValue(MaVe);

            }       
            print.SetParameterValue("InHoaDonTheoVe", print.Parameter_InHoaDonTheoVe.CurrentValues);
            crystalReportViewer1.ReportSource = print;
        }

    }
}
