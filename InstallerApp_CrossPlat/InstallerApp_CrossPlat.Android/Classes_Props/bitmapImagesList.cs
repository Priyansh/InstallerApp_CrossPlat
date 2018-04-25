using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Android.Graphics;

namespace InstallerApp_CrossPlat.Droid
{

    public class BitmapImagesList : IBitmapImageImages
    {
        private Bitmap _bitmapImage;
       
        public Bitmap bitmapImage
        {
            get { return this._bitmapImage; }
            set { this._bitmapImage = value; }
        }
        public int widthInDp { get; set; }
        public int heightInDp { get; set; }
    }
}