using FA.Buiness;
using FA.Common;
using FA.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        int comboxi;
        string comboxiname;

        DataRow tree_Current_row;
        clsFile_Managermentinfo iitem1;
        public frmNewCreate(string ty, DataRow tree_Current_row1, clsFile_Managermentinfo iitem)
        {
            InitializeComponent();
            iitem1 = new clsFile_Managermentinfo();
            iitem1 = iitem;

            tree_Current_row = tree_Current_row1;
            if (iitem == null)
                textBox7.Text = tree_Current_row["Description"].ToString();
            else
            {
                this.txname.Text = iitem.wenjianbiaohao;
                textBox1.Text = iitem.biaoti;
                textBox2.Text = iitem.wenhao;
                textBox3.Text = iitem.zhiwendanwei;
                textBox4.Text = iitem.xingwendanwei;
                this.dateTimePicker1.Value = Convert.ToDateTime(iitem.dengjiriqi);
                textBox6.Text = iitem.miji;
                textBox7.Text = tree_Current_row["Description"].ToString();
                textBox8.Text = iitem.yeshu;
                textBox9.Text = iitem.fenshu;


                clsAllnew BusinessHelp = new clsAllnew();
                List<clsAccFileinfo> dailyResult = new List<clsAccFileinfo>();
                string conditions = "";
                conditions = "select * from AccFile where accfile_id='" + iitem.accfile_id + "'";
                dailyResult = BusinessHelp.find_ACCFile(conditions);
                listBox1.Items.Clear();
                filename = new List<string>();
                foreach (clsAccFileinfo s in dailyResult)
                {
                    filename.Add(s.mark1);
                    listBox1.Items.Add(System.IO.Path.GetFileName(s.mark1));
                }
            }
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
                    temp.File_name = System.IO.Path.GetFileName(temp.mark1);
                    temp.accfile_id = ACCid;
                   // var strs = System.IO.Directory.GetFiles(temp.mark1.Replace(System.IO.Path.GetFileName(temp.mark1), "")).Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("gif") || file.ToLower().EndsWith("jpeg") || file.ToLower().EndsWith("png")).ToList();
                    //foreach (string file in strs)
                    {
                        //System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        string serverimg = temp.mark1.Replace(temp.mark1 + "\\", "");
                        string copyToPath = clsCommHelp.LocationImagePath(temp);
                        //if (File.Exists(copyToPath))
                        {

                            File.Copy(temp.mark1.Replace(",",""), copyToPath, true);
                        }
                        temp.mark1 = copyToPath;
                    }
                    accFile_Result.Add(temp);

                }

                iitem.beizhu = textBox11.Text;
                if (iitem1 != null)
                    iitem.T_id = iitem1.T_id;
                File_Result.Add(iitem);
                clsAllnew BusinessHelp = new clsAllnew();
                if (iitem1 != null)
                {
                    BusinessHelp.Update_File_detail_Server(File_Result);

                    BusinessHelp.deleteaccFil(ACCid);
                }
                else
                {
                    BusinessHelp.InsterFile_detail_Server(File_Result);
                  
                }
                  BusinessHelp.InsteraccFile_Server(accFile_Result);
                MessageBox.Show("成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误！" + ex.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                throw;
            }
        }


        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename == null || filename.Count == 0)
            {
                filename = new List<string>();
                listBox1.Items.Clear();
            }
            OpenFileDialog tbox = new OpenFileDialog();
            tbox.Multiselect = false;
            tbox.Filter = "所有文件|*.*";
            tbox.Multiselect = true;
            tbox.SupportMultiDottedExtensions = true;
            if (tbox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                foreach (string s in tbox.SafeFileNames)
                {
                    filename.Add(tbox.FileName);

                    listBox1.Items.Add(s);
                }
            }
        }

        private void 删除所选ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            filename.RemoveAt(comboxi);
            listBox1.Items.Remove(comboxi);
            listBox1.Items.Clear();
            foreach (string s in filename)
            {
                listBox1.Items.Add(System.IO.Path.GetFileName(s));
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                comboxi = this.listBox1.SelectedIndex;
                comboxiname = this.listBox1.SelectedItem.ToString();
            }
        }
    }
}
