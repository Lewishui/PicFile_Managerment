using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicFile_Managerment
{
    public partial class frmCutImage : Form
    {

        public Bitmap image1;   
        public Bitmap chushihua_image1;
     
        Point stpoint, endpoint;
        Rectangle rect1;
        Point borg = new Point(20, 20);
        bool Track_move = false;
        //private System.ComponentModel.IContainer components = null;
        string fullname;

        bool isDrag = false;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point startPoint, oldPoint;
        private Graphics ig;


        public frmCutImage(string fullname1, Bitmap image)
        {
            InitializeComponent();
            chushihua(fullname1, image);

        }

        private void chushihua(string fullname1, Bitmap image)
        {
            image1 = image;
            //pictureBox2.Image = (Image)image1;

            pictureBox2.Image = Image.FromFile(fullname1);
       
            fullname = fullname1;
          
            original_image_picture_box.Image = Image.FromFile(fullname1);
            original_image_picture_box.SizeMode = PictureBoxSizeMode.Zoom;

        }
        protected override void OnPaint(PaintEventArgs e)
        {

            //base.OnPaint(e);
            //e.Graphics.DrawImage(image1, borg);
            //if (rect1 != null)
            //{
            //    e.Graphics.DrawRectangle(new Pen(Color.Red, 1), rect1);
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmCutImage_Load(object sender, EventArgs e)
        {
            //showimg();
        }
        private void showimg()
        {
            int wd = 400;
            int hg = 200;
            int len = wd * hg * 3;
            byte[] pdata = new byte[len];
            for (int i = 0; i < len; i++)
            {
                if (i > 3 * wd * (hg / 2))
                {
                    pdata[i] = 255;
                }
                else
                {
                    pdata[i] = 0;
                }
            }

            try
            {
                image1 = new Bitmap(wd, hg, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                for (int y = 0; y < hg; y++)
                {
                    for (int x = 0; x < wd; x++)
                    {
                        Color crr = Color.FromArgb(pdata[3 * wd * y + x], pdata[3 * wd * y + x], pdata[3 * wd * y + x]);
                        image1.SetPixel(x, y, crr);
                    }
                }
                // Set the PictureBox to display the image.
                //  pictureBox1.Image = image1;
                Image myImage = System.Drawing.Image.FromFile(fullname);


                pictureBox1.Image = myImage;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("There was an error check data.");
            }
        }

        private void frmCutImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                stpoint = new Point(e.X, e.Y);
                Track_move = true;
                return;
            }
            Track_move = false;

        }

        private void frmCutImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Track_move == true)
            {
                Track_move = false;
                endpoint = new Point(e.X, e.Y);
                rect1 = new Rectangle(stpoint.X, stpoint.Y, endpoint.X - stpoint.X, endpoint.Y - stpoint.Y);
                Rectangle rectorg = new Rectangle(borg.X, borg.Y, image1.Width, image1.Height);
                if (rect1.Width <= 0)
                    return;
                if (rect1.Height <= 0)
                    return;
                if (rectorg.Contains(rect1))
                {
                    Rectangle rectadj = new Rectangle(rect1.X - borg.X, rect1.Y - borg.Y, rect1.Width, rect1.Height);
                    Bitmap cropimge = image1.Clone(rectadj, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    pictureBox1.Image = cropimge;
                }
                else
                {
                    pictureBox1.Image = null;
                }
                this.Invalidate();
            }

        }

        private void frmCutImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (Track_move)
                endpoint = new Point(e.X, e.Y);
            else
            {
                return;
            }
            rect1 = new Rectangle(stpoint.X, stpoint.Y, endpoint.X - stpoint.X, endpoint.Y - stpoint.Y);

            Rectangle tempr = new Rectangle(rect1.X, rect1.Y, rect1.Width + 2, rect1.Height + 2);
            this.Invalidate(tempr);

        }


        public Bitmap GetRect(Image pic, Rectangle Rect)
        {
            //创建图像
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }
        public Bitmap GetRectTo(Image pic, Rectangle Rect, Rectangle drawRect)
        {
            //创建图像
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            Bitmap pic = new Bitmap(fullname);
            theRectangle = new Rectangle(startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
            Bitmap p111ic = GetRect(pic, theRectangle);


            Bitmap cropimge = pic.Clone(theRectangle, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            pictureBox1.Image = cropimge;

            pictureBox2.Image = p111ic;

            pictureBox2.Width = p111ic.Width;
            pictureBox2.Height = p111ic.Height;


        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = new Point(e.X, e.Y);
            oldPoint = new Point(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            image1 = (Bitmap)pictureBox2.Image.Clone();
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            chushihua(fullname, chushihua_image1);
        }

    }
}
