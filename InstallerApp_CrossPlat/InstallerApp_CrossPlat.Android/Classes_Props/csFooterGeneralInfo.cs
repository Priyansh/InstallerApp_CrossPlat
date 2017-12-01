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
using Java.IO;
using Android.Graphics;

namespace InstallerApp_CrossPlat.Droid
{    
    public class csFooterGeneralInfo
    {
        private Activity activity;
        public ImageButton footerImgBtnCamera;
        
        public csFooterGeneralInfo(Activity _activity)
        {
           this.activity = _activity;
           footerImgBtnCamera = this.activity.FindViewById<ImageButton>(Resource.Id.footerImgBtnCamera);
        }
        
    }
}