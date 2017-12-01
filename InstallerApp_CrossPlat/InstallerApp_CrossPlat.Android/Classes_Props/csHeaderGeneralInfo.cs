using System;
using System.Collections.Generic;
using System.Text;

using Android.App;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    public class csHeaderGeneralInfo
    {
        private Activity activity;
        public TextView textViewGeneral;
        public ImageView imgViewIcon;
        public ImageButton imgbtnBack;

        public csHeaderGeneralInfo(Activity _activity)
        {
            this.activity = _activity;
            textViewGeneral = this.activity.FindViewById<TextView>(Resource.Id.textViewGeneral);
            imgViewIcon = this.activity.FindViewById<ImageView>(Resource.Id.imgViewIcon);
            imgbtnBack = this.activity.FindViewById<ImageButton>(Resource.Id.imgbtnBack);
        }
    }
}
