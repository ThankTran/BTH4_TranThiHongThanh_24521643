using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Bai05
{
    public partial class Form2 : Form
    {
        private string connStr = @"Data Source=localhost;Initial Catalog=QLSV;User ID=User;Password=thanh;";

        public Form2()
        {
            InitializeComponent();
        }
        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[\p{L} ]+$");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            string tenSV = txtTenSV.Text.Trim();
            string khoa = cboKhoa.Text.Trim();
            string diemText = txtDiemTB.Text.Trim();

            if (string.IsNullOrEmpty(maSV) ||
                string.IsNullOrEmpty(tenSV) ||
                string.IsNullOrEmpty(khoa) ||
                string.IsNullOrEmpty(diemText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidName(tenSV))
            {
                MessageBox.Show("Tên sinh viên chỉ được chứa chữ cái và khoảng trắng.\n" +
                                "Không được chứa số hoặc ký tự đặc biệt.", "Sai định dạng tên",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(diemText, out double diemTB) ||
                diemTB < 0 || diemTB > 10)
            {
                MessageBox.Show("Điểm TB phải là số từ 0 đến 10.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string checkSql = "SELECT COUNT(*) FROM SINHVIEN WHERE MASV = @MASV";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MASV", maSV);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã sinh viên đã tồn tại.", "Thông báo",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string insertSql = @"INSERT INTO SINHVIEN (MASV, TENSV, KHOA, DIEMTB)
                                         VALUES (@MASV, @TENSV, @KHOA, @DIEMTB)";

                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 10).Value = maSV;
                        cmd.Parameters.Add("@TENSV", SqlDbType.NVarChar, 50).Value = tenSV;
                        cmd.Parameters.Add("@KHOA", SqlDbType.NVarChar, 50).Value = khoa;
                        cmd.Parameters.Add("@DIEMTB", SqlDbType.Float).Value = diemTB;

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm sinh viên thành công.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
