using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Graphics;

namespace InstallerApp_CrossPlat.Droid
{
    interface IBitmapImageImages
    {
        Bitmap bitmapImage { get; set; }
        int widthInDp { get; set; }
        int heightInDp { get; set; }
    }
}