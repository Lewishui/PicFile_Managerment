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
    public partial class frmSpecialEfficacy : Form
    {
        public Image ig;
        public frmSpecialEfficacy()
        {
            InitializeComponent();
        }

        private void tscbXG_SelectedIndexChanged(object sender, EventArgs e)
        {
            //private void tscbXG_SelectedIndexChanged(object sender, EventArgs e)
            {
                pictureBox2.Image = ig;
                string Ttype = tscbXG.Text.Trim();
                switch (Ttype)
                {

                    case "浮雕":

                        try
                        {
                            Bitmap myBitmap;
                            Image myImage = pictureBox2.Image;
                            myBitmap = new Bitmap(myImage);
                            for (int i = 0; i < myBitmap.Width - 1; i++)
                            {
                                for (int j = 0; j < myBitmap.Height - 1; j++)
                                {
                                    Color Color1 = myBitmap.GetPixel(i, j);
                                    Color Color2 = myBitmap.GetPixel(i + 1, j + 1);
                                    int red = Math.Abs(Color1.R - Color2.R + 128);
                                    int green = Math.Abs(Color1.G - Color2.G + 128);
                                    int blue = Math.Abs(Color1.B - Color2.B + 128);
                                    //颜色处理
                                    if (red > 255) red = 255;
                                    if (red < 0) red = 0;

                                    if (green > 255) green = 255;
                                    if (green < 0) green = 0;

                                    if (blue > 255) blue = 255;
                                    if (blue < 0) blue = 0;
                                    myBitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                                }
                            }
                            pictureBox2.Image = myBitmap;
                        }
                        catch
                        { }
                        break;



                    case "积木":

                        Graphics myGraphics = this.CreateGraphics();
                        Bitmap myBitmap1 = new Bitmap(pictureBox2.Image);
                        int myWidth, myHeight, m, n, iAvg, iPixel;
                        Color myColor, myNewColor;
                        RectangleF myRect;
                        myWidth = myBitmap1.Width;
                        myHeight = myBitmap1.Height;
                        myRect = new RectangleF(0, 0, myWidth, myHeight);
                        Bitmap bitmap = myBitmap1.Clone(myRect, System.Drawing.Imaging.PixelFormat.DontCare);
                        m = 0;
                        while (m < myWidth - 1)
                        {
                            n = 0;
                            while (n < myHeight - 1)
                            {
                                myColor = bitmap.GetPixel(m, n);
                                iAvg = (myColor.R + myColor.G + myColor.B) / 3;
                                iPixel = 0;
                                if (iAvg >= 128)
                                    iPixel = 255;
                                else
                                    iPixel = 0;
                                myNewColor = Color.FromArgb(255, iPixel, iPixel, iPixel);
                                bitmap.SetPixel(m, n, myNewColor);
                                n = n + 1;
                            }
                            m = m + 1;
                        }
                        myGraphics.Clear(Color.WhiteSmoke);
                        myGraphics.DrawImage(bitmap, new Rectangle(0, 0, myWidth, myHeight));
                        pictureBox2.Image = bitmap;
                        break;

                    case "底片":

                        int myh = pictureBox2.Image.Height;
                        int myw = pictureBox2.Image.Width;
                        Bitmap bitp = new Bitmap(myw, myh);
                        Bitmap mybitmap = (Bitmap)pictureBox2.Image;
                        Color Mpixel;
                        for (int mx = 1; mx < myw; mx++)
                        {
                            for (int my = 1; my < myh; my++)
                            {
                                int r, g, b;
                                Mpixel = mybitmap.GetPixel(mx, my);
                                r = 255 - Mpixel.R;
                                g = 255 - Mpixel.G;
                                b = 255 - Mpixel.B;
                                bitp.SetPixel(mx, my, Color.FromArgb(r, g, b));
                            }
                        }
                        pictureBox2.Image = bitp;

                        break;

                    case "雾化":

                        int wh = pictureBox2.Image.Height;
                        int ww = pictureBox2.Image.Width;
                        Bitmap wbitmap = new Bitmap(ww, wh);
                        Bitmap wmybitmap = (Bitmap)pictureBox2.Image;
                        Color wpixel;
                        for (int wx = 1; wx < ww; wx++)
                        {
                            for (int wy = 1; wy < wh; wy++)
                            {
                                Random wmyrandom = new Random();
                                int wk = wmyrandom.Next(123456);
                                int wdx = wx + wk % 19;
                                int wdy = wy + wk % 19;
                                if (wdx >= ww)
                                {
                                    wdx = ww - 1;
                                }
                                if (wdy >= wh)
                                {
                                    wdy = wh - 1;
                                }
                                wpixel = wmybitmap.GetPixel(wdx, wdy);
                                wbitmap.SetPixel(wx, wy, wpixel);
                            }
                        }
                        pictureBox2.Image = wbitmap;

                        break;

                }
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSpecialEfficacy_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = ig;
            pictureBox2.Image = ig;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        {
            if (tscbXG.Text.Trim() == "")
            {
                MessageBox.Show("请选择处理效果","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                saveFileDialog1.Filter = "BMP|*.bmp|JPEG|*.jpeg|GIF|*.gif|PNG|*.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string picPath = saveFileDialog1.FileName;
                    string picType = picPath.Substring(picPath.LastIndexOf(".") + 1, (picPath.Length - picPath.LastIndexOf(".") - 1));
                    switch (picType)
                    {
                        case "bmp":
                            Bitmap bt = new Bitmap(pictureBox2.Image);
                            Bitmap mybmp = new Bitmap(bt, ig.Width, ig.Height);
                            mybmp.Save(picPath, ImageFormat.Bmp); break;
                        case "jpeg":
                            Bitmap bt1 = new Bitmap(pictureBox2.Image);
                            Bitmap mybmp1 = new Bitmap(bt1, ig.Width, ig.Height);
                            mybmp1.Save(picPath, ImageFormat.Jpeg); break;
                        case "gif":
                            Bitmap bt2 = new Bitmap(pictureBox2.Image);
                            Bitmap mybmp2 = new Bitmap(bt2, ig.Width, ig.Height);
                            mybmp2.Save(picPath, ImageFormat.Gif); break;
                        case "png":
                            Bitmap bt3 = new Bitmap(pictureBox2.Image);
                            Bitmap mybmp3 = new Bitmap(bt3, ig.Width, ig.Height);
                            mybmp3.Save(picPath, ImageFormat.Png); break;
                    }
                }
            }
        }
        }
    }
}
