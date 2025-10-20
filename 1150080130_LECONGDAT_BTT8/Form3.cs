using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySach
{
    public partial class Form3 : Form
    {
        // 🔹 Chuỗi kết nối
        // 🔹 Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=E:\LECONGDAT_1150080130_BTTUAN8\1150080130_LECONGDAT_BTT8\1150080130_LECONGDAT_BTT8\Quanlybansach.mdf;
Integrated Security=True";

        SqlConnection sqlCon = null;

        public Form3()
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

        // 🔹 Hiển thị danh sách NXB
        private void HienThiDanhSachNXB()
        {
            MoKetNoi();
            SqlCommand sqlCmd = new SqlCommand("HienThiNXB", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;

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

        // 🔹 Load Form3
        private void Form3_Load(object sender, EventArgs e)
        {
            HienThiDanhSachNXB();
        }

        // 🔹 Khi chọn dòng trong ListView
        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0)
                return;

            ListViewItem lvi = lsvDanhSach.SelectedItems[0];
            txtMaNXB.Text = lvi.SubItems[0].Text;
            txtTenNXB.Text = lvi.SubItems[1].Text;
            txtDiaChi.Text = lvi.SubItems[2].Text;
        }

        // 🔹 Nút Cập nhật thông tin
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNXB.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhà xuất bản để cập nhật!", "Thông báo");
                return;
            }

            try
            {
                MoKetNoi();

                SqlCommand sqlCmd = new SqlCommand("CapNhatThongTin", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@maNXB", txtMaNXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@tenNXB", txtTenNXB.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@diaChi", txtDiaChi.Text.Trim());

                int result = sqlCmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    HienThiDanhSachNXB();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật dữ liệu!", "Lỗi");
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
