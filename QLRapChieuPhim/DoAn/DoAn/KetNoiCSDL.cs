using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn
{
    class KetNoiCSDL
    {
        SqlConnection conn;

        public SqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }
        
        public void ketNoi()
        {
             String strcnHome = "Data Source=DESKTOP-JT2VOVA\\SQLEXPRESS;Initial Catalog=dbQLRapPhim;User ID=sa;Password=123";
             conn = new SqlConnection(strcnHome);
        }
    }
}
