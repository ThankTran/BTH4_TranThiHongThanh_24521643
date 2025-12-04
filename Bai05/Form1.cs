using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai05
{
    public partial class Form1 : Form
    {
        private string currentFilePath = "";
        private string connStr = @"Data Source=localhost;Initial Catalog=QLSV;User ID=User;Password=thanh;";
        private SqlConnection conn;
        private SqlDataAdapter myAdapter;
        private DataSet ds;
        private DataTable dt;

        private void LoadData()
        {
            conn = new SqlConnection(connStr);
            conn.Open();
            myAdapter = new SqlDataAdapter("SELECT * FROM SINHVIEN", conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "SINHVIEN");
            dt = ds.Tables["SINHVIEN"];
            dgvSinhVien.DataSource = dt;
            conn.Close();
        }
        private void dgvSinhVien_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvSinhVien.Rows[e.RowIndex].Cells[0].Value = (e.RowIndex + 1).ToString();
        }
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dgvSinhVien.Columns["MaSV"].DataPropertyName = "MASV";

            dgvSinhVien.AutoGenerateColumns = false;

            dgvSinhVien.AllowUserToAddRows = false;
            LoadData();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                showForm2();
                e.SuppressKeyPress = true;
            }
        }

        private void showForm2()
        {
            using (Form2 f2 = new Form2())
            {
                if (f2.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void tslAdd_Click(object sender, EventArgs e)
        {
            showForm2();
        }

        private void tsmAdd_Click(object sender, EventArgs e)
        {
            showForm2();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            if (dt == null) return;

            string keyword = txtSearchName.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                dt.DefaultView.RowFilter = string.Empty;
                return;
            }

            string safeKeyword = keyword.Replace("'", "''");

            dt.DefaultView.RowFilter = $"TENSV LIKE '%{safeKeyword}%'";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow == null || dgvSinhVien.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên cần xóa.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object cellValue = dgvSinhVien.CurrentRow.Cells["MaSV"].Value;
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                MessageBox.Show("Không lấy được mã sinh viên.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maSV = cellValue.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sinh viên có mã {maSV}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM SINHVIEN WHERE MASV = @MASV";
                    using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.Add("@MASV", SqlDbType.VarChar, 10).Value = maSV;
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Xóa sinh viên thành công.", "Thông báo",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên cần xóa.", "Thông báo",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
