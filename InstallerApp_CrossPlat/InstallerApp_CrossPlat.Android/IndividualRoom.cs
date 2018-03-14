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
using System.Threading;
using System.Threading.Tasks;

namespace InstallerApp_CrossPlat.Droid
{
    [Activity(Label = "",ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]
    public class IndividualRoom : Activity
    {
        List<IndividualRoomList> lstIndividualRoomInfoClass = new List<IndividualRoomList>();
        List<InstallerInfoList> lstInstallerInfoClass = new List<InstallerInfoList>();
        ListView listViewRoomInfo, listViewInstallerInfo;
        string[] getstringRooms, getSelectedInstaller;
        TextView textViewRoomInfo;
        ProgressDialog progressDialog;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        int PartsCount, deliveryPhoto, installationPhoto = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.IndividualRoom);
            getstringRooms = Intent.GetStringArrayExtra("keyRoomInfo");
            getSelectedInstaller = Intent.GetStringArrayExtra("keyselectedInstaller");

            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            //Display Header Information
            displayHeaderInfo();
            textViewRoomInfo = FindViewById<TextView>(Resource.Id.textViewRoomInfo);
            textViewRoomInfo.Text = getstringRooms[2];
            //Display ProgressBar
            progressDialog = ProgressDialog.Show(this, "Loading...", "Please wait!!", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            //Manage LongRunning Task
            ThreadPool.QueueUserWorkItem(q => longRunningMethod());
        }

        public void displayHeaderInfo()
        {
            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(StartJobScheduleStatus)).SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(intent);
            };
            headerGeneralInfo.textViewGeneral.Text = "Job Number: " + getSelectedInstaller[3];
            headerGeneralInfo.imgViewIcon.Visibility = Android.Views.ViewStates.Gone;
        }

        public void longRunningMethod()
        {
            new Thread(new ThreadStart(async delegate
            {
                await Task.Delay(50);
                RunOnUiThread(() =>
                {
                    serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
                    var partsInfoList = serviceInstaller.InsKP_GetPartInfo(getSelectedInstaller[4].ToString(), getstringRooms[2].ToString());
                    PartsCount = partsInfoList.Length;
                    var lstInstallerImages = serviceInstaller.insKP_getInstallerImages(getstringRooms[0]).ToList<byte[]>();
                    installationPhoto = lstInstallerImages.Count;
                    displayFetchedRoomInfo();
                    progressDialog.Dismiss();
                });
            })).Start();
        }
        
        public void displayFetchedRoomInfo()
        {
            //Installer Info List
            var fillInstallerProperties = new InstallerInfoList
            {
                Company = getSelectedInstaller[0],
                Project = getSelectedInstaller[1],
                Lot = getSelectedInstaller[2],
                JobNum = getSelectedInstaller[3],
                InstallerJobStatus = int.Parse(getSelectedInstaller[7])
            };
            lstInstallerInfoClass.Add(fillInstallerProperties);
            listViewInstallerInfo = FindViewById<ListView>(Resource.Id.lstInstallerInfo);
            //Populate Installer ListView with Data
            listViewInstallerInfo.Adapter = new JobScreenAdapter(this, lstInstallerInfoClass);

            //Room Info List
            int roomListcount=0;
            while (roomListcount < 2)
            {
                if (roomListcount == 0)
                {
                    var fillIndividualRoomProperties = new IndividualRoomList
                    {
                        RSNo = getstringRooms[0],
                        CSID = getstringRooms[1],
                        Rooms = getstringRooms[2],
                        Style = getstringRooms[3],
                        Colour = getstringRooms[4],
                        Hardware = getstringRooms[3],
                        CounterTop = getstringRooms[5],
                        PartsCount = PartsCount
                    };
                    lstIndividualRoomInfoClass.Add(fillIndividualRoomProperties);
                }
                else if (roomListcount == 1)
                {
                    var fillIndividualRoomProperties = new IndividualRoomList
                    {
                        Rooms = "",
                        Style = "Delivery Photos: " + deliveryPhoto,
                        Colour = "",
                        Hardware = "",
                        CounterTop = "Installation Photos : " + installationPhoto,
                        deliveryPhoto = 0,
                        installationPhoto = installationPhoto
                    };
                    lstIndividualRoomInfoClass.Add(fillIndividualRoomProperties);
                }
                roomListcount++;
            }
            listViewRoomInfo = FindViewById<ListView>(Resource.Id.lstIndividualRoom);
            //Populate IndividualRoom Listview with data
            listViewRoomInfo.Adapter = new IndividualRoomAdapter(this, lstIndividualRoomInfoClass);
            listViewRoomInfo.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                if (args.Position == 0)
                {
                    Bundle b = new Bundle();
                    b.PutStringArray("keyRoomInfo", getstringRooms);
                    b.PutStringArray("keyselectedInstaller", getSelectedInstaller);
                    var intent = new Android.Content.Intent(this, typeof(PartsInfo));
                    intent.PutExtras(b);
                    StartActivity(intent);
                }
                else if (args.Position == 1)
                {
                    Bundle b = new Bundle();
                    b.PutStringArray("keyRoomInfo", getstringRooms);
                    b.PutStringArray("keyselectedInstaller", getSelectedInstaller);
                    var intent = new Android.Content.Intent(this, typeof(PhotoGallery));
                    intent.PutExtras(b);
                    StartActivity(intent);
                }
            };

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.JobMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            lstInstallerInfoClass.Clear();
            lstIndividualRoomInfoClass.Clear();
            longRunningMethod();
            return base.OnOptionsItemSelected(item);
        }
    }
}