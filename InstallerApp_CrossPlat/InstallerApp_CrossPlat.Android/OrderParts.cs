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
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class OrderParts : Activity
    {
        List<InstallerInfoList> lstInstallerInfoClass = new List<InstallerInfoList>();
        List<PartsIssueList> lstPartsIssueClass = new List<PartsIssueList>();
        ListView listViewInstallerInfo, listViewPartIssues;
        string[] getstringRooms, getSelectedInstaller, getSelectedPartsInfo;
        ProgressDialog progressDialog;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        TextView textViewRoomInfo, textViewOrderCabinet;
        Button btnAddOrder;
        CheckBox cbOrderParts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OrderParts);
            getstringRooms = Intent.GetStringArrayExtra("keyRoomInfo");
            getSelectedInstaller = Intent.GetStringArrayExtra("keySelectedInstaller");
            getSelectedPartsInfo = Intent.GetStringArrayExtra("keyPartsInfo");

            //Adding Loading bar
            progressDialog = ProgressDialog.Show(this, "Loading...", "Please wait!!", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            progressDialog.SetCanceledOnTouchOutside(true);

            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            //Display Header Information
            displayHeaderInfo();
            textViewRoomInfo = FindViewById<TextView>(Resource.Id.textViewRoomInfo);
            textViewOrderCabinet = FindViewById<TextView>(Resource.Id.textViewOrderCabinet);
            textViewRoomInfo.Text = getstringRooms[2];
            textViewOrderCabinet.Text = getSelectedPartsInfo[0];
            btnAddOrder = FindViewById<Button>(Resource.Id.btnAddOrder);
            btnAddOrder.Click += BtnAddOrder_Click;
            cbOrderParts = FindViewById<CheckBox>(Resource.Id.cbOrderParts);
            ThreadPool.QueueUserWorkItem(q => longRunningMethod());

        }

        private void BtnAddOrder_Click(object sender, EventArgs e)
        {
            //TODO Insert Records in [InsKP_PartsOrder]
            new Thread(new ThreadStart(async delegate
            {
                await Task.Delay(50);
                RunOnUiThread(() =>
                {
                    int partsOrderID = serviceInstaller.InsKP_PartsOrder(int.Parse(getSelectedPartsInfo[3]), int.Parse(getSelectedPartsInfo[4]), int.Parse(getSelectedPartsInfo[5]));
                    //TODO Bind cbOrderParts First and get selected value from checkbox

                    //TODO Insert Records in [InsKP_PartsOrderIssue]
                });
            })).Start();

        }

        public void displayHeaderInfo()
        {
            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(PartsInfo)).SetFlags(ActivityFlags.ReorderToFront);
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
                    displayWebServiceInfo(serviceInstaller);
                    progressDialog.Dismiss();
                });
            })).Start();
        }

        public void displayWebServiceInfo(FrendelWebService.phonegap serviceInstaller)
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
            listViewInstallerInfo = FindViewById<ListView>(Resource.Id.listInstallerInfo);
            listViewInstallerInfo.Adapter = new JobScreenAdapter(this, lstInstallerInfoClass);

            new Thread(new ThreadStart(async delegate
            {
                await Task.Delay(50);
                RunOnUiThread(() =>
                {
                    var getPartsIssueList = serviceInstaller.InsKP_GetPartIssueList(int.Parse(getSelectedPartsInfo[3]));
                    for (int i = 0; i < getPartsIssueList.Length; i++)
                    {
                        var fillPartsIssueProperties = new PartsIssueList
                        {
                            PartIssueListID = getPartsIssueList[i].PartIssueListID,
                            PartDescription = getPartsIssueList[i].PartDescription
                        };
                        lstPartsIssueClass.Add(fillPartsIssueProperties);
                    }

                    listViewPartIssues = FindViewById<ListView>(Resource.Id.listPartsIssue);
                    listViewPartIssues.Adapter = new OrderPartsAdapter(this, lstPartsIssueClass);
                });
            })).Start();
            
        }
    }
}