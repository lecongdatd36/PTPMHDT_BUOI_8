using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLySach
{
    public partial class Form4 : Form
    {
        // 🔹 Chuỗi kết nối
        // 🔹 Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=E:\LECONGDAT_1150080130_BTTUAN8\1150080130_LECONGDAT_BTT8\1150080130_LECONGDAT_BTT8\Quanlybansach.mdf;
Integrated Security=True";

        SqlConnection sqlCon = null;

        public Form4()
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

        // 🔹 Form Load
        private void Form4_Load(object sender, EventArgs e)
        {
            HienThiDanhSachNXB();
        }

        // 🔹 Nút Xóa dữ liệu
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhà xuất bản để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListViewItem lvi = lsvDanhSach.SelectedItems[0];
            string maNXB = lvi.SubItems[0].Text;

            DialogResult dr = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa NXB có mã [{maNXB}] không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            try
            {
                MoKetNoi();

                SqlCommand sqlCmd = new SqlCommand("XoaNXB", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@maNXB", maNXB);

                int result = sqlCmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Đã xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachNXB();
                }
                else
                {
                    MessageBox.Show("Không thể xóa dữ liệu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DongKetNoi();
            }
        }
    }
}
