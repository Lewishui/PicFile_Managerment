using clsCommon;
using FA.Buiness;
using FA.DB;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicFile_Managerment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmLogin());
            #region Noway
            //bool success = NewMySqlHelper.DbConnectable();

            //if (success == false)
            //{
            //    MessageBox.Show("系统网络异常,请保持网络畅通或联系开发人员 !");
            //    return;
            //}

            //string strSelect = "select * from control_soft_time where name='" + "PicFile_Managerment" + "'";


            //clsAllnew BusinessHelp = new clsAllnew();
            //List<softTime_info> list_Server = new List<softTime_info>();
            //list_Server = BusinessHelp.findsoftTime(strSelect);



            //DateTime oldDate = DateTime.Now;
            //DateTime dt3;
            //string endday = DateTime.Now.ToString("yyyy/MM/dd");
            //dt3 = Convert.ToDateTime(endday);
            //DateTime dt2;
            //if (list_Server.Count == 0 && list_Server[0].endtime == null || list_Server[0].endtime == "")
            //{
            //    MessageBox.Show("系统网络异常,请保持网络畅通或联系开发人员 !");
            //    return;
            //}
            //else
            //    dt2 = Convert.ToDateTime(list_Server[0].endtime);

            //TimeSpan ts = dt2 - dt3;
            //int timeTotal = ts.Days;

            //if (timeTotal > 0 && timeTotal < 10)
            //{
            //    MessageBox.Show("本系统测试版本即将到期,请及时续费以免影响使用 !\r\n\r\n温馨提示：联系方式网址：www.yhocn.com\r\nQQ：512250428\r\n微信：bqwl07910", "服务到期", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //}
            //if (timeTotal < 0)
            //{
            //    MessageBox.Show("本系统测试版本即将到期,请及时续费 !\r\n\r\n温馨提示：联系方式网址：www.yhocn.com\r\nQQ：512250428\r\n微信：bqwl07910", "服务到期", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Application.Exit();

            //    return;
            //}
            #endregion
            #region Noway2
            DateTime oldDate = DateTime.Now;
            DateTime dt3;
            string endday = DateTime.Now.ToString("yyyy/MM/dd");
            dt3 = Convert.ToDateTime(endday);
            DateTime dt2;
            dt2 = Convert.ToDateTime("2018/11/20");

            TimeSpan ts = dt2 - dt3;
            int timeTotal = ts.Days;
            if (timeTotal < 0)
            {
                MessageBox.Show("测试版本运行期已到，请将剩余费用付清 !");
                return;
            }


            #endregion
            Application.Run(new frmLogin());//frmLogin
        }
    }
}
