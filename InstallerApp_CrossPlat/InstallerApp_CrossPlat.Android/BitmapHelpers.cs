using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace InstallerApp_CrossPlat.Droid
{
    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            //First get Dimension of the file
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile (fileName, options);

            //Calculate ration how much resize image
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            //Now Load image & have BitmapFactory resize a image
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizeBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizeBitmap;
        }
    }
}