using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicFile_Managerment
{
    public partial class frmCut2 : Form
    {
        bool isDrag = false;                                                    //是否可以剪切图片
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));                //实例化Rectangle类
        Point startPoint, oldPoint;                                             //定义Point类
        private Graphics ig;
        Bitmap image1;
        string fullname;
        public frmCut2(string fullname1, Bitmap image)
        {
            InitializeComponent();
            fullname = fullname1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                                       //打开文件对话框
            Image myImage = System.Drawing.Image.FromFile(fullname);        //实例化Image类
            pictureBox1.Image = myImage;     
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //如果开始绘制，则开始记录鼠标位置
                if ((isDrag = !isDrag) == true)
                {
                    startPoint = new Point(e.X, e.Y);                               //记录鼠标的当前位置
                    oldPoint = new Point(e.X, e.Y);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrag = false;
            ig = pictureBox1.CreateGraphics();                              //创建pictureBox1控件的Graphics类
            //绘制矩形框
            ig.DrawRectangle(new Pen(Color.Black, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
            theRectangle = new Rectangle(startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);   //获得矩形框的区域
      
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();                                     //为当前窗体创建Graphics类
            if (isDrag)                                                     //如果鼠示已按下
            {
                //绘制一个矩形框
                g.DrawRectangle(new Pen(Color.Black, 1), startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
            }
  
        }

        private void frmCut2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Graphics graphics = this.CreateGraphics();                      //为当前窗体创建Graphics类
                Bitmap bitmap = new Bitmap(pictureBox1.Image);                  //实例化Bitmap类
                Bitmap cloneBitmap = bitmap.Clone(theRectangle, PixelFormat.DontCare);//通过剪切图片的大小实例化Bitmap类
                graphics.DrawImage(cloneBitmap, e.X, e.Y);                      //绘制剪切的图片
                Graphics g = pictureBox1.CreateGraphics();
                SolidBrush myBrush = new SolidBrush(Color.White);               //定义画刷
                g.FillRectangle(myBrush, theRectangle);                         //绘制剪切后的图片
            }
            catch { }
  

        }
    }
}
