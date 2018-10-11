using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace PicFile_Managerment
{
	/// <summary>
	/// Summary description for DesktopUI.
	/// </summary>
	public class UI
	{
		public UI()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        #region Get App Path
        public static string GetAppPath()
        {
           string AppPath = "";

           try
           {
             AppPath = Path.GetFullPath(".");  
             AppPath = AppPath.Replace(@"\bin\Debug",""); 
             AppPath = AppPath.Replace(@"\bin\Release",""); 
             if (AppPath.EndsWith(@"\") == false) { AppPath += @"\"; }
           }
           catch (Exception) { throw; }
           return AppPath;
        }
        #endregion

        #region Show Error
        public static void ShowError(string msg)
        {
          MessageBox.Show(msg);
        }
        #endregion

        #region Hourglass
        public static void Hourglass(bool Show)
        {
           if (Show == true)
           {
              System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
           }
           else
           {
              System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default; 
           }
           return;
        }
        #endregion

	}
}
