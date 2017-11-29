using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] //ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize,
    public class StartJobScheduleStatus : Activity
    {
        TextView textViewCompany, textViewProject, textViewLot, textViewDelivered, textViewJobStarted, txtViewSelectRoom;
        ImageButton imgbtnStartJob;
        TableLayout tblMainLayout;
        LinearLayout content2;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        string[] getStrings;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartJobScheduleStatus);
            getStrings = Intent.GetStringArrayExtra("keyInstallerInfo");
            textViewCompany = FindViewById<TextView>(Resource.Id.textViewCompany);
            textViewProject = FindViewById<TextView>(Resource.Id.textViewProject);
            textViewLot = FindViewById<TextView>(Resource.Id.textViewLot);
            textViewDelivered = FindViewById<TextView>(Resource.Id.textViewDelivered);
            textViewJobStarted = FindViewById<TextView>(Resource.Id.textViewJobStarted);
            tblMainLayout = FindViewById<TableLayout>(Resource.Id.tblMainLayout);
            content2 = FindViewById<LinearLayout>(Resource.Id.content2);
            txtViewSelectRoom = FindViewById<TextView>(Resource.Id.txtViewSelectRoom);

            textViewCompany.Text = getStrings[0];
            textViewProject.Text = getStrings[1];
            textViewLot.Text = "Unit " + getStrings[2];
            textViewDelivered.Text = "Delivered on " + Convert.ToDateTime(getStrings[5]).ToString("MMM dd, yyyy");

            //Call WebService

            //serviceInstaller.Url = "http://192.168.3.76:53435/phonegap.asmx";
            serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";

            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.textViewGeneral.Text = "Job Number: " + getStrings[3];
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(JobScreen)).SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(intent);
            };

            imgbtnStartJob = FindViewById<ImageButton>(Resource.Id.imgbtnStartJob);
            if (int.Parse(getStrings[7]) == 0) { //If InstallerJobStatus = 0, then first start job and do something
                headerGeneralInfo.imgViewIcon.SetImageResource(Resource.Drawable.imgScheduleTrans);
                imgbtnStartJob.Click += delegate
                {
                    funStartingJob(0);
                    headerGeneralInfo.imgViewIcon.SetImageResource(Resource.Drawable.imgProgressTrans);
                };
            }
            else if (int.Parse(getStrings[7]) == 1) { //If InstallerJobStatus = 1, means jobs already started
                headerGeneralInfo.imgViewIcon.SetImageResource(Resource.Drawable.imgProgressTrans);
                funStartingJob(1);
            }
        }

        public void displayTableLayout()
        {
            if (tblMainLayout.ChildCount > 1) //Remove table rows except the HeaderTextView
                tblMainLayout.RemoveViews(1, tblMainLayout.ChildCount - 1);

            var serviceListRoomsInfo = serviceInstaller.InsKP_GetRoomInfo(int.Parse(getStrings[6]));
            
            // Find Orientation and arrange code accordingly
            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            TableRow.LayoutParams tbllayoutPara = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            TableRow tblRow = new TableRow(this);
            int leftMargin = 5;
            int TopMargin = 5;
            int rightMargin = 5;
            int bottomMargin = 5;
            int column = 0;
            tbllayoutPara.SetMargins(leftMargin, TopMargin, rightMargin, bottomMargin);

            for (int i = 0; i < serviceListRoomsInfo.Length; i++)
            {
                if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
                {
                    if (i % 3 == 0 && i != 0)
                    {
                        tblMainLayout.AddView(tblRow);
                        tblRow = new TableRow(this);
                        column = 0;
                    }
                }
                else if (surfaceOrientation == SurfaceOrientation.Rotation270 || surfaceOrientation == SurfaceOrientation.Rotation90)
                {
                    if (i % 5 == 0 && i != 0)
                    {
                        tblMainLayout.AddView(tblRow);
                        tblRow = new TableRow(this);
                        column = 0;
                    }
                }
                Button btn = new Button(this);
                btn.Id = i;
                btn.Text = serviceListRoomsInfo[i].Rooms;
                btn.SetMinimumWidth(200);
                btn.SetMinimumHeight(180);
                btn.LayoutParameters = tbllayoutPara;
                btn.Click += delegate
                {
                    int k = btn.Id;
                    string[] strRooms = new string[] {serviceListRoomsInfo[k].RSNo, serviceListRoomsInfo[k].CSID, serviceListRoomsInfo[k].Rooms, serviceListRoomsInfo[k].Style, serviceListRoomsInfo[k].Colour, serviceListRoomsInfo[k].Hardware, serviceListRoomsInfo[k].CounterTop };
                    Bundle b = new Bundle();
                    b.PutStringArray("keyRoomInfo", strRooms);
                    b.PutStringArray("keyselectedInstaller", getStrings);
                    //var intent = new Android.Content.Intent(this, typeof(IndividualRoom));
                    //intent.PutExtras(b);
                    //StartActivity(intent);
                };
                tblRow.AddView(btn, column);
                column++;
            }
            tblMainLayout.AddView(tblRow);
        }

        public void funStartingJob(int jobStatus)
        {
            imgbtnStartJob.SetImageResource(Resource.Drawable.StartJobPressed);
            content2.Visibility = Android.Views.ViewStates.Gone;

            //Visible Dynamic TableLayout
            textViewJobStarted.Visibility = Android.Views.ViewStates.Visible;
            tblMainLayout.Visibility = Android.Views.ViewStates.Visible;
            txtViewSelectRoom.Visibility = ViewStates.Visible;
            //Job Status 0: Scheduled 1: Started 2: Completed
            // When User click on Job Start button, Update job status and job start in Purcharser table
            if (jobStatus == 0) { 
                serviceInstaller.InsKP_UpdateInstallerStatus(int.Parse(getStrings[6]), 1);
            }
            var updateInstallerJobStatus = serviceInstaller.InsKP_GetInstallerByCSID(int.Parse(getStrings[6]));
            textViewJobStarted.Text = "Job Started On " + Convert.ToDateTime(updateInstallerJobStatus[0].InstallerJobStart).ToString("MMM dd, yyyy");
            //Once Job Status Updated , then update getStrings' existing JobStatus with updated JobStatus
            getStrings[7] = updateInstallerJobStatus[0].InstallerJobStatus.ToString();
            //Display Table Layout
            var serviceListRoomsInfo = serviceInstaller.InsKP_GetRoomInfo(int.Parse(getStrings[6]));
            displayTableLayout();
        }

        //TableLayout should perform differently when orientation changed
        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                displayTableLayout();
            }
            else if(newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                displayTableLayout();
            }
        }

    }
}