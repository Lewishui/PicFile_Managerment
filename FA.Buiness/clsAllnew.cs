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
                    sql = "insert into File_Managerment(wenjianbiaohao,biaoti,wenhao,zhiwendanwei,xingwendanwei,dengjiriqi,miji,wenjianleibie,yeshu,fenshu,accfile_id,beizhu,NodeID) values ('" + item.wenjianbiaohao + "','" + item.biaoti + "',N'" + item.wenhao + "','" + item.zhiwendanwei + "','" + item.xingwendanwei + "','" + item.dengjiriqi + "','" + item.miji + "','" + item.wenjianleibie + "',N'" + item.yeshu + "',N'" + item.fenshu + "',N'" + item.accfile_id + "',N'" + item.beizhu + "',N'" + item.NodeID + "')";

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

    }
}
