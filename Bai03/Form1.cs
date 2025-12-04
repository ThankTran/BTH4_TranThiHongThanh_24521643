using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string now = DateTime.Now.ToString("hh:mm:ss tt");

            toolStripStatusLabel1.Text =
                $"Hôm nay là ngày {today} - Bây giờ là {now}";

            timer1.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chọn tập tin media";
            ofd.Filter = "Media Files|*.mp3;*.mp4;*.mpeg;*.avi;*.wav;*.midi;*.mpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = ofd.FileName;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string now = DateTime.Now.ToString("hh:mm:ss tt");

            toolStripStatusLabel1.Text =
                $"Hôm nay là ngày {today} - Bây giờ là {now}";
        }

        
    }
}
