﻿using FA.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FA.Common
{
    public class clsCommHelp
    {
        #region NullToString
        public static string NullToString(object obj)
        {
            string strResult = "";
            if (obj != null)
            {
                strResult = obj.ToString().Trim();
            }
            return strResult;
        }
        #endregion

        #region StringToDecimal
        /// <summary>
        /// 转换字符串，将字符串转换成数字，并且将空字符串转换成0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s)
        {
            decimal result = 0;

            if (s != null && s != "")
            {
                result = Decimal.Parse(s);
            }
            return result;
        }
        #endregion

        #region StringToInt
        /// <summary>
        /// 转换字符串，将字符串转换成数字，并且将空字符串转换成0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int StringToInt(string s)
        {
            int result = 0;

            if (s != null && s != "")
            {
                result = Convert.ToInt32(s.Trim());
            }
            return result;
        }
        #endregion

        #region 日期转换(objToDateTime)
        /// <summary>
        /// 将excel里取得的日期转化成String数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string objToDateTime<T>(T t)
        {
            string strResult = "";
            object obj = t;

            try
            {
                if (obj != null)
                {
                    strResult = DateTime.FromOADate((double)obj).ToString("MM/dd/yyyy");
                }
            }
            catch
            {
                try
                {
                    strResult = Convert.ToDateTime(obj.ToString()).ToString("MM/dd/yyyy");
                }
                catch
                {
                    try
                    {
                        if (obj.ToString().Length == 8)
                        {
                            strResult = DateTime.Parse(obj.ToString().Substring(0, 4) + "-" +
                                                       obj.ToString().Substring(4, 2) + "-" +
                                                       obj.ToString().Substring(6, 2)).ToString("MM/dd/yyyy");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return strResult;
        }

        public static string objToDateTime1<T>(T t)
        {
            string strResult = "";
            object obj = t;

            try
            {
                if (obj != null)
                {
                    strResult = DateTime.FromOADate((double)obj).ToString("yyyy/MM/dd");
                }
            }
            catch
            {
                try
                {
                    strResult = Convert.ToDateTime(obj.ToString()).ToString("yyyy/MM/dd");
                }
                catch
                {
                    try
                    {
                        if (obj.ToString().Length == 8)
                        {
                            strResult = DateTime.Parse(obj.ToString().Substring(4, 4) + "-" +
                                                       obj.ToString().Substring(0, 2) + "-" +
                                                       obj.ToString().Substring(2, 2)).ToString("yyyy/MM/dd");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return strResult;
        }
        public static string objToDateTime2<T>(T t)
        {
            string strResult = "";
            object obj = t;

            try
            {
                if (obj != null)
                {
                    strResult = DateTime.FromOADate((double)obj).ToString("yyyy/MM/dd/HH/mm");
                }
            }
            catch
            {
                try
                {
                    strResult = Convert.ToDateTime(obj.ToString()).ToString("yyyy/MM/dd/HH/mm");
                }
                catch
                {
                    try
                    {
                        if (obj.ToString().Length == 8)
                        {
                            strResult = DateTime.Parse(obj.ToString().Substring(4, 4) + "-" +
                                                       obj.ToString().Substring(0, 2) + "-" +
                                                       obj.ToString().Substring(2, 2)).ToString("yyyy/MM/dd/HH/mm");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return strResult;
        }
        #endregion

        #region 字符串简单加密解密

        /// <summary>
        /// 简单加密解密

        /// </summary>
        /// <param name="str">需要加密、解密的字符串</param>
        /// <returns>加密、解密后的字符串</returns>
        public static string encryptString(string str)
        {
            string strResult = "";
            char[] charMessage = str.ToCharArray();
            foreach (char c in charMessage)
            {
                char newChar = changerChar(c);
                strResult += newChar.ToString();
            }
            return strResult;
        }

        private static char changerChar(char c)
        {
            char resutlt;
            int intStrLength = 0;
            string twoString = Convert.ToString(c, 2).PadLeft(8, '0');
            if (twoString.Length > 8)
            {
                twoString = Convert.ToString(c, 2).PadLeft(16, '0');
            }
            intStrLength = twoString.Length;
            string newTwoString = twoString.Substring(intStrLength / 2) + twoString.Substring(0, intStrLength / 2);
            resutlt = Convert.ToChar(Convert.ToInt32(newTwoString, 2));
            return resutlt;
        }
        #endregion

        #region 将字符串日期转换为时间类型

        public static DateTime GetDateByString(string dateString)
        {
            return DateTime.Parse(dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" + dateString.Substring(6, 2));
        }
        #endregion

        #region 关闭打开的Excel
        public static void CloseExcel(Microsoft.Office.Interop.Excel.Application ExcelApplication, Microsoft.Office.Interop.Excel.Workbook ExcelWorkbook)
        {
            ExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            ExcelWorkbook = null;
            ExcelApplication.Quit();
            GC.Collect();
            clsKeyMyExcelProcess.Kill(ExcelApplication);
        }
        #endregion

        #region 得到Sap连接字符串

        #endregion

        #region 判断2个日期是否是整年

        public static bool CheckThroughoutTheYear(string data1, string date2)
        {
            bool blnResult = false;
            string dtStart = "";
            string dtEnd = "";
            if (Convert.ToDateTime(date2).CompareTo(Convert.ToDateTime(data1)) > 0)
            {
                dtStart = data1;
                dtEnd = date2;
            }
            else
            {
                dtStart = date2;
                dtEnd = data1;
            }
            string strTheoryDate = Convert.ToDateTime(dtEnd).ToString("yyyy")
                                 + Convert.ToDateTime(dtStart).ToString("MMdd");
            strTheoryDate = Convert.ToDateTime(objToDateTime<string>(strTheoryDate)).AddDays(-1).ToString("MM/dd/yyyy");
            if (objToDateTime<string>(strTheoryDate) == objToDateTime<string>(dtEnd))
            {
                blnResult = true;
            }
            return blnResult;
        }

        #endregion

        public static string RandomID()
        {
            Random rd = new Random();
            string str = "";
            while (str.Length < 10)
            {
                int temp = rd.Next(0, 10);
                if (!str.Contains(temp + ""))
                {
                    str += temp;
                }
            }

            return str;

        }

        #region pic
        public static string LocationImagePath(clsAccFileinfo location)
        {

            string basePath = LocationImageBasePath();
            string folderPath = Path.Combine(basePath, (Convert.ToInt64(location.accfile_id) / 1000 * 1000).ToString());

            CreateFolder(folderPath);

            string fullPath = Path.Combine(folderPath, location.File_name);

            return fullPath;
        }
        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string LocationImageBasePath()
        {
            int type = 1;

            string path = Path.Combine(ImageBasePath, "location_" + type.ToString().ToLower());
            CreateFolder(path);
            return path;
        }
        public static string ImageBasePath
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "images");
                CreateFolder(path);
                return path + Path.DirectorySeparatorChar.ToString();
            }
        }
        #endregion


        #region pic
        public static void pdfMain(string[] args, string strFileName)
        {
            int ishave = 0;
            string[] files = { @"D:\Devlop\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\0x1024a0a0.jpg", @"D:\Devlop\PIC_control\PicFile_Managerment\PicFile_Managerment\bin\Debug\FileList\1.jpg" };
            files = args;
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
            try
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite));
                document.Open();
                iTextSharp.text.Image image;
               

                for (int i = 0; i < files.Length; i++)
                {
                    if (String.IsNullOrEmpty(files[i])) break;

                    if (File.Exists(files[i]))
                        image = iTextSharp.text.Image.GetInstance(files[i]);
                    else
                    {
                        
                        continue;
                    }

                    if (image.Height > iTextSharp.text.PageSize.A4.Height - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    else if (image.Width > iTextSharp.text.PageSize.A4.Width - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                    document.NewPage();
                    document.Add(image);
                    ishave++;

                    //iTextSharp.text.Chunk c1 = new iTextSharp.text.Chunk("Hello World");
                    //iTextSharp.text.Phrase p1 = new iTextSharp.text.Phrase();
                    //p1.Leading = 150;      //行间距
                    //document.Add(p1);
                }
                Console.WriteLine("转换成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("转换失败，原因：" + ex.Message);
            }
            if (ishave != 0)
                document.Close();
            //Console.ReadKey();
        }
        #endregion



    }
}
