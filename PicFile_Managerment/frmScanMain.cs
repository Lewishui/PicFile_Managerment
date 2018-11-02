using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace PicFile_Managerment
{
    public partial class frmScanMain : Form
    {
        public string savefilepath;

        public frmScanMain()
        {
            InitializeComponent();
        }
        private ADFScan _scanner;
        private int[] _colors = new int[]
		{
			1,
			2,
			4
		};
        private int count = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = folderBrowserDialog.SelectedPath;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._scanner = new ADFScan();
            WIA.ImageFile imageFile = this._scanner.ScanAndSaveOnePage();
            if (imageFile != null)
            {
                var buffer = imageFile.FileData.get_BinaryData() as byte[];
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buffer, 0, buffer.Length);
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            //ScanColor color = (ScanColor)this._colors[this.comboBox1.SelectedIndex];
            //this._scanner = new ADFScan();
            //this._scanner.Scanning += new EventHandler<WiaImageEventArgs>(this._scanner_Scanning);
            //this._scanner.ScanComplete += new EventHandler(this._scanner_ScanComplete);

            //int dotsperinch = (int)this.numericUpDown1.Value;
            //this._scanner.BeginScan(color, dotsperinch);

        }
        private void _scanner_ScanComplete(object sender, EventArgs e)
        {
            MessageBox.Show("Scan Complete");
        }
        private void _scanner_Scanning(object sender, WiaImageEventArgs e)
        {
            string text = this.textBox1.Text + "image" + this.count++.ToString() + ".jpg";
            this.listBox1.Items.Add(text);
            e.ScannedImage.Save(text, ImageFormat.Jpeg);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                this.pictureBox1.Image = Image.FromFile(this.listBox1.SelectedItem.ToString());
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }
    }
}
