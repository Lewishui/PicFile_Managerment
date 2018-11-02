using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WIA;
using System.IO;

namespace PicFile_Managerment
{
    public partial class frmAutoScan : Form
    {
        public frmAutoScan()
        {
            InitializeComponent();
        }
        private int[] _colors = new int[]
		{
			1,
			2,
			4
		};
        private void btnScan_Click(object sender, EventArgs e)
        {


            ImageFile imageFile = null;
            CommonDialogClass cdc = new WIA.CommonDialogClass();

            try
            {
                imageFile = cdc.ShowAcquireImage(WIA.WiaDeviceType.ScannerDeviceType,
                                                WIA.WiaImageIntent.TextIntent,
                                                WIA.WiaImageBias.MaximizeQuality,
                                                "{00000000-0000-0000-0000-000000000000}",
                                                false,
                                                false,
                                                false);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                imageFile = null;
            }
            if (imageFile != null)
            {

                // SaveFileDialog s = new SaveFileDialog();
                //s.ShowDialog();
                // imageFile.SaveFile(s.FileName);
                //using (FileStream stream = new FileStream(s.FileName, FileMode.Open,
                //    FileAccess.Read, FileShare.Read))
                //{
                // pictureBox1.Image = Image.FromFile(s.FileName);
                // } 

            }


            if (imageFile != null)
            {
                var buffer = imageFile.FileData.get_BinaryData() as byte[];
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buffer, 0, buffer.Length);
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }


        }
        private void setItem(IItem item, object property, object value)
        {
            WIA.Property aProperty = item.Properties.get_Item(ref property);
            aProperty.set_Value(ref value);
            Type obj = aProperty.GetType();
        }

        private void btnAutoScan_Click(object sender, EventArgs e)
        {
            DeviceManager manager = new DeviceManagerClass();
            Device device = null;

            foreach (DeviceInfo info in manager.DeviceInfos)
            {
                if (info.Type != WiaDeviceType.ScannerDeviceType)
                {
                    continue;
                }
                device = info.Connect();
                break;
            }
            Item item = device.Items[1];
            //string s = "";
            //for (int i = 1; i < item.Properties.Count - 1; i++)
            //{
            //    object o = (int)i;
            //    Property p = item.Properties.get_Item(ref o);

            //    s += p.Name + "----" + p.PropertyID + "----" + Convert.ToString(p.get_Value()) + "\r\n";
            //}



            int dpi = (int)numericUpDown1.Value;
            ScanColor color = (ScanColor)this._colors[this.comboBox1.SelectedIndex];
            object obj = (int)color;
            object obj2 = "6146";//WiaImageIntent

            setItem(item, obj2, obj);

            object obj3 = dpi;
            object obj4 = "6147";
            setItem(item, obj4, obj3);
            object obj5 = dpi;
            object obj6 = "6148";

            setItem(item, obj6, obj5);

            setItem(item, "6151", 800);
            setItem(item, "6152", 100 * 11);





            CommonDialogClass cdc = new WIA.CommonDialogClass();
            ImageFile imageFile = cdc.ShowTransfer(item,
                "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}",
                true) as ImageFile;

            if (imageFile != null)
            {
                var buffer = imageFile.FileData.get_BinaryData() as byte[];
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buffer, 0, buffer.Length);
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
        }

        private void frmAutoScan_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            numericUpDown1.Value = 100;
            //Image img= Image.FromFile(@"E:\Users\zhwlyf\Pictures\m.png");
            //pictureBox1.Image = img;
        }


    }
}
