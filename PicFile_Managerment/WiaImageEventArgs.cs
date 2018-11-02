using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PicFile_Managerment
{
    public class WiaImageEventArgs : EventArgs
    {

        public WiaImageEventArgs(Image img)
        {
            this.ScannedImage = img;
        }
        public Image ScannedImage
        {
            get;
            private set;
        }
    }
}
