using FA.Buiness;
using FA.DB;
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
    public partial class frmNewCreate : Form
    {

        public List<clsFile_Managermentinfo> File_Result;

        public frmNewCreate(string ty)
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                File_Result = new List<clsFile_Managermentinfo>();
                clsFile_Managermentinfo iitem = new clsFile_Managermentinfo();
                //iitem.T_id = "1";
                iitem.wenjianbiaohao = this.txname.Text;
                iitem.biaoti = textBox1.Text;
                iitem.wenhao = textBox2.Text;
                iitem.zhiwendanwei = textBox3.Text;
                iitem.xingwendanwei = textBox4.Text;
                iitem.dengjiriqi = this.dateTimePicker1.Value.AddDays(0).Date.ToString("MM/dd/yyyy"); ;
                iitem.miji =textBox6.Text;
                iitem.wenjianleibie =textBox7.Text;
                iitem.yeshu = textBox8.Text;
                iitem.fenshu =textBox9.Text;
                iitem.accfile_id =textBox10.Text;
                iitem.beizhu = textBox11.Text;

                File_Result.Add(iitem);


                clsAllnew BusinessHelp = new clsAllnew();

                BusinessHelp.InsterFile_detail_Server(File_Result);

                MessageBox.Show("成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误！" + ex.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw;
            }
        }
    }
}
