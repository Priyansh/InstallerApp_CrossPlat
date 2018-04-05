using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    [Activity(Label = "")]
    public class JobScreen : Activity
    {
        List<InstallerInfoList> lstInstallerInfoClass = new List<InstallerInfoList>();
        ListView listView;
        ProgressDialog dialog;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        int installerId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.JobScreen);
            
            if (lstInstallerInfoClass.Count <= 0)
            {
                dialog = ProgressDialog.Show(this, "Loading...", "Please Wait!!!", true);
                dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                
                var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
                SetActionBar(toolbar);

                //Display Header Information
                displayHeaderInfo();
                //Call WebService
                ThreadPool.QueueUserWorkItem(o => longRunningMethod());
            }
        }

        public void longRunningMethod()
        {
            new Thread(new ThreadStart(async delegate
            {
                await Task.Delay(50);
                RunOnUiThread(() =>
                {
                    serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
                    displayWebServiceInfo(serviceInstaller);
                    dialog.Dismiss();
                });
            })).Start();
        }

        public void displayHeaderInfo()
        {
            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(MainActivity));
                intent.PutExtra("getInstallerId", installerId.ToString());
                StartActivity(intent);
            };
            headerGeneralInfo.textViewGeneral.Text = "Jobs";
            headerGeneralInfo.imgViewIcon.SetImageResource(Resource.Drawable.ToolBox48);
        }

        public void displayWebServiceInfo(FrendelWebService.phonegap serviceInstaller)
        {
            try
            {
                installerId = int.Parse(Intent.GetStringExtra("getInstallerId"));
                var serviceListInstallerInfo = serviceInstaller.InsKP_GetInstaller(installerId);
                for (int i = 0; i < serviceListInstallerInfo.Length; i++)
                {
                    if(serviceListInstallerInfo[i].InstallerJobStatus != 2)
                    {
                        var fillInstallerProperties = new InstallerInfoList
                        {
                            Company = serviceListInstallerInfo[i].Company,
                            Project = serviceListInstallerInfo[i].Project,
                            CSID = serviceListInstallerInfo[i].CSID,
                            Lot = serviceListInstallerInfo[i].Lot,
                            JobNum = serviceListInstallerInfo[i].MasterNum.ToString().Substring(6),
                            MasterNum = serviceListInstallerInfo[i].MasterNum,
                            ShippedDone = serviceListInstallerInfo[i].ShippedDone,
                            InstallerJobStatus = serviceListInstallerInfo[i].InstallerJobStatus,
                            InstallerJobStart = serviceListInstallerInfo[i].InstallerJobStart,
                            InstallerJobComplete = serviceListInstallerInfo[i].InstallerJobComplete
                        };
                        lstInstallerInfoClass.Add(fillInstallerProperties);
                    }
                }
                listView = FindViewById<ListView>(Resource.Id.lstInstallerInfo);
                // populate the listview with data
                listView.Adapter = new JobScreenAdapter(this, lstInstallerInfoClass);
                listView.ItemClick += OnListItemClick;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Found :" + ex.ToString());
            }

        }
        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var listView = sender as ListView;
            var t = lstInstallerInfoClass[e.Position];
            string[] str = new string[] { t.Company, t.Project, t.Lot, t.JobNum, t.MasterNum, t.ShippedDone, t.CSID.ToString(), t.InstallerJobStatus.ToString(), t.InstallerJobStart, t.InstallerJobComplete };
            Bundle b = new Bundle();
            b.PutStringArray("keySelectedInstaller", str);
            var intent = new Android.Content.Intent(this, typeof(StartJobScheduleStatus));
            intent.PutExtra("getInstallerId", installerId.ToString());
            intent.PutExtras(b);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.JobMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {            
            onRestart();
            return base.OnOptionsItemSelected(item);
        }
        protected void onRestart()
        {
            base.OnRestart();
            var intent = new Android.Content.Intent(this, typeof(JobScreen));
            intent.PutExtra("getInstallerId", installerId.ToString());
            StartActivity(intent);
            Finish();
        }
    }
}