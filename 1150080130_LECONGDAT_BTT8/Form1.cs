using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySach
{
    public partial class Form1 : Form
    {
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=E:\LECONGDAT_1150080130_BTTUAN8\1150080130_LECONGDAT_BTT8\1150080130_LECONGDAT_BTT8\Quanlybansach.mdf;
Integrated Security=True";

        SqlConnection sqlCon = null;

      
        
        public Form1()
        {
            InitializeComponent();
        }

 
        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }


        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }


        private void HienThiDanhSachNXB()
        {
            MoKetNoi();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "HienThiNXB"; 
            sqlCmd.Connection = sqlCon;

            SqlDataReader reader = sqlCmd.ExecuteReader();
            lsvDanhSach.Items.Clear();

            while (reader.Read())
            {
                string maNXB = reader.GetString(0);
                string tenNXB = reader.GetString(1);
                string diaChi = reader.GetString(2);

                ListViewItem lvi = new ListViewItem(maNXB);
                lvi.SubItems.Add(tenNXB);
                lvi.SubItems.Add(diaChi);

                lsvDanhSach.Items.Add(lvi);
            }

            reader.Close();
            DongKetNoi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDanhSachNXB();
        }

      
        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0)
                return;

            ListViewItem lvi = lsvDanhSach.SelectedItems[0];
            string maNXB = lvi.SubItems[0].Text;

            HienThiThongTinNXBTheoMa(maNXB);
        }


        private void HienThiThongTinNXBTheoMa(string maNXB)
        {
            MoKetNoi();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "HienThiChiTietNXB";
            sqlCmd.Connection = sqlCon;

            SqlParameter parMaNXB = new SqlParameter("@maNXB", SqlDbType.Char);
            parMaNXB.Value = maNXB;
            sqlCmd.Parameters.Add(parMaNXB);

            SqlDataReader reader = sqlCmd.ExecuteReader();

            txtMaNXB.Text = txtTenNXB.Text = txtDiaChi.Text = "";

            if (reader.Read())
            {
                txtMaNXB.Text = reader.GetString(0);
                txtTenNXB.Text = reader.GetString(1);
                txtDiaChi.Text = reader.GetString(2);
            }

            reader.Close();
            DongKetNoi();
        }
    }
}
