using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading.Tasks;
using ImageClassLib;
using Gif.Components;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using FA.Buiness;
using FA.DB;
using FA.Common;
namespace PicFile_Managerment
{
    public partial class frmPicEdit : Form
    {
        Bitmap img, imgview;  //图片图像
        string fullname; //文件路径+文件名，用于保存
        Bitmap m_bmp;               //画布中的图像
        Point m_ptCanvas;           //画布原点在设备上的坐标
        Point m_ptCanvasBuf;        //重置画布坐标计算时用的临时变量
        Point m_ptBmp;              //图像位于画布坐标系中的坐标
        float m_nScale = 1.0F;      //缩放比例
        Image erweima_image;

        Point m_ptMouseDown;        //鼠标点下是在设备坐标上的坐标

        string m_strMousePt;        //鼠标当前位置对应的坐标
        Bitmap img2;
        string pic1; //文件路径+文件名，用于保存
        string pic2; //文件路径+文件名，用于保存
        bool cautimage = false;
        private int number = 0;

        Bitmap myBmp;
        Point mouseDownPoint = new Point(); //记录拖拽过程鼠标位置
        bool isMove = false;    //判断鼠标在picturebox上移动时，是否处于拖拽过程(鼠标左键是否按下)
        int zoomStep = 20;
        /// <summary>
        /// /////////
        /// </summary>
        int x1;     //鼠标按下时横坐标
        int y1;     //鼠标按下时纵坐标
        int width;  //所打开的图像的宽
        int heigth; //所打开的图像的高
        bool HeadImageBool = false;    // 此布尔变量用来判断pictureBox1控件是否有图片
        #region 画矩形使用到的变量
        Point p1;   //定义鼠标按下时的坐标点
        Point p2;   //定义移动鼠标时的坐标点
        Point p3;   //定义松开鼠标时的坐标点
        #endregion
        ImageCut1 IC1;  //定义所画矩形的图像对像



        public Bitmap img1;
        public string FPath;
        public string PictureWidth;
        public string Pictureheight;
        public double SelectFileSize;
        private int box1_xlcX = 0;
        private int box1_xlcY = 0;



        bool isDrag = false;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point startPoint, oldPoint;
        private Graphics ig;
        Point borg = new Point(20, 20);


        #region variances
        private string curFileName = null;
        private Bitmap curBitmap = null;
        private List<string> pathString = new List<string>();
        private List<Bitmap> thumbnailImage = new List<Bitmap>();
        private List<Bitmap> srcImage = new List<Bitmap>();
        private Bitmap temp = null;
        private int count = 0;
        //private int number = 0;
        private int deleteNumber = 0;
        #endregion

        List<clsAccFileinfo> dailyResult;
        bool Track_move = false;
        public string savefilepath;
        List<string> filename;
        int comboxi;
        string comboxiname;
        clsFile_Managermentinfo selcetitem;


        public frmPicEdit(string id, clsFile_Managermentinfo selcetitem1)
        {
            InitializeComponent();
            selcetitem = selcetitem1;
            InitializeDataSource(id);
            this.pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            InitializeListView();
        }

        private void InitializeDataSource(string id)
        {

            string conditions = "";

            conditions = "select * from AccFile where accfile_id='" + id + "'";

            clsAllnew BusinessHelp = new clsAllnew();
            dailyResult = new List<clsAccFileinfo>();
            if (id != null)
                dailyResult = BusinessHelp.find_ACCFile(conditions);
            #region listView1
            this.listView1.Items.Clear();
            if (dailyResult != null)
            {
                listView1.View = View.LargeIcon;
                ImageList imageListSmall = new ImageList();
                int i = 0;

                foreach (clsAccFileinfo item in dailyResult)
                {

                    // this.listView1.Items.Add(System.IO.Path.GetFileName(item.mark1));
                    //产生图像对象

                    //imageListSmall.Images.Add(Bitmap.FromFile(item.mark1));
                    if (File.Exists(item.mark1))
                    {
                        imageListSmall.Images.Add(Image.FromFile(item.mark1));

                        imageListSmall.ImageSize = new Size(64, 128);// new Point(32, 32);
                    }
                }
                listView1.View = View.LargeIcon;
                listView1.LargeImageList = imageListSmall;

                int Index = 1;
                foreach (clsAccFileinfo item in dailyResult)
                {
                    if (File.Exists(item.mark1))
                    {
                        //  listView1.Items.Add(System.IO.Path.GetFileName(item.mark1));
                        listView1.Items.Add(Index.ToString());
                        listView1.Items[i].ImageIndex = i;
                        i++;
                        Index++;
                    }
                }
            }
            #endregion
            if (dailyResult.Count > 0)
                ShowImage(dailyResult[0].mark1);
            //  CreateMyListView();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //CreateMyListView1();
            //return;

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "bmp,jpg,gif,png,tiff,icon|*.bmp;*.jpg;*.gif;*.png;*.tiff;*.icon";
            OpenFileDialog1.Title = "选择图片";
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fullname = OpenFileDialog1.FileName.ToString();

                //img1 = new Bitmap(OpenFileDialog1.FileName);
                //imgview = new Bitmap(img1.Width, img1.Height);
                //Graphics draw = Graphics.FromImage(imgview);
                //draw.DrawImage(img1, 0, 0, img1.Width, img1.Height);

                //if (imgview.Width > pictureBox1.Width || imgview.Height > pictureBox1.Height)
                //{
                //    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                //}
                //else
                //{
                //    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                //}
                ShowImage(fullname);
            }
        }

        private void ShowImage(string fullname1)
        {
            try
            {
                fullname = fullname1;

                FPath = fullname;

                myBmp = new Bitmap(fullname);
                img = myBmp;

                //pictureBox1.Image = (Image)imgview;
                //OpenFileDialog1.Dispose();
                //draw.Dispose();
                //img1.Dispose();
                //img = (Bitmap)imgview.Clone();
                //pic1 = fullname;


                #region 第二中显示方法

                pictureBox1.Image = Image.FromFile(fullname);
                pictureBox1.Width = 587;
                pictureBox1.Height = 334;


                img1 = new Bitmap(fullname);
                imgview = new Bitmap(img1.Width, img1.Height);
                PictureWidth = img1.Width.ToString();
                Pictureheight = img1.Height.ToString();
                FileInfo finfo = new FileInfo(fullname);
                string FileSize = finfo.Length.ToString();
                SelectFileSize = Convert.ToDouble(FileSize) / 1024 / 1024;
                img1.Dispose();

                ShowInfo();
                //contextMenuStrip1.Enabled = true;
                //ToolStatusEnable();
                //Pindex = listBox1.SelectedIndex;//当前选项索引值
                pictureBox1.BackColor = Color.Transparent;
                //img1.b
                pictureBox1.Parent = pictureBox2;
                #endregion
            }
            catch (Exception ex)
            {
                return;

                throw;
            }
        }

        private void ShowInfo()
        {
            this.label1.Text = "图片大小：" + SelectFileSize.ToString("F") + "M   " + "分辨率：" + PictureWidth + "×" + Pictureheight;
            this.label2.Text = listView1.Items.Count.ToString();
            this.label3.Text = Convert.ToString(listView1.SelectedItems[0].Index + 1);
            this.label3.Visible = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (fullname == null)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|png|*.png|tiff|*.tiff| icon| *.icon";
            sfd.OverwritePrompt = true;
            sfd.Title = "保存图像";
            sfd.ValidateNames = true;
            sfd.RestoreDirectory = true;
            sfd.FileName = fullname;
            switch (Path.GetExtension(fullname).ToLower())
            {
                case ".jpg": sfd.FilterIndex = 1; break;
                case ".bmp": sfd.FilterIndex = 2; break;
                case ".gif": sfd.FilterIndex = 3; break;
                case ".png": sfd.FilterIndex = 4; break;
                case ".tiff": sfd.FilterIndex = 5; break;
                case ".icon": sfd.FilterIndex = 6; break;
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (img != null)
                {
                    fullname = sfd.FileName;
                    switch (sfd.FilterIndex)
                    {
                        case 1: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                        case 2: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp); break;
                        case 3: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif); break;
                        case 4: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png); break;
                        case 5: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Tiff); break;
                        case 6: img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Icon); break;

                    }
                    MessageBox.Show("保存成功！");

                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //private void toolStripButton3_Click(object sender, EventArgs e)//显示图片实际大小，这部分可以参考打开图片根据宽高再显示
            {
                if (img != null)
                {
                    imgview = new Bitmap(img.Width, img.Height);
                    Graphics draw = Graphics.FromImage(imgview);
                    draw.DrawImage(img, 0, 0, img.Width, img.Height);
                    if (imgview.Width > pictureBox1.Width || imgview.Height > pictureBox1.Height)
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    //pictureBox1.Image = (Image)imgview;
                    draw.Dispose();
                    //img = (Bitmap)imgview.Clone();
                    this.pictureBox1.Image = img;//用上边的话，这样的如果img对象改变后就不会在实际大小显示变化后的图片
                }

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //private void toolStripButton4_Click(object sender, EventArgs e)//放大图片功能
            {

                //10%放大
                if (imgview.Width >= pictureBox1.Width * 1.1 && imgview.Height >= pictureBox1.Height * 1.1)//乘与1.1是最小要求，当然更大也更好了，一样的当前规格窗口图片放大的就更大了
                {
                    pictureBox1.Width = (int)Math.Ceiling(pictureBox1.Width * 1.1);
                    pictureBox1.Height = (int)Math.Ceiling(pictureBox1.Height * 1.1);
                }
                else
                {
                    if ((int)Math.Ceiling(imgview.Width * 1.1) >= pictureBox1.Width && (int)Math.Ceiling(imgview.Height * 1.1) >= pictureBox1.Height)
                    {
                        Bitmap img1 = new Bitmap((int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        //img = new Bitmap((int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        Graphics draw = Graphics.FromImage(img1);
                        draw.DrawImage(imgview, 0, 0, (int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        imgview = (Bitmap)img1.Clone();
                        draw.Dispose();
                        img1.Dispose();
                        pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                        pictureBox1.Image = imgview;
                    }
                    else
                    {
                        Bitmap img1 = new Bitmap((int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        //img = new Bitmap((int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        Graphics draw = Graphics.FromImage(img1);
                        draw.DrawImage(imgview, 0, 0, (int)Math.Ceiling(imgview.Width * 1.1), (int)Math.Ceiling(imgview.Height * 1.1));
                        imgview = (Bitmap)img1.Clone();
                        draw.Dispose();
                        img1.Dispose();
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        pictureBox1.Image = imgview;

                    }
                }
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //private void toolStripButton5_Click(object sender, EventArgs e)//缩小图片功能
            {
                //10%缩放
                if (imgview.Width >= pictureBox1.Width * 2 && imgview.Height >= pictureBox1.Height * 2)//这里应该是乘与个比较大的数为好，这样万一放大特别大了可以缩小回去
                {
                    pictureBox1.Width = (int)Math.Ceiling(pictureBox1.Width * 0.9);
                    pictureBox1.Height = (int)Math.Ceiling(pictureBox1.Height * 0.9);
                }
                else
                {
                    if ((int)Math.Ceiling(imgview.Width * 0.9) >= pictureBox1.Width || (int)Math.Ceiling(imgview.Height * 0.9) >= pictureBox1.Height)
                    {
                        Bitmap img1 = new Bitmap((int)Math.Ceiling(imgview.Width * 0.9), (int)Math.Ceiling(imgview.Height * 0.9));
                        Graphics draw = Graphics.FromImage(img1);
                        draw.DrawImage(imgview, 0, 0, (int)Math.Ceiling(imgview.Width * 0.9), (int)Math.Ceiling(imgview.Height * 0.9));
                        imgview = (Bitmap)img1.Clone();
                        draw.Dispose();
                        img1.Dispose();
                        pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                        pictureBox1.Image = imgview;
                    }
                    else
                    {
                        Bitmap img1 = new Bitmap((int)Math.Ceiling(imgview.Width * 0.9), (int)Math.Ceiling(imgview.Height * 0.9));
                        Graphics draw = Graphics.FromImage(img1);
                        draw.DrawImage(imgview, 0, 0, (int)Math.Ceiling(imgview.Width * 0.9), (int)Math.Ceiling(imgview.Height * 0.9));
                        imgview = (Bitmap)img1.Clone();
                        draw.Dispose();
                        img1.Dispose();
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        pictureBox1.Image = imgview;
                    }
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //private void toolStripButton6_Click(object sender, EventArgs e)//水平翻转功能
            {
                if (img != null)
                {
                    img.RotateFlip(RotateFlipType.Rotate180FlipY);

                    this.pictureBox1.Image = img;
                }
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //private void toolStripButton7_Click(object sender, EventArgs e)//垂直翻转
            {
                if (img != null)
                {
                    img.RotateFlip(RotateFlipType.Rotate180FlipX);
                    this.pictureBox1.Image = img;
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            //private void toolStripButton8_Click(object sender, EventArgs e)//反色处理
            {
                if (img != null)
                {
                    Color c = new Color();
                    for (int i = 0; i < img.Width; i++)
                        for (int j = 0; j < img.Height; j++)
                        {
                            c = img.GetPixel(i, j);
                            Color c1 = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                            img.SetPixel(i, j, c1);
                        }
                    this.pictureBox1.Image = img;
                }
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //private void toolStripButton9_Click(object sender, EventArgs e)//锐化处理
            {
                if (img != null)
                {
                    Color c = new Color();
                    Color c1 = new Color();
                    int rr, gg, bb;
                    for (int i = 1; i < img.Width; i++)
                        for (int j = 1; j < img.Height; j++)
                        {
                            c = img.GetPixel(i, j);
                            c1 = img.GetPixel(i - 1, j - 1);
                            rr = c.R + Math.Abs(c.R - c1.R) / 2;
                            gg = c.R + Math.Abs(c.R - c1.R) / 2;
                            bb = c.R + Math.Abs(c.R - c1.R) / 2;
                            if (rr > 255)
                                rr = 255;
                            if (gg > 255)
                                gg = 255;
                            if (bb > 255)
                                bb = 255;
                            Color c2 = Color.FromArgb(rr, gg, bb);
                            img.SetPixel(i, j, c2);
                        }
                    this.pictureBox1.Image = img;
                }
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //private void toolStripButton10_Click(object sender, EventArgs e)//马赛克处理
            {
                if (img != null)
                {
                    //Color c = new Color();
                    Color c1 = new Color();
                    int rr, gg, bb, kr, kc;
                    for (int i = 0; i < img.Width - 5; i += 5)
                        for (int j = 0; j < img.Height; j += 5)
                        {
                            rr = 0;
                            gg = 0;
                            bb = 0;
                            for (kc = 0; kc < 5; kc++)
                                for (kr = 0; kr < 5; kr++)
                                {
                                    c1 = img.GetPixel(i + kc, j + kr);
                                    rr += c1.R;
                                    gg += c1.G;
                                    bb += c1.B;
                                }
                            rr /= 25;
                            gg /= 25;
                            bb /= 25;
                            if (rr > 255)
                                rr = 255;
                            if (gg > 255)
                                gg = 255;
                            if (bb > 255)
                                bb = 255;
                            if (rr < 0)
                                rr = 0;
                            if (gg < 0)
                                gg = 0;
                            if (bb < 0)
                                bb = 0;
                            for (kc = 0; kc < 5; kc++)
                                for (kr = 0; kr < 5; kr++)
                                {
                                    Color cn = Color.FromArgb(rr, gg, bb);
                                    img.SetPixel(i + kc, j + kr, cn);
                                }
                        }
                    this.pictureBox1.Image = img;
                }
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            //private void toolStripButton11_Click(object sender, EventArgs e)//灰度处理
            {
                if (img != null)
                {
                    Color c1 = new Color();
                    int yy;
                    for (int i = 0; i < img.Width; i++)
                        for (int j = 0; j < img.Height; j++)
                        {
                            c1 = img.GetPixel(i, j);
                            yy = (int)(0.31 * c1.R + 0.59 * c1.G + 0.11 * c1.B);
                            if (yy > 255)
                                yy = 255;
                            Color cn = Color.FromArgb(yy, yy, yy);
                            img.SetPixel(i, j, cn);
                        }
                    this.pictureBox1.Image = img;
                }
            }


        }

        #region 图片旋转函数
        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="b">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <returns></returns>
        public Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);

            g.InterpolationMode = InterpolationMode.Bilinear;

            g.SmoothingMode = SmoothingMode.HighQuality;

            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();
            return dsImage;
        }
        #endregion 图片旋转函数

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Image img = pictureBox1.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox1.Image = img;

                //angle是角度

            }
            //if (theRectangle.X != 0 && theRectangle.Y != 0 && theRectangle.Height != 0)
            //{
            //    Bitmap bitmap = new Bitmap(pictureBox1.Image);
            //    Bitmap cloneBitmap = bitmap.Clone(theRectangle, PixelFormat.DontCare);
            //    pictureBox2.Image = (Image)cloneBitmap;
            //    //graphics.DrawImage(cloneBitmap, e.X, e.Y);
            //    Graphics g = pictureBox1.CreateGraphics();
            //    SolidBrush myBrush = new SolidBrush(Color.Transparent);
            //    g.FillRectangle(myBrush, theRectangle);
            //}
        }

        private void frmPicEdit_Load(object sender, EventArgs e)
        {
            //Form1 f = (Form1)this.Owner;
            //int id = f.SentData();
            //MonthlyReimbursementTableEntities mr = new MonthlyReimbursementTableEntities();
            //var aa = mr.MonthTable.Where(d => d.ID == id).FirstOrDefault();
            //Stream str1 = new MemoryStream(aa.Pictrue);
            //Image img1 = Image.FromStream(str1);

            //toolTip1.ShowAlways = true;
            //string Str1 = "双击可关闭";
            //toolTip1.SetToolTip(PicLook, Str1);
            //m_bmp = (Bitmap)img1;
            //m_ptCanvas = new Point(PicLook.Width / 2, PicLook.Height / 2);
            //m_ptBmp = new Point(-(m_bmp.Width / 2), -(m_bmp.Height / 2));
        }

        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        private void 度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //逆时针转
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipXY);
            pictureBox1.Refresh();
        }

        private void 顺90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //private void 顺时针转ToolStripMenuItem_Click(object sender, EventArgs e)
            {
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox1.Refresh();
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Focus();
            if (isMove)
            {
                int x, y;
                int moveX, moveY;
                moveX = Cursor.Position.X - mouseDownPoint.X;
                moveY = Cursor.Position.Y - mouseDownPoint.Y;
                x = pictureBox1.Location.X + moveX;
                y = pictureBox1.Location.Y + moveY;
                pictureBox1.Location = new Point(x, y);
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
            }
            if (cautimage == true)
            {
                ///
                if (this.Cursor == Cursors.Cross)
                {
                    this.p2 = new Point(e.X, e.Y);
                    if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) > 0)     //当鼠标从左上角向开始移动时P3坐标
                    {
                        this.p3 = new Point(p1.X, p1.Y);
                    }
                    if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) > 0)     //当鼠标从右上角向左下方向开始移动时P3坐标
                    {
                        this.p3 = new Point(p2.X, p1.Y);
                    }
                    if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) < 0)     //当鼠标从左下角向上开始移动时P3坐标
                    {
                        this.p3 = new Point(p1.X, p2.Y);
                    }
                    if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) < 0)     //当鼠标从右下角向左方向上开始移动时P3坐标
                    {
                        this.p3 = new Point(p2.X, p2.Y);
                    }
                    this.pictureBox1.Invalidate();  //使控件的整个图面无效，并导致重绘控件
                }
            }
            if (isDrag == true)
            {
                if (Track_move)
                    oldPoint = new Point(e.X, e.Y);
                else
                {
                    return;
                }
                theRectangle = new Rectangle(startPoint.X, startPoint.Y, oldPoint.X - startPoint.X, oldPoint.Y - startPoint.Y);

                Rectangle tempr = new Rectangle(theRectangle.X, theRectangle.Y, theRectangle.Width + 2, theRectangle.Height + 2);
                this.Invalidate(tempr);

            }

            //cut line color
            if (this.Cursor == Cursors.Cross)
            {
                this.p2 = new Point(e.X, e.Y);
                this.pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            int ow = pictureBox1.Width;
            int oh = pictureBox1.Height;
            int VX, VY;
            if (e.Delta > 0)
            {
                pictureBox1.Width += zoomStep;
                pictureBox1.Height += zoomStep;

                PropertyInfo pInfo = pictureBox1.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                    BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pictureBox1, null);

                pictureBox1.Width = rect.Width;
                pictureBox1.Height = rect.Height;
            }
            if (e.Delta < 0)
            {

                if (pictureBox1.Width < myBmp.Width / 10)
                    return;

                pictureBox1.Width -= zoomStep;
                pictureBox1.Height -= zoomStep;
                PropertyInfo pInfo = pictureBox1.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                    BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(pictureBox1, null);
                pictureBox1.Width = rect.Width;
                pictureBox1.Height = rect.Height;
            }

            VX = (int)((double)x * (ow - pictureBox1.Width) / ow);
            VY = (int)((double)y * (oh - pictureBox1.Height) / oh);
            pictureBox1.Location = new Point(pictureBox1.Location.X + VX, pictureBox1.Location.Y + VY);
            box1_xlcX = pictureBox1.Location.X + VX;
            box1_xlcY = pictureBox1.Location.Y + VY;


        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDrag == false)
            {
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
                isMove = true;
                pictureBox1.Focus();
                //如果开始绘制，则开始记录鼠标位置



            }
            if (cautimage == true)
            {
                this.Cursor = Cursors.Cross;
                this.p1 = new Point(e.X, e.Y);
                x1 = e.X;
                y1 = e.Y;
                if (this.pictureBox1.Image != null)
                {
                    HeadImageBool = true;
                }
                else
                {
                    HeadImageBool = false;
                }

            }
            if (e.Button == MouseButtons.Left && isDrag == true)
            {
                // startPoint = new Point(Cursor.Position.X, Cursor.Position.Y);
                startPoint = new Point(e.X, e.Y);
                oldPoint = new Point(e.X, e.Y);
                Track_move = true;
                return;

            }
            Track_move = false;


            //cut line color

            this.Cursor = Cursors.Cross;
            this.p1 = new Point(e.X, e.Y);


        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    isMove = false;
                }
                if (cautimage == true)
                {
                    ///
                    if (HeadImageBool)
                    {
                        width = this.pictureBox1.Image.Width;
                        heigth = this.pictureBox1.Image.Height;
                        if ((e.X - x1) > 0 && (e.Y - y1) > 0)   //当鼠标从左上角向右下方向开始移动时发生
                        {
                            IC1 = new ImageCut1(x1, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));    //实例化ImageCut1类
                        }
                        if ((e.X - x1) < 0 && (e.Y - y1) > 0)   //当鼠标从右上角向左下方向开始移动时发生
                        {
                            IC1 = new ImageCut1(e.X, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                        }
                        if ((e.X - x1) > 0 && (e.Y - y1) < 0)   //当鼠标从左下角向右上方向开始移动时发生
                        {
                            IC1 = new ImageCut1(x1, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                        }
                        if ((e.X - x1) < 0 && (e.Y - y1) < 0)   //当鼠标从右下角向左上方向开始移动时发生
                        {
                            IC1 = new ImageCut1(e.X, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));      //实例化ImageCut1类
                        }
                        this.pictureBox2.Width = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Width;
                        this.pictureBox2.Height = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Height;
                        this.pictureBox2.Image = IC1.KiCut1((Bitmap)(this.pictureBox1.Image));
                        this.Cursor = Cursors.Default;
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                if (isDrag == true && e.Button == MouseButtons.Left && Track_move == true)
                {
                    Track_move = false;
                    oldPoint = new Point(e.X, e.Y);


                    theRectangle = new Rectangle(startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                    // theRectangle = new Rectangle(startPoint.X, startPoint.Y, oldPoint.X - startPoint.X, oldPoint.Y - startPoint.Y);
                    Rectangle rectorg = new Rectangle(borg.X, borg.Y, img.Width, img.Height);
                    if (theRectangle.Width <= 0)
                        return;
                    if (theRectangle.Height <= 0)
                        return;
                    if (rectorg.Contains(theRectangle))
                    {
                        //if (e.X - startPoint.X != 0 && e.Y - startPoint.Y != 0)
                        {
                            Bitmap pic = new Bitmap(fullname);
                            Rectangle rectadj = new Rectangle(theRectangle.X - borg.X, theRectangle.Y - borg.Y, theRectangle.Width, theRectangle.Height);
                            Bitmap myBitmap = (Bitmap)pictureBox1.Image.Clone();

                            Bitmap p111ic = GetRect(pic, theRectangle);
                            pictureBox1.Image = p111ic;
                            pictureBox1.Width = p111ic.Width;
                            pictureBox1.Height = p111ic.Height;
                            isDrag = false;




                            //Bitmap bit = new Bitmap(theRectangle.Width, theRectangle.Height);
                            //using (Graphics g = Graphics.FromImage(bit))
                            //{
                            //    g.DrawImage(pictureBox1.Image, new Rectangle(0, 0, theRectangle.Width, theRectangle.Height), new Rectangle(startPoint.X, startPoint.Y, theRectangle.Width, theRectangle.Height), GraphicsUnit.Pixel);
                            //    //  bit.Save(saveBitmapPath + "\\" + frame + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            //    // bit.Save("C:\\Users\\IBM_ADMIN\\Desktop\\1" + ".bmp", ImageFormat.Bmp);
                            //    // bit.Save("C:\\Users\\IBM_ADMIN\\Desktop" + "\\1" + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            //    //   bit.Dispose();
                            //}
                            //pictureBox1.Image = (Image)bit;

                            //  CloneImage(startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                        }
                    }
                    this.Invalidate();
                }

                //cut line color
                //this.Cursor = Cursors.Default;
                //this.p2 = new Point(e.X, e.Y);

            }
            catch (Exception ex)
            {
                return;

                throw;
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
                isMove = true;
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMove = false;
            }

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            panel2.Focus();
            if (isMove)
            {
                int x, y;
                int moveX, moveY;
                moveX = Cursor.Position.X - mouseDownPoint.X;
                moveY = Cursor.Position.Y - mouseDownPoint.Y;
                x = pictureBox1.Location.X + moveX;
                y = pictureBox1.Location.Y + moveY;
                pictureBox1.Location = new Point(x, y);
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
            }
        }

        private void Erweima()
        {
            //if (txtEncodeData.Text.Trim() == String.Empty)
            //{
            //    MessageBox.Show("Data must not be empty.");
            //    return;
            //}

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            String encoding = "Byte";
            if (encoding == "Byte")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16("4");
                qrCodeEncoder.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid size!");
                return;
            }
            try
            {
                int version = Convert.ToInt16("7");
                qrCodeEncoder.QRCodeVersion = version;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid version !");
            }

            string errorCorrect = "M";
            if (errorCorrect == "L")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            Image image;
            String data = "www.baidu.com";
            image = qrCodeEncoder.Encode(data);
            erweima_image = image;

            //pb_view.Image = image;
        }

        private void WriteBitmapToFile(string source, string target)
        {
            Bitmap bitmap = new Bitmap(Image.FromFile(source));
            bitmap.Save(target, ImageFormat.Png);
        }

        private string rightPicture;
        public string RightPicture
        {
            get
            {
                return this.rightPicture;
            }
            set
            {
                this.rightPicture = value;
                this.OnPropertyChanged("RightPicture");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }


        public Image CombinImage(string sourceImg, string destImg)
        {
            Image imgBack = System.Drawing.Image.FromFile(pic2);     //相框图片 
            Image img = System.Drawing.Image.FromFile(fullname);        //照片图片


            #region MyRegion
            //Bitmap imgTemp = new System.Drawing.Bitmap(imgBack.Width + img.Width, imgBack.Height >= img.Height ? imgBack.Height : img.Height, PixelFormat.Format24bppRgb);
            ////从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            //Graphics g = Graphics.FromImage(imgTemp);

            //g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      // g.DrawImage(imgBack, 0, 0, 相框宽, 相框高); 
            ////g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框

            ////g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            //g.DrawImage(img, imgBack.Width, 0, img.Width, img.Height);
            //GC.Collect();
            //return imgTemp;

            #endregion
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics    
            //Bitmap newimgview = new Bitmap(imgBack.Width, imgBack.Height);
            //g.DrawImage(imgBack, 0, 0, 148, 124);
            //g.DrawImage(imgBack, 0, 0, pictureBox2.Width, pictureBox2.Height);      // g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);            
            //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框       

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);

            int p2x = pictureBox2.Width;
            int p2y = pictureBox2.Height;
            int p1x = pictureBox1.Width;
            int p1y = pictureBox1.Height;
            #region 获取当前坐标
            int x, y;
            int moveX, moveY;
            moveX = Cursor.Position.X - mouseDownPoint.X;
            moveY = Cursor.Position.Y - mouseDownPoint.Y;
            x = pictureBox1.Location.X + moveX;
            y = pictureBox1.Location.Y + moveY;
            //   pictureBox1.Location = new Point(x, y);

            int Fwidth = 0;
            int Fheight = 0;

            x = (int)(imgBack.Width - Fwidth) / 2;
            y = (int)(imgBack.Height - Fheight) / 2;


            Color Fcolor = System.Drawing.Color.Red;
            FontStyle Fstyle = FontStyle.Regular;
            FontFamily a = FontFamily.GenericMonospace;
            float Fsize = 18;
            System.Drawing.Image image = Image.FromFile(pic2);
            //System.Drawing.Graphics e = System.Drawing.Graphics.FromImage(image);
            //Graphics e = Graphics.FromImage(image);
            //Bitmap originalBmp = new (Bitmap)Image.FromFile("YourFileName.gif");
            #region MyRegion
            //using (Image lima = Image.FromFile(pic2))
            //{
            //    //如果原图片是索引像素格式之列的，则需要转换
            //    if (IsPixelFormatIndexed(lima.PixelFormat))
            //    {
            //        Bitmap bmp = new Bitmap(lima.Width, lima.Height, PixelFormat.Format32bppArgb);
            //        using (Graphics gg = Graphics.FromImage(bmp))
            //        {
            //            gg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //            gg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //            gg.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //            gg.DrawImage(lima, 0, 0);
            //            gg.DrawImage(img, p2x - p1x, p2y - p1y, p1x, p1y);

            //        }
            //        //下面的水印操作，就直接对 bmp 进行了
            //        //......
            //    }
            //    else //否则直接操作
            //    {
            //        //直接对img进行水印操作
            //    }

            //    GC.Collect();
            //    // return lima;
            //} 
            #endregion
            //Bitmap tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);
            //byte[] imgData = { pic2, 1, 1, 1, 1, 1, };

            //using (System.Drawing.Image img121 = System.Drawing.Image.FromStream(new MemoryStream(imgData)))
            //{
            //    for (int i = 1; i <= 1000; i++)
            //    {
            //        Bitmap img1 = new Bitmap(new Bitmap(img121));
            //        RectangleF rectf = new RectangleF(800, 550, 200, 200);
            //        Graphics g = Graphics.FromImage(img1);
            //        g.DrawString(i.ToString("0000"), new Font("Thaoma", 30), Brushes.Black, rectf);
            //        img1.Save(@"E:\Img\" + i.ToString("0000") + ".tif");
            //        g.Flush();
            //        g.Dispose();
            //        img1.Dispose();
            //        GC.Collect();
            //    }
            //}
            #region ceshi

            Graphics g = Graphics.FromImage(img);

            //using (Image sourceImage = Image.FromFile(pic2))
            using (Image sourceImage = System.Drawing.Image.FromFile(pic2))
            {
                ////判断原图片是否是GIF图片
                if (sourceImage.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                {
                    Bitmap bmp = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);
                    using (Graphics ggg = Graphics.FromImage(bmp))
                    {
                        ////提高图片质量
                        ggg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        ggg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        ggg.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        //ggg.DrawImage(sourceImage, 0, 0);
                        g = ggg;
                        ggg.DrawImage(img, 270, 170, 612, 573);
                        return bmp;
                    }
                    g = Graphics.FromImage(bmp);
                    ////现在的 bmp就代替了原来的gif图片，下面的操做，就全部针对这个 bmp 进行就是了
                    g.DrawImage(img, 270, 170, 612, 573);


                    //Image tmp = (Image)sourceImage;

                    //Bitmap tmpbitmap = new Bitmap(tmp);

                    //Image image1 = tmpbitmap;
                    //image1.Save("C:\\Users\\IBM_ADMIN\\Desktop\\相片批量处理\\1\\" + ".Jpeg", ImageFormat.Jpeg);
                    return bmp;
                }
                GC.Collect();
                return sourceImage;
            }

            #endregion

            ////good use
            #endregion


            return imgBack;
        }

        #region 判断数据异常情况，
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,
PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
PixelFormat.Format8bppIndexed
    };
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }

        #endregion

        private Bitmap MergerImg(params Bitmap[] maps)
        {


            int X = 0;
            int Y = 0;
            Bitmap newImg;
            for (int i = 0; i < maps.Length; i++)
            {
                //if (radioButton1.Checked) //左右合并
                {
                    X += maps[i].Width;
                    Y = Y > maps[i].Height ? Y : maps[i].Height;
                }
                //else                      //上下合并
                {

                    //X = X > maps[i].Width ? X : maps[i].Width;
                    //Y += maps[i].Height;
                }
            }
            newImg = new Bitmap(X, Y);
            X = 0;
            Y = 0;
            Graphics g = Graphics.FromImage(newImg);
            g.Clear(System.Drawing.Color.White);
            for (int i = 0; i < maps.Length; i++)
            {
                g.DrawImage(maps[i], X, Y, maps[i].Width, maps[i].Height);
                //if (radioButton1.Checked) //左右合并
                {
                    X += maps[i].Width;
                }
                //else
                {
                    //Y += maps[i].Height;
                }
            }
            g.Dispose();
            return newImg;
        }


        //原理：复制下层PictureBox被上层PictureBox挡住的部分，赋给上层PictureBox
        private void Transparent(PictureBox pbFore, PictureBox pbBack)
        {
            pbFore.Visible = false;
            // 生成一个与PictureBox一样大的缓冲区
            Bitmap buffer = new Bitmap(pbFore.Width, pbFore.Height);
            // 创建图形对象，绘图区域为刚刚生成的缓冲区
            Graphics g = Graphics.FromImage(buffer);
            // 背景上被PictureBox覆盖的区域区域。假定PictureBox覆盖在背景图之上
            Rectangle scrRect = new Rectangle(
                pbFore.Left - pbBack.Left,            // 截图区域的左上角X坐标
                pbFore.Top - pbBack.Top,              // 截图区域的左上角Y坐标
                pbFore.Width - (pbFore.Left - pbBack.Left),                          // 截图区域的宽度
                pbFore.Height - (pbFore.Top - pbBack.Top));                        // 截图区域的高度
            // 将背景上被PictureBox覆盖的那块区域截图下来，保存在缓冲区中
            if (pbBack.Image == null)
                pbBack.Image = new Bitmap(1, 1);
            if (pbFore.Image == null)
                pbFore.Image = new Bitmap(1, 1);
            g.DrawImage(pbBack.Image,               // 背景图
                0,                                  // 所截图片画在缓冲区中的左上角X坐标
                0,                                  // 所截图片画在缓冲区中的左上角Y坐标
                scrRect,                            // 需要截图的区域
                GraphicsUnit.Pixel);                // 此参数表示本次调用传入的参数以像素为单位
            // 指定被当做透明色的颜色值范围
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Color.FromArgb(0, 0, 0), Color.FromArgb(255, 255, 255));
            // 将PictureBox的Image拷贝到Bitmap中，指定的颜色当做透明色
            g.DrawImage(pbFore.Image,                               // 将PictureBox的图片绘制在缓冲区中
                new Rectangle(0, 0, pbFore.Width, pbFore.Height),   // 绘制区域，将PictureBox的图片按比例缩放绘制在这个区域内
                0,                                                  // 截图区域的左边坐标，PictureBox必须覆盖为背景图之上
                0,                                                  // 截图区域的上边坐标
                pbFore.Width,                                       // 截图区域的宽度
                pbFore.Height,                                      // 截图区域的高度
                GraphicsUnit.Pixel,                                 // 此参数表示本次调用传入的参数以像素为单位
                imageAttr);                                         // 透明色
            pbFore.Image = buffer;
            pbFore.Visible = true;
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  cautimage = true;
            isDrag = true;
            isMove = false;
            var form = new frmCutImage(fullname, img1);
            //var form = new frmCut2(fullname, img1);

            if (form.ShowDialog() == DialogResult.OK)
            {

                img = form.image1;
                pictureBox1.Image = (Image)img;
            }
        }

        private void 图片特效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (listBox1.Items.Count != 0)
            {
                frmSpecialEfficacy special = new frmSpecialEfficacy();
                special.ig = pictureBox1.Image;
                special.ShowDialog();
            }
        }

        private void 图片调节ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPicAdjust picadjust = new frmPicAdjust();
            picadjust.ig = pictureBox1.Image;
            picadjust.PicOldPath = FPath;
            picadjust.ShowDialog();
        }

        private void 图片文字ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                frmWater water = new frmWater();
                water.ig = pictureBox1.Image;
                water.FPath = fullname;
                water.ShowDialog();
            }

        }

        private void 幻灯片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog2.SelectedPath;
                frmSlide slide = new frmSlide();
                slide.Ppath = path;
                slide.ShowDialog();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(pictureBox1.Image, 20, 20);
            e.Graphics.DrawImage(this.pictureBox1.Image, new Rectangle(0, 0, Width * 80 / 100, Height * 95 / 100));
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog MyPrintDg = new PrintDialog();
            MyPrintDg.Document = printDocument1;
            if (MyPrintDg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                }
                catch
                {   //停止打印
                    printDocument1.PrintController.OnEndPrint(printDocument1, new System.Drawing.Printing.PrintEventArgs());

                }
            }
        }

        private void 自适应ToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        public void OpenImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有图像文件 | *.bmp; *.pcx; *.png; *.jpg; *.gif;" +
                   "*.tif; *.ico; *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf|" +
                   "位图( *.bmp; *.jpg; *.png;...) | *.bmp; *.pcx; *.png; *.jpg; *.gif; *.tif; *.ico|" +
                   "矢量图( *.wmf; *.eps; *.emf;...) | *.dxf; *.cgm; *.cdr; *.wmf; *.eps; *.emf";
            ofd.ShowHelp = true;
            ofd.Title = "打开图像文件";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                curFileName = ofd.FileName;
                try
                {
                    curBitmap = (Bitmap)System.Drawing.Image.FromFile(curFileName);
                    pathString.Add(curFileName);
                    srcImage.Add(new Bitmap(curBitmap));
                    if (curBitmap.Width >= curBitmap.Height)
                        temp = new Bitmap(curBitmap, new Size(100, (int)(100 * curBitmap.Height / curBitmap.Width)));
                    else
                        temp = new Bitmap(curBitmap, new Size((int)(100 * curBitmap.Width / curBitmap.Height), 100));
                    thumbnailImage.Add(new Bitmap(temp));
                    count++;
                }
                catch (Exception exp)
                { MessageBox.Show(exp.Message); }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int ddd = listView1.SelectedIndices[0];
                ShowImage(dailyResult[this.listView1.SelectedItems[0].Index].mark1);
                comboxi = ddd;
                comboxiname = dailyResult[this.listView1.SelectedItems[0].Index].mark1.ToString();

            }
        }

        private void CreateMyListView()
        {

            ListView listView1 = new ListView();       //声明一个ListView控件。 

            listView1.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));

            listView1.View = View.Details;           //将view属性设为Details。

            listView1.LabelEdit = true;              //允许用户编辑文本项。

            listView1.AllowColumnReorder = true;     //允许用户重排列。

            listView1.CheckBoxes = true;            //显示check boxes。

            listView1.FullRowSelect = true;          //允许选择项及其子项。

            listView1.GridLines = true;              //显示行列的网格线。

            listView1.Sorting = SortOrder.Ascending;   //所列项按升序自动排序。

            //用指定的项文本和项图标的图像索引位置初始化ListViewItem类的新实例。

            //图像从零开始索引，该图像位于与包含该项的ListView关联的ImageList中。

            ListViewItem item1 = new ListViewItem("item1", 0);

            item1.Checked = true;                  //item1被选中。

            //SubItems类获取包含该项的所有子项的集合，Add方法向集合中添加单个子项，

            //子项的顺序决定ListView控件中显示子项的列。

            item1.SubItems.Add("1");

            item1.SubItems.Add("2");

            item1.SubItems.Add("3");

            ListViewItem item2 = new ListViewItem("item2", 1);

            item2.SubItems.Add("4");

            item2.SubItems.Add("5");

            item2.SubItems.Add("6");

            ListViewItem item3 = new ListViewItem("item3", 2);

            item3.Checked = true;

            item3.SubItems.Add("7");

            item3.SubItems.Add("8");

            item3.SubItems.Add("9");

            //产生项和子项的列，Add方法往列添加3个参数：列表头，初始宽度，对齐方式。

            listView1.Columns.Add("Item Column", -2, HorizontalAlignment.Left);

            listView1.Columns.Add("Column 2", -2, HorizontalAlignment.Left);

            listView1.Columns.Add("Column 3", -2, HorizontalAlignment.Left);

            listView1.Columns.Add("Column 4", -2, HorizontalAlignment.Center);

            //Add the items to the ListView.

            listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });

            ImageList imageListSmall = new ImageList();   //产生图像对象

            //Initialize the ImageList objects with bitmaps.

            imageListSmall.Images.Add(Bitmap.FromFile(@"D:\Dev Project\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\Forward.bmp"));

            imageListSmall.Images.Add(Bitmap.FromFile(@"D:\Dev Project\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\NewFolder.bmp"));

            imageListSmall.Images.Add(Bitmap.FromFile(@"D:\Dev Project\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\Up.bmp"));

            //Assign the ImageList objects to the ListView.

            listView1.SmallImageList = imageListSmall;

            //Add the ListView to the control collection.

            this.Controls.Add(listView1);

        }
        private void CreateMyListView1()
        {
            //ColumnHeader ch = new ColumnHeader();

            //ch.Text = "列标题1";   //设置列标题

            //ch.Width = 120;    //设置列宽度

            //ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式

            //this.listView1.Columns.Add(ch);    //将列头添加到ListView控件。


            ////this.listView1.Columns.Add("列标题1", 120, HorizontalAlignment.Left); //一步添加

            //this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            //for (int i = 0; i < 10; i++)   //添加10行数据
            //{
            //    ListViewItem lvi = new ListViewItem();

            //    lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标

            //    lvi.Text = "subitem" + i;

            //    lvi.SubItems.Add("第2列,第" + i + "行");

            //    lvi.SubItems.Add("第3列,第" + i + "行");

            //    this.listView1.Items.Add(lvi);
            //}

            //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。



            //foreach (ListViewItem item in this.listView1.Items)
            //{
            //    for (int i = 0; i < item.SubItems.Count; i++)
            //    {
            //       // MessageBox.Show(item.SubItems[i].Text);
            //    }
            //}


            //foreach (ListViewItem lvi in listView1.SelectedItems)  //选中项遍历
            //{
            //    listView1.Items.RemoveAt(lvi.Index); // 按索引移除
            //    //listView1.Items.Remove(lvi);   //按项移除
            //}

            //listView1.View = View.LargeIcon;     
            ImageList imgList = new ImageList();

            //imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高
            string paaa = @"D:\Dev Project\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\Hall.ico";

            imgList.Images.Add(Image.FromFile(paaa));
            //listView1.Items.Add(paaa);
            //listView1.Items[0].ImageIndex =0 ;
            //listView1.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大


            //this.listView1.Clear();  //从控件中移除所有项和列（包括列表头）。

            //this.listView1.Items.Clear();  //只移除所有的项。
            listView1.View = View.LargeIcon;
            listView1.LargeImageList = imgList;

            for (int i = 0; i < 4; i++)
            {
                listView1.Items.Add("图片" + (i + 1));
                listView1.Items[i].ImageIndex = i;
            }

        }

        #region cut
        public Bitmap GetRect(Image pic, Rectangle Rect)
        {
            if (Rect.Width == 0 || Rect.Height == 0)
                return null;

            //创建图像
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }
        private void CloneImage(float x, float y, float width, float height)
        {

            {
                //获取图像
                //Bitmap myBitmap = new Bitmap(fullname);

                Bitmap myBitmap = (Bitmap)pictureBox1.Image.Clone();



                //设定图像剪切区域
                RectangleF cloneRect = new RectangleF(x, y, width, height);
                PixelFormat format = myBitmap.PixelFormat;

                Bitmap cloneBitmap = myBitmap.Clone(cloneRect, format);

                //   this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

                this.pictureBox1.Image = cloneBitmap;
            }
        }
        #endregion

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Pen p = new Pen(Color.Black, 1);//画笔
            //p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //Rectangle rect = new Rectangle(p1, new Size(p2.X - p1.X, p2.Y - p1.Y));
            //e.Graphics.DrawRectangle(p, rect);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {

            savefilepath = "";

            var form = new frmScanMain();

            if (form.ShowDialog() == DialogResult.OK)
            {
                savefilepath = form.savefilepath;

            }
            SavePIC();
        }

        private void SavePIC()
        {
            if (savefilepath != "")
            {
                clsAllnew buline = new clsAllnew();

                List<string> filename = buline.GetBy_CategoryReportFileName(savefilepath);

                List<clsAccFileinfo> accFile_Result = new List<clsAccFileinfo>();
                for (int i = 0; i < filename.Count; i++)
                {
                    clsAccFileinfo temp = new clsAccFileinfo();
                    if (i != 0)
                        temp.mark1 += "," + filename[i];
                    else
                        temp.mark1 += filename[i];
                    temp.File_name = System.IO.Path.GetFileName(temp.mark1);
                    temp.accfile_id = dailyResult[0].accfile_id;
                    string serverimg = temp.mark1.Replace(temp.mark1 + "\\", "");
                    string copyToPath = clsCommHelp.LocationImagePath(temp);
                    File.Copy(temp.mark1.Replace(",", ""), copyToPath, true);
                    temp.mark1 = copyToPath;
                    accFile_Result.Add(temp);
                }
                //更新 文档信息
                List<clsFile_Managermentinfo> File_Result = new List<clsFile_Managermentinfo>();
                selcetitem.yeshu = accFile_Result.Count.ToString();

                File_Result.Add(selcetitem);
                if (selcetitem != null)
                {
                    buline.Update_File_detail_Server(File_Result);
                }
                buline.InsteraccFile_Server(accFile_Result);
                InitializeDataSource(dailyResult[0].accfile_id);
            }
        }


        #region listView1
        // 初始化listView1.  
        private void InitializeListView()
        {
            listView1.AllowDrop = true;
            listView1.ListViewItemSorter = new ListViewIndexComparer();
            //初始化插入标记  
            listView1.InsertionMark.Color = Color.Red;
            //  
            listView1.ItemDrag += listView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragLeave += listView1_DragLeave;
            listView1.DragDrop += listView1_DragDrop;
        }

        // 当一个项目拖拽是启动拖拽操作  
        void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Dictionary<ListViewItem, int> itemsCopy = new Dictionary<ListViewItem, int>();
            foreach (ListViewItem item in listView1.SelectedItems)
                itemsCopy.Add(item, item.Index);
            listView1.DoDragDrop(itemsCopy, DragDropEffects.Move);

            reselect();
        }

        void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        //像拖拽项目一样移动插入标记  
        void listView1_DragOver(object sender, DragEventArgs e)
        {
            // 获得鼠标坐标  
            Point point = listView1.PointToClient(new Point(e.X, e.Y));
            // 返回离鼠标最近的项目的索引  
            int index = listView1.InsertionMark.NearestIndex(point);
            // 确定光标不在拖拽项目上  
            if (index > -1)
            {
                Rectangle itemBounds = listView1.GetItemRect(index);
                if (point.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    listView1.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    listView1.InsertionMark.AppearsAfterItem = false;
                }
            }
            listView1.InsertionMark.Index = index;
        }

        // 当鼠标离开控件时移除插入标记  
        void listView1_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1;
        }

        // 将项目移到插入标记所在的位置  
        void listView1_DragDrop(object sender, DragEventArgs e)
        {
            // 返回插入标记的索引值  
            int index = listView1.InsertionMark.Index;
            // 如果插入标记不可见，则退出.  
            if (index == -1)
            {
                return;
            }
            // 如果插入标记在项目的右面，使目标索引值加一  
            if (listView1.InsertionMark.AppearsAfterItem)
            {
                index++;
            }

            // 返回拖拽项  
            Dictionary<ListViewItem, int> items = (Dictionary<ListViewItem, int>)e.Data.GetData(typeof(Dictionary<ListViewItem, int>));
            foreach (var item in items)
            {
                //在目标索引位置插入一个拖拽项目的副本   
                listView1.Items.Insert(index, (ListViewItem)item.Key.Clone());
                // 移除拖拽项目的原文件  
                listView1.Items.Remove(item.Key);
                if (item.Value >= index) index++;
            }
        }

        // 对ListView里的各项根据索引进行排序  
        class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }

        private void reselect()
        {
            int index = 1;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Text = index.ToString();
                index++;

            }
        }

        #endregion

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dailyResult.RemoveAt(comboxi);

            // filename.RemoveAt(comboxi);
            listView1.Items.RemoveAt(comboxi);
            listView1.Items.Clear();
            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.deleteaccFil(dailyResult[0].accfile_id);
            BusinessHelp.InsteraccFile_Server(dailyResult);

            InitializeDataSource(dailyResult[0].accfile_id);
        }

    }


}
