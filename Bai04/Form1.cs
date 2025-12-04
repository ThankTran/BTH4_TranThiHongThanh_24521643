using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Bai04
{
    public partial class Form1 : Form
    {
        private string currentFilePath = "";
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFontList();
            LoadSizeList();
            ResetDocument();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveDocument();
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.N)
            {
                ResetDocument();
                e.SuppressKeyPress = true;
            }
        }
        private void tsmCreateNew_Click(object sender, EventArgs e)
        {
            ResetDocument();
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            OpenDocument();
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ResetDocument();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }


        private void tsmFormat_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowColor = true;
            fd.Font = rtb_Text.SelectionFont ?? rtb_Text.Font;
            fd.Color = rtb_Text.SelectionColor;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                rtb_Text.SelectionFont = fd.Font;
                rtb_Text.SelectionColor = fd.Color;
                cb_Font.Text = fd.Font.Name;
                cb_Size.Text = fd.Font.Size.ToString();
            }
        }

        private void cb_Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Font.Text == "") return;

            Font current = rtb_Text.SelectionFont ?? rtb_Text.Font;
            rtb_Text.SelectionFont = new Font(cb_Font.Text, current.Size, current.Style);
        }

        private void cb_Size_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Size.Text == "") return;

            float newSize = float.Parse(cb_Size.Text);
            Font current = rtb_Text.SelectionFont ?? rtb_Text.Font;

            rtb_Text.SelectionFont = new Font(current.FontFamily, newSize, current.Style);
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Underline);
        }

        private void LoadFontList()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily font in fonts.Families)
                cb_Font.Items.Add(font.Name);
        }

        private void LoadSizeList()
        {
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int s in sizes)
                cb_Size.Items.Add(s.ToString());
        }

        private void ResetDocument()
        {
            rtb_Text.Clear();
            rtb_Text.Font = new Font("Tahoma", 14);
            rtb_Text.ForeColor = Color.Black;

            cb_Font.Text = "Tahoma";
            cb_Size.Text = "14";

            currentFilePath = "";
            rtb_Text.SelectionStart = 0;
        }

        private void OpenDocument()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rich Text Format (*.rtf)|*.rtf|Plain Text (*.txt)|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string ext = Path.GetExtension(ofd.FileName).ToLower();

                if (ext == ".rtf")
                    rtb_Text.LoadFile(ofd.FileName, RichTextBoxStreamType.RichText);
                else
                    rtb_Text.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);

                currentFilePath = ofd.FileName;
            }
        }

        private void SaveDocument()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Rich Text Format (*.rtf)|*.rtf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    rtb_Text.SaveFile(sfd.FileName, RichTextBoxStreamType.RichText);
                    currentFilePath = sfd.FileName;
                }
            }
            else
            {
                rtb_Text.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                MessageBox.Show("Lưu văn bản thành công!");
            }
        }

        private void ToggleStyle(FontStyle style)
        {
            if (rtb_Text.SelectionFont == null) return;

            Font current = rtb_Text.SelectionFont;
            FontStyle newStyle = current.Style.HasFlag(style)
                                ? current.Style & ~style
                                : current.Style | style;

            rtb_Text.SelectionFont = new Font(current.FontFamily, current.Size, newStyle);
        }
    }
}
