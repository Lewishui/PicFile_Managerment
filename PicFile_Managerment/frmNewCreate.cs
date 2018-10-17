using FA.Buiness;
using FA.Common;
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
        List<string> filename = new List<string>();

        DataRow tree_Current_row;

        public frmNewCreate(string ty, DataRow tree_Current_row1)
        {
            InitializeComponent();
            tree_Current_row = tree_Current_row1;

            textBox7.Text = tree_Current_row["Description"].ToString();


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
                iitem.miji = textBox6.Text;
                iitem.wenjianleibie = textBox7.Text;
                iitem.NodeID = tree_Current_row["NodeID"].ToString();

                iitem.yeshu = textBox8.Text;
                iitem.fenshu = textBox9.Text;
                string ACCid = clsCommHelp.RandomID();


                iitem.accfile_id = ACCid;
                List<clsAccFileinfo> accFile_Result = new List<clsAccFileinfo>();
                for (int i = 0; i < filename.Count; i++)
                {
                    clsAccFileinfo temp = new clsAccFileinfo();

                    if (i != 0)
                        temp.mark1 += "," + filename[i];
                    else
                        temp.mark1 += filename[i];

                    temp.accfile_id = ACCid;
                    accFile_Result.Add(temp);

                }

                iitem.beizhu = textBox11.Text;

                File_Result.Add(iitem);


                clsAllnew BusinessHelp = new clsAllnew();

                BusinessHelp.InsterFile_detail_Server(File_Result);
                BusinessHelp.InsteraccFile_Server(accFile_Result);




                MessageBox.Show("成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误！" + ex.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw;
            }
        }


        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = new List<string>();

            OpenFileDialog tbox = new OpenFileDialog();
            tbox.Multiselect = false;
            tbox.Filter = "所有文件|*.*";
            tbox.Multiselect = true;
            tbox.SupportMultiDottedExtensions = true;
            if (tbox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Clear();

                foreach (string s in tbox.SafeFileNames)
                {
                    filename.Add(tbox.FileName);

                    listBox1.Items.Add(s);
                }
            }
        }
    }
}
