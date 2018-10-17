using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PicFile_Managerment
{
    public partial class frmWater : Form
    {
        public Image ig;
        public string FPath;
        FontStyle Fstyle = FontStyle.Regular;
        float Fsize = 18;
        Color Fcolor = System.Drawing.Color.Yellow;
        FontFamily a = FontFamily.GenericMonospace;
        int Fwidth;
        int Fheight;


        public frmWater()
        {
            InitializeComponent();
        }
        public void makeWatermark(int x, int y, string txt)
        {
            System.Drawing.Image image = Image.FromFile(FPath);
            System.Drawing.Graphics e = System.Drawing.Graphics.FromImage(image);
            System.Drawing.Font f = new System.Drawing.Font(a, Fsize, Fstyle);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(Fcolor);
            e.DrawString(txt, f, b, x, y);
            SizeF XMaxSize = e.MeasureString(txt, f);

            Fwidth = (int)XMaxSize.Width;
            Fheight = (int)XMaxSize.Height;

            e.Dispose();
            pictureBox1.Image = image;
        }

        private void frmWater_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = ig;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            fontDialog1.ShowColor = true;
            fontDialog1.ShowHelp = false;
            fontDialog1.ShowApply = false;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                Fstyle = fontDialog1.Font.Style;
                Fcolor = fontDialog1.Color;
                Fsize = fontDialog1.Font.Size;
                a = fontDialog1.Font.FontFamily;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                pictureBox1.Image = ig;
                if (txtChar.Text.Trim() != "")
                {
                    if (radioButton1.Checked)
                    {
                        int x = 10, y = 10;
                        makeWatermark(x, y, txtChar.Text.Trim());
                    }
                    if (radioButton2.Checked)
                    {
                        int x1 = 10, y1 = ig.Height - Fheight;
                        makeWatermark(x1, y1, txtChar.Text.Trim());
                    }
                    if (radioButton3.Checked)
                    {
                        int x2 = (int)(ig.Width - Fwidth) / 2;
                        int y2 = (int)(ig.Height - Fheight) / 2;
                        makeWatermark(x2, y2, txtChar.Text.Trim());
                    }
                    if (radioButton4.Checked)
                    {
                        int x3 = ig.Width - Fwidth;
                        int y3 = 10;
                        makeWatermark(x3, y3, txtChar.Text.Trim());
                    }
                    if (radioButton5.Checked)
                    {
                        int x4 = ig.Width - Fwidth;
                        int y4 = ig.Height - Fheight;
                        makeWatermark(x4, y4, txtChar.Text.Trim());
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "BMP|*.bmp|JPEG|*.jpeg|GIF|*.gif|PNG|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string picPath = saveFileDialog1.FileName;
                string picType = picPath.Substring(picPath.LastIndexOf(".") + 1, (picPath.Length - picPath.LastIndexOf(".") - 1));
                switch (picType)
                {
                    case "bmp":
                        Bitmap bt = new Bitmap(pictureBox1.Image);
                        Bitmap mybmp = new Bitmap(bt, ig.Width, ig.Height);
                        mybmp.Save(picPath, ImageFormat.Bmp); break;
                    case "jpeg":
                        Bitmap bt1 = new Bitmap(pictureBox1.Image);
                        Bitmap mybmp1 = new Bitmap(bt1, ig.Width, ig.Height);
                        mybmp1.Save(picPath, ImageFormat.Jpeg); break;
                    case "gif":
                        Bitmap bt2 = new Bitmap(pictureBox1.Image);
                        Bitmap mybmp2 = new Bitmap(bt2, ig.Width, ig.Height);
                        mybmp2.Save(picPath, ImageFormat.Gif); break;
                    case "png":
                        Bitmap bt3 = new Bitmap(pictureBox1.Image);
                        Bitmap mybmp3 = new Bitmap(bt3, ig.Width, ig.Height);
                        mybmp3.Save(picPath, ImageFormat.Png); break;
                }
            }
        }


    }
}
