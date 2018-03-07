using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace InstallerApp_CrossPlat.Droid
{
    //[Activity(Label = "InstallerApp", Icon = "@drawable/FrendelLogo", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, MainLauncher = true)]
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        ImageButton imgbtnJobs;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";
            ActionBar.SetLogo(Resource.Drawable.fk48);
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.textViewGeneral.Text = "Frendel Kitchens-Installer App";
            headerGeneralInfo.imgViewIcon.Visibility = Android.Views.ViewStates.Gone;
            headerGeneralInfo.imgbtnBack.Visibility = Android.Views.ViewStates.Gone;

            imgbtnJobs = FindViewById<ImageButton>(Resource.Id.imgbtnJobs);
            imgbtnJobs.Click += delegate {
                var intent = new Android.Content.Intent(this, typeof(JobScreen));
                StartActivity(intent);
                Finish();
            };
        }

    } //End MainActivity
}


