using System;
using System.Collections.Generic;
using System.Text;
using WIA;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace PicFile_Managerment
{
    public class ADFScan
    {
        private class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 1u;
            public const uint FLATBED = 2u;
        }
        private class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEED_READY = 1u;
        }
        private class WIA_PROPERTIES
        {
            public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024u;
            public const uint WIA_DIP_FIRST = 2u;
            public const uint WIA_DPA_FIRST = 1026u;
            public const uint WIA_DPC_FIRST = 2050u;
            public const uint WIA_DPS_FIRST = 3074u;
            public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = 3087u;
            public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088u;
        }
        private class WIA_ERRORS
        {
            public const uint BASE_VAL_WIA_ERROR = 2149646336u;
            public const uint WIA_ERROR_PAPER_EMPTY = 2149646339u;
        }
        public event EventHandler<WiaImageEventArgs> Scanning;
        public event EventHandler ScanComplete;
        public void BeginScan(ScanColor color, int dotsperinch)
        {
            this.Scan(color, dotsperinch);
        }

        private void setItem(IItem item, object property, object value)
        {
            WIA.Property aProperty = item.Properties.get_Item(ref property);
            aProperty.set_Value(ref value);
        }
        private void Scan(ScanColor clr, int dpi)
        {

            //  Microsoft.Win32.RegistryKey jpegKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(@"CLSID\{D2923B86-15F1-46FF-A19A-DE825F919576}\SupportedExtension\.jpg");

            //string  jpegGuid = jpegKey.GetValue("FormatGUID") as string;

            CommonDialogClass commonDialogClass = new CommonDialogClass();
            Device device = commonDialogClass.ShowSelectDevice(WiaDeviceType.UnspecifiedDeviceType, true, false);
            if (device != null)
            {
                string deviceID = device.DeviceID;
                WIA.CommonDialog commonDialog = new CommonDialogClass();
                bool flag = true;
                int num = 0;
                int num2 = 0;
                while (flag)
                {
                    DeviceManager deviceManager = new DeviceManagerClass();
                    Device device2 = null;
                    foreach (DeviceInfo deviceInfo in deviceManager.DeviceInfos)
                    {
                        if (deviceInfo.DeviceID == deviceID)
                        {
                            WIA.Properties properties = deviceInfo.Properties;
                            device2 = deviceInfo.Connect();
                            break;
                        }
                    }
                    Item item = device2.Items[1];
                    object obj = (int)clr;
                    object obj2 = "6146";
                    setItem(item, obj2, obj);

                    object obj3 = dpi;
                    object obj4 = "6147";
                    setItem(item, obj4, obj3);
                    object obj5 = dpi;
                    object obj6 = "6148";

                    setItem(item, obj6, obj5);
                    try
                    {
                        ImageFile imageFile = (ImageFile)commonDialog.ShowTransfer(item, "{00000000-0000-0000-0000-000000000000}", false);
                        string tempFileName = Path.GetTempFileName();
                        if (File.Exists(tempFileName))
                        {
                            File.Delete(tempFileName);
                        }
                        imageFile.SaveFile(tempFileName);
                        Image img = Image.FromFile(tempFileName);


                        EventHandler<WiaImageEventArgs> scanning = this.Scanning;
                        if (scanning != null)
                        {
                            scanning(this, new WiaImageEventArgs(img));
                        }
                        num2++;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        Property property = null;
                        Property property2 = null;
                        foreach (Property property3 in device2.Properties)
                        {
                            if ((long)property3.PropertyID == 3088L)
                            {
                                property = property3;
                            }
                            if ((long)property3.PropertyID == 3087L)
                            {
                                property2 = property3;
                            }
                        }
                        flag = false;
                        if (property != null)
                        {
                            if ((Convert.ToUInt32(property.get_Value()) & 1u) != 0u)
                            {
                                flag = ((Convert.ToUInt32(property2.get_Value()) & 1u) != 0u);
                            }
                        }
                        num++;
                    }
                }
                EventHandler scanComplete = this.ScanComplete;
                if (scanComplete != null)
                {
                    scanComplete(this, EventArgs.Empty);
                }
            }
        }

        public WIA.ImageFile ScanAndSaveOnePage()
        {
            WIA.CommonDialog Dialog1 = new WIA.CommonDialogClass();
            WIA.DeviceManager DeviceManager1 = new WIA.DeviceManagerClass();
            System.Object Object1 = null;
            System.Object Object2 = null;
            WIA.Device Scanner = null;

            try
            {
                Scanner = Dialog1.ShowSelectDevice(WIA.WiaDeviceType.ScannerDeviceType, false, false);

            }
            catch
            {
                MessageBox.Show("请确认是否联系设备");
                return null;

                throw;
            }
            WIA.Item Item1 = Scanner.Items[1];
            setItem(Item1, "4104", 24);
            setItem(Item1, "6146", 2);
            setItem(Item1, "6147", 150);
            setItem(Item1, "6148", 150);
            setItem(Item1, "6151", 150 * 8.5);
            setItem(Item1, "6152", 150 * 11);

            WIA.ImageFile Image1 = new WIA.ImageFile();
            WIA.ImageProcess ImageProcess1 = new WIA.ImageProcess();
            Object1 = (Object)"Convert";
            ImageProcess1.Filters.Add(ImageProcess1.FilterInfos.get_Item(ref Object1).FilterID, 0);

            Object1 = (Object)"FormatID";
            Object2 = (Object)WIA.FormatID.wiaFormatBMP;
            ImageProcess1.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);

            Object1 = null;
            Object2 = null;

            Image1 = (WIA.ImageFile)Item1.Transfer(WIA.FormatID.wiaFormatBMP);

            return Image1;

            //string DestImagePath = @"C:\test.bmp";
            //File.Delete(DestImagePath);
            //Image1.SaveFile(DestImagePath);
        }





    }
}
