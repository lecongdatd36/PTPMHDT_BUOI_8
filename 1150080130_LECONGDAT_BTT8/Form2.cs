using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySach
{
    public partial class Form2 : Form
    {
        // 🔹 Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=E:\LECONGDAT_1150080130_BTTUAN8\1150080130_LECONGDAT_BTT8\1150080130_LECONGDAT_BTT8\Quanlybansach.mdf;
Integrated Security=True";

        SqlConnection sqlCon = null;

        public Form2()
        {
            InitializeComponent();
        }

        // 🔹 Mở kết nối
        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }

        // 🔹 Đóng kết nối
        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }

        // 🔹 Hiển thị danh sách Nhà Xuất Bản
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

        // 🔹 Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDanhSachNXB();
        }

        // 🔹 Nút Thêm Nhà Xuất Bản
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNXB.Text) ||
                string.IsNullOrWhiteSpace(txtTenNXB.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            try
            {
                MoKetNoi();

                SqlCommand sqlCmd = new SqlCommand("ThemDuLieu", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@maNXB", txtMaNXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@tenNXB", txtTenNXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@diaChi", txtDiaChi.Text.Trim());

                int result = sqlCmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm nhà xuất bản thành công!", "Thông báo");
                    HienThiDanhSachNXB();

                    txtMaNXB.Clear();
                    txtTenNXB.Clear();
                    txtDiaChi.Clear();
                }
                else
                {
                    MessageBox.Show("Không thể thêm dữ liệu!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }
    }
}
