using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai06
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDest.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string sourcePath = txtSource.Text.Trim();
            string destPath = txtDest.Text.Trim();

            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show("Thư mục nguồn không tồn tại.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(destPath))
            {
                MessageBox.Show("Vui lòng chọn thư mục đích.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            try
            {
                string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
                int totalFiles = files.Length;

                if (totalFiles == 0)
                {
                    MessageBox.Show("Thư mục nguồn không có tập tin nào.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                progressBar1.Minimum = 0;
                progressBar1.Maximum = totalFiles;
                progressBar1.Value = 0;

                int copied = 0;

                foreach (string file in files)
                {
                    string relativePath = file.Substring(sourcePath.Length).TrimStart('\\');
                    string destFile = Path.Combine(destPath, relativePath);

                    string destDir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);

                    File.Copy(file, destFile, true);

                    copied++;
                    progressBar1.Value = copied;

                    int percent = copied * 100 / totalFiles;
                    lblStatus.Text = $"Đang sao chép: {file} ({copied}/{totalFiles})";
                    toolTip1.SetToolTip(progressBar1, $"Tiến độ: {percent}%");

                    Application.DoEvents(); 
                }

                lblStatus.Text = "Sao chép hoàn tất.";
                toolTip1.SetToolTip(progressBar1, "Hoàn tất 100%");

                MessageBox.Show("Đã sao chép xong tất cả tập tin.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi sao chép: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
