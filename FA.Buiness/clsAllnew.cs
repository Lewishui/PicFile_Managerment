using clsCommon;
using FA.Common;
using FA.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FA.Buiness
{

    public class clsAllnew
    {
        private BackgroundWorker bgWorker1;
        //private object missing = System.Reflection.Missing.Value;
        public ToolStripProgressBar pbStatus { get; set; }
        public ToolStripStatusLabel tsStatusLabel1 { get; set; }
        public log4net.ILog ProcessLogger { get; set; }
        public log4net.ILog ExceptionLogger { get; set; }

        public List<clszichanfuzaibiaoinfo> zichanfuzaibiao_Result;

        public string ConStr;
        public clsAllnew()
        {
            //  ServerID();
            //ConStr = SQLHelper.ConStr_sql;
            ConStr = ConfigurationManager.ConnectionStrings["GODDbContext"].ConnectionString;


        }

        public List<clszichanfuzaibiaoinfo> ReadDatasources(ref BackgroundWorker bgWorker, string filename)
        {
            zichanfuzaibiao_Result = new List<clszichanfuzaibiaoinfo>();

            string path = AppDomain.CurrentDomain.BaseDirectory + "Resources";
            List<string> Alist = GetBy_CategoryReportFileName(path);

            for (int i = 0; i < Alist.Count; i++)
            {
                //GetKEYnfo(path + "\\" + Alist[i]);
            }


            return zichanfuzaibiao_Result;


        }
        //获取文件路径方法‘
        public List<string> GetBy_CategoryReportFileName(string dirPath)
        {

            List<string> FileNameList = new List<string>();
            ArrayList list = new ArrayList();

            if (Directory.Exists(dirPath))
            {
                list.AddRange(Directory.GetFiles(dirPath));
            }
            if (list.Count > 0)
            {
                foreach (object item in list)
                {
                    if (!item.ToString().Contains("~$"))
                        FileNameList.Add(item.ToString().Replace(dirPath + "\\", ""));
                }
            }

            return FileNameList;
        }

        public void DownLoadPDF(ref BackgroundWorker bgWorker, string pathname)
        {
            bgWorker1 = bgWorker;

            if (XLSConvertToPDF(pathname, pathname.Replace("xlsx", "pdf")))
            {
                var dir = System.IO.Path.GetDirectoryName(pathname);
                string namesave = System.IO.Path.GetFileName(pathname);
                //File.Copy(pathname.Replace("xlsx", "pdf"), dir + "\\" + namesave.Replace("xlsx", "pdf"));

                //File.Delete(pathname);
                //File.Delete(pathname.Replace("xlsx", "pdf"));
            }



        }
        private bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.Application ExcelApp = null;
            Microsoft.Office.Interop.Excel._Workbook ExcelBook = null;
            try
            {

                object target = targetPath;
                object type = targetType;

                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                System.Reflection.Missing missingValue = System.Reflection.Missing.Value;
                ExcelBook = ExcelApp.Workbooks.Open(sourcePath, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue, missingValue);

                ExcelBook.ExportAsFixedFormat(targetType, target, Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;


            }
            catch
            {
                result = false;
            }
            finally
            {
                if (ExcelBook != null)
                {
                    ExcelBook.Close(true, missing, missing);
                    ExcelBook = null;
                }
                if (ExcelApp != null)
                {
                    ExcelApp.Quit();
                    ExcelApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        public bool InsterFile_detail_Server(List<clsFile_Managermentinfo> updateResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clsFile_Managermentinfo item in updateResult)
                {

                    string sql = "";
                    sql = "insert into File_Managerment(wenjianbiaohao,biaoti,wenhao,zhiwendanwei,xingwendanwei,dengjiriqi,miji,wenjianleibie,yeshu,fenshu,accfile_id,beizhu,NodeID,wenjianqiriqi,wenjianzhiriqi,baoguanqixian) values ('" + item.wenjianbiaohao + "','" + item.biaoti + "',N'" + item.wenhao + "','" + item.zhiwendanwei + "','" + item.xingwendanwei + "','" + item.dengjiriqi + "','" + item.miji + "','" + item.wenjianleibie + "',N'" + item.yeshu + "',N'" + item.fenshu + "',N'" + item.accfile_id + "',N'" + item.beizhu + "',N'" + item.NodeID + "','" + item.wenjianqiriqi + "','" + item.wenjianzhiriqi + "','" + item.baoguanqixian + "')";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return isok;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }
        public bool Update_File_detail_Server(List<clsFile_Managermentinfo> updateResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clsFile_Managermentinfo item in updateResult)
                {

                    string sql = "";
                    string conditions = "";
                    if (item.wenjianbiaohao != null)
                    {
                        conditions += " wenjianbiaohao ='" + item.wenjianbiaohao + "'";
                    }
                    if (item.biaoti != null)
                    {
                        conditions += " ,biaoti ='" + item.biaoti + "'";
                    }
                    if (item.wenhao != null)
                    {
                        conditions += " ,wenhao ='" + item.wenhao + "'";
                    }
                    if (item.zhiwendanwei != null)
                    {
                        conditions += " ,zhiwendanwei ='" + item.zhiwendanwei + "'";
                    }
                    if (item.xingwendanwei != null)
                    {
                        conditions += " ,xingwendanwei ='" + item.xingwendanwei + "'";
                    }
                    if (item.dengjiriqi != null)
                    {
                        conditions += " ,dengjiriqi ='" + item.dengjiriqi + "'";
                    }
                    if (item.miji != null)
                    {
                        conditions += " ,miji ='" + item.miji + "'";
                    }
                    if (item.wenjianleibie != null)
                    {
                        conditions += " ,wenjianleibie ='" + item.wenjianleibie + "'";
                    }
                    if (item.yeshu != null)
                    {
                        conditions += " ,yeshu ='" + item.yeshu + "'";
                    }
                    if (item.fenshu != null)
                    {
                        conditions += " ,fenshu ='" + item.fenshu + "'";
                    }
                    if (item.accfile_id != null)
                    {
                        conditions += " ,accfile_id ='" + item.accfile_id + "'";
                    }
                    if (item.beizhu != null)
                    {
                        conditions += " ,beizhu ='" + item.beizhu + "'";
                    }
                    if (item.NodeID != null)
                    {
                        conditions += " ,NodeID ='" + item.NodeID + "'";
                    }
                    if (item.wenjianqiriqi != null)
                    {
                        conditions += " ,wenjianqiriqi ='" + item.wenjianqiriqi + "'";
                    }
                    if (item.wenjianzhiriqi != null)
                    {
                        conditions += " ,wenjianzhiriqi ='" + item.wenjianzhiriqi + "'";
                    }
                    if (item.baoguanqixian != null)
                    {
                        conditions += " ,baoguanqixian ='" + item.baoguanqixian + "'";
                    }
                    conditions = "update File_Managerment set  " + conditions + " where T_id = " + item.T_id + " ";
                    sql = conditions;

                    //sql = "update File_Managerment t set (t.wenjianbiaohao,t.biaoti,t.wenhao,t.zhiwendanwei,t.xingwendanwei,t.dengjiriqi,miji,t.wenjianleibie,t.yeshu,t.fenshu,t.accfile_id,t.beizhu,t.NodeID) values ('" + item.wenjianbiaohao + "','" + item.biaoti + "',N'" + item.wenhao + "','" + item.zhiwendanwei + "','" + item.xingwendanwei + "','" + item.dengjiriqi + "','" + item.miji + "','" + item.wenjianleibie + "',N'" + item.yeshu + "',N'" + item.fenshu + "',N'" + item.accfile_id + "',N'" + item.beizhu + "',N'" + item.NodeID + "') where T_id='" + item.T_id + "'";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return isok;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }
        public bool InsteraccFile_Server(List<clsAccFileinfo> updateResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);


            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clsAccFileinfo item in updateResult)
                {

                    string sql = "";
                    sql = "insert into AccFile(File_name,accfile_id,mark1,mark2,mark3,mark4,mark5) values ('" + item.File_name + "','" + item.accfile_id + "',N'" + item.mark1 + "','" + item.mark2 + "','" + item.mark3 + "','" + item.mark4 + "','" + item.mark5 + "')";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return isok;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }
        public bool Update_accFile_Server(List<clsAccFileinfo> updateResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);


            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clsAccFileinfo item in updateResult)
                {

                    string conditions = "";
                    if (item.File_name != null)
                    {
                        conditions += " File_name ='" + item.File_name + "'";
                    }
                    if (item.accfile_id != null)
                    {
                        conditions += " ,accfile_id ='" + item.accfile_id + "'";
                    }
                    if (item.mark1 != null)
                    {
                        conditions += " ,mark1 ='" + item.mark1 + "'";
                    }
                    if (item.mark2 != null)
                    {
                        conditions += " ,mark2 ='" + item.mark2 + "'";
                    }
                    if (item.mark3 != null)
                    {
                        conditions += " ,mark3 ='" + item.mark3 + "'";
                    }
                    if (item.mark4 != null)
                    {
                        conditions += " ,mark4 ='" + item.mark4 + "'";
                    }
                    if (item.mark5 != null)
                    {
                        conditions += " ,mark5 ='" + item.mark5 + "'";
                    }

                    conditions = "update AccFile set  " + conditions + " where T_id = " + item.T_id + " ";
                  


                    string sql = "";
                    sql = conditions;
                    //sql = "update  AccFile set (File_name,accfile_id,mark1,mark2,mark3,mark4,mark5) values ('" + item.File_name + "','" + item.accfile_id + "',N'" + item.mark1 + "','" + item.mark2 + "','" + item.mark3 + "','" + item.mark4 + "','" + item.mark5 + "')";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return isok;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }

        public List<clsFile_Managermentinfo> find_File_Managerment(ref BackgroundWorker bgWorker, string text)
        {

         bgWorker1 = bgWorker;

            OleDbConnection aConnection = new OleDbConnection(ConStr);
            try
            {
                List<clsFile_Managermentinfo> dailyResult = new List<clsFile_Managermentinfo>();
                if (aConnection.State == ConnectionState.Closed)
                    aConnection.Open();

                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(text, aConnection);
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(myDataAdapter);
                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds, "File_Managerment");
                foreach (DataRow emp in ds.Tables["File_Managerment"].Rows)
                {
                    clsFile_Managermentinfo tempnote = new clsFile_Managermentinfo(); //定义返回值

                    if (emp["T_id"].ToString() != "")
                        tempnote.T_id = emp["T_id"].ToString();
                    if (emp["wenjianbiaohao"].ToString() != "")
                        tempnote.wenjianbiaohao = emp["wenjianbiaohao"].ToString();
                    if (emp["biaoti"].ToString() != "")
                        tempnote.biaoti = emp["biaoti"].ToString();
                    if (emp["wenhao"].ToString() != "")
                        tempnote.wenhao = emp["wenhao"].ToString();
                    if (emp["zhiwendanwei"].ToString() != "")
                        tempnote.zhiwendanwei = emp["zhiwendanwei"].ToString();
                    if (emp["xingwendanwei"].ToString() != "")
                        tempnote.xingwendanwei = emp["xingwendanwei"].ToString();
                    if (emp["dengjiriqi"].ToString() != "")
                        tempnote.dengjiriqi = emp["dengjiriqi"].ToString();
                    if (emp["miji"].ToString() != "")
                        tempnote.miji = emp["miji"].ToString();
                    if (emp["wenjianleibie"].ToString() != "")
                        tempnote.wenjianleibie = emp["wenjianleibie"].ToString();
                    if (emp["yeshu"].ToString() != "")
                        tempnote.yeshu = emp["yeshu"].ToString();
                    if (emp["fenshu"].ToString() != "")
                        tempnote.fenshu = emp["fenshu"].ToString();
                    if (emp["accfile_id"].ToString() != "")
                        tempnote.accfile_id = emp["accfile_id"].ToString();
                    if (emp["beizhu"].ToString() != "")
                        tempnote.beizhu = emp["beizhu"].ToString();
                    if (emp["NodeID"].ToString() != "")
                        tempnote.NodeID = emp["NodeID"].ToString();

                    if (emp["wenjianqiriqi"].ToString() != "")
                        tempnote.wenjianqiriqi = emp["wenjianqiriqi"].ToString();

                    if (emp["wenjianzhiriqi"].ToString() != "")
                        tempnote.wenjianzhiriqi = emp["wenjianzhiriqi"].ToString();
                    if (emp["baoguanqixian"].ToString() != "")
                        tempnote.baoguanqixian = emp["baoguanqixian"].ToString();

                    dailyResult.Add(tempnote);

                }



                return dailyResult;


            }
            catch (Exception ex)
            {
                if (aConnection.State == ConnectionState.Open) aConnection.Close(); aConnection.Dispose();
                //  bgWorker1.ReportProgress(0, "读取失败 ，请刷新后重新读取！");

                return null;

                throw ex;
            }
            finally { if (aConnection.State == ConnectionState.Open) aConnection.Close(); aConnection.Dispose(); }

        }


        public List<clsAccFileinfo> find_ACCFile(string text)
        {


            OleDbConnection aConnection = new OleDbConnection(ConStr);
            try
            {
                List<clsAccFileinfo> dailyResult = new List<clsAccFileinfo>();
                if (aConnection.State == ConnectionState.Closed)
                    aConnection.Open();

                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(text, aConnection);
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(myDataAdapter);
                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds, "AccFile");
                foreach (DataRow emp in ds.Tables["AccFile"].Rows)
                {
                    clsAccFileinfo tempnote = new clsAccFileinfo(); //定义返回值

                    if (emp["T_id"].ToString() != "")
                        tempnote.T_id = emp["T_id"].ToString();
                    if (emp["File_name"].ToString() != "")
                        tempnote.File_name = emp["File_name"].ToString();
                    if (emp["accfile_id"].ToString() != "")
                        tempnote.accfile_id = emp["accfile_id"].ToString();
                    if (emp["mark1"].ToString() != "")
                        tempnote.mark1 = emp["mark1"].ToString();
                    if (emp["mark2"].ToString() != "")
                        tempnote.mark2 = emp["mark2"].ToString();
                    if (emp["mark3"].ToString() != "")
                        tempnote.mark3 = emp["mark3"].ToString();
                    if (emp["mark4"].ToString() != "")
                        tempnote.mark4 = emp["mark4"].ToString();
                    if (emp["mark5"].ToString() != "")
                        tempnote.mark5 = emp["mark5"].ToString();


                    dailyResult.Add(tempnote);

                }



                return dailyResult;


            }
            catch (Exception ex)
            {
                if (aConnection.State == ConnectionState.Open) aConnection.Close(); aConnection.Dispose();
                //  bgWorker1.ReportProgress(0, "读取失败 ，请刷新后重新读取！");

                return null;

                throw ex;
            }
            finally { if (aConnection.State == ConnectionState.Open) aConnection.Close(); aConnection.Dispose(); }

        }

        public List<softTime_info> findsoftTime(string findtext)
        {
            //    findtext = sqlAddPCID(findtext);
            MySql.Data.MySqlClient.MySqlDataReader reader = NewMySqlHelper.ExecuteReader(findtext);
            List<softTime_info> ClaimReport_Server = new List<softTime_info>();

            while (reader.Read())
            {
                softTime_info item = new softTime_info();
                if (reader.GetValue(0) != null && Convert.ToString(reader.GetValue(0)) != "")
                    item._id = Convert.ToString(reader.GetValue(0));

                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.name = reader.GetString(1);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.starttime = reader.GetString(2);
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.endtime = reader.GetString(3);

                ClaimReport_Server.Add(item);

                //这里做数据处理....
            }
            return ClaimReport_Server;
        }
        public bool deleteFile_Managerment(string name)
        {
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string sql2 = "delete from File_Managerment where   T_id='" + name + "'";

                OleDbCommand cmd = new OleDbCommand(sql2, con);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }

        }
        public bool deleteaccFil (string name)
        {
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string sql2 = "delete from AccFile where   accfile_id='" + name + "'";

                OleDbCommand cmd = new OleDbCommand(sql2, con);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }

        }
        public void downcsv(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Sorry , No Data Output !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "csv|*.csv";
            string strFileName = "  下载信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            saveFileDialog.FileName = strFileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                strFileName = saveFileDialog.FileName.ToString();
            }
            else
            {
                return;
            }
            FileStream fa = new FileStream(strFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fa, Encoding.Unicode);
            string delimiter = "\t";
            string strHeader = "";
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                strHeader += dataGridView.Columns[i].HeaderText + delimiter;
            }
            sw.WriteLine(strHeader);

            //output rows data
            for (int j = 0; j < dataGridView.Rows.Count; j++)
            {
                string strRowValue = "";

                for (int k = 0; k < dataGridView.Columns.Count; k++)
                {
                    if (dataGridView.Rows[j].Cells[k].Value != null)
                    {
                        strRowValue += dataGridView.Rows[j].Cells[k].Value.ToString().Replace("\r\n", " ").Replace("\n", "") + delimiter;


                    }
                    else
                    {
                        strRowValue += dataGridView.Rows[j].Cells[k].Value + delimiter;
                    }
                }
                sw.WriteLine(strRowValue);
            }
            sw.Close();
            fa.Close();
            MessageBox.Show("下载完成 ！", "System", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
