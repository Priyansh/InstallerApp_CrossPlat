﻿using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace InstallerApp_CrossPlat.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PartsInfo : Activity
    {
        List<InstallerInfoList> lstInstallerInfoClass = new List<InstallerInfoList>();
        List<PartsInfoList> lstPartsInfoClass = new List<PartsInfoList>();
        ListView listViewInstallerInfo, listViewPartsInfo;
        string[] getstringRooms, getSelectedInstaller;
        TextView textViewRoomInfo;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        ProgressDialog progressDialog;
        Button btnSubmitOrder;
        private PartsInfoAdapter partsInfoAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PartInfo);
            getstringRooms = Intent.GetStringArrayExtra("keyRoomInfo");
            getSelectedInstaller = Intent.GetStringArrayExtra("keySelectedInstaller");

            //Adding Loading bar
            progressDialog = ProgressDialog.Show(this, "Loading...", "Please wait!!", true);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            progressDialog.SetCanceledOnTouchOutside(true);

            var toolbar = FindViewById<Toolbar>(Resource.Id.HeaderToolbar);
            SetActionBar(toolbar);
            //Display Header Information
            displayHeaderInfo();
            textViewRoomInfo = FindViewById<TextView>(Resource.Id.textViewRoomInfo);
            textViewRoomInfo.Text = getstringRooms[2];
            btnSubmitOrder = FindViewById<Button>(Resource.Id.btnSubmitOrder);
            btnSubmitOrder.Click += BtnSubmitOrder_Click;
            ThreadPool.QueueUserWorkItem(q => longRunningMethod());
        }

        private void BtnSubmitOrder_Click(object sender, System.EventArgs e)
        {
            var checkedItems = partsInfoAdapter.GetCheckedItems();
            if(checkedItems.Count > 0)
            {
                var emailText = new StringBuilder();
                string[] subject = new string[] {"FK #" + lstInstallerInfoClass[0].JobNum, lstInstallerInfoClass[0].Company, lstInstallerInfoClass[0].Project, lstInstallerInfoClass[0].Lot };
                emailText.AppendLine("--- Installer Company ---");
                emailText.AppendLine("");
                emailText.AppendFormat("{0}, {1} \n", lstInstallerInfoClass[0].Company, lstInstallerInfoClass[0].Project);
                emailText.AppendFormat("{0}, {1} \n", lstInstallerInfoClass[0].Lot, lstInstallerInfoClass[0].JobNum);
                emailText.AppendLine("");
                foreach (var items in checkedItems)
                {
                    emailText.AppendFormat("Cabinet : {0} \n", items.CabinetName);
                    emailText.AppendFormat("LFinish : {0}, RFinish : {1} \n", items.LFinish, items.RFinish);
                    emailText.AppendLine("");
                }
                
                Intent email = new Intent(Intent.ActionSend);

                email.PutExtra(Intent.ExtraEmail, new string[] { "installerparts@frendel.com" });
                
                email.PutExtra(Intent.ExtraSubject, string.Join(" / ", subject) );

                email.PutExtra(Intent.ExtraText, emailText.ToString());

                email.SetType("message/rfc822");

                StartActivity(email);
            }
        }

        public void displayHeaderInfo()
        {
            //Adding Header Information
            csHeaderGeneralInfo headerGeneralInfo = new csHeaderGeneralInfo(this);
            headerGeneralInfo.imgbtnBack.Click += delegate
            {
                var intent = new Android.Content.Intent(this, typeof(IndividualRoom)).SetFlags(ActivityFlags.ReorderToFront);
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

            //Populate Parts Info based on FKNO and Room Name i.e. Kitchen / Main / Ensuit
            var partsInfoList = serviceInstaller.InsKP_GetPartInfo(getSelectedInstaller[4].ToString(), getstringRooms[2].ToString());

            for (int i = 0; i < partsInfoList.Length; i++)
            {
                var countOrderPartIssues = serviceInstaller.InsKP_GetPartOrderIssuesCount(partsInfoList[i].PartType, partsInfoList[i].LabelNo, partsInfoList[i].CSID);
                var fillPartsInfoProperties = new PartsInfoList
                {
                    CabinetName = partsInfoList[i].CabinetName,
                    LFinish = partsInfoList[i].LFinish,
                    RFinish = partsInfoList[i].RFinish,
                    PartType = partsInfoList[i].PartType,
                    LabelNo = partsInfoList[i].LabelNo,
                    CSID = partsInfoList[i].CSID,
                    OrderPartsStatus = countOrderPartIssues,
                    IsCbSelected = false
                };
                lstPartsInfoClass.Add(fillPartsInfoProperties);
            }
            listViewPartsInfo = FindViewById<ListView>(Resource.Id.listPartInfo);
            // populate the listview with data
            partsInfoAdapter = new PartsInfoAdapter(this, lstPartsInfoClass);
            listViewPartsInfo.Adapter = partsInfoAdapter;
            listViewPartsInfo.ItemClick += ListViewPartsInfo_ItemClick;
        }

        private void ListViewPartsInfo_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Bundle b = new Bundle();
            b.PutStringArray("keyRoomInfo", getstringRooms);
            b.PutStringArray("keySelectedInstaller", getSelectedInstaller);

            var t = lstPartsInfoClass[e.Position];
            string[] str = new string[] { t.CabinetName, t.LFinish, t.RFinish, t.PartType.ToString(), t.LabelNo.ToString(), t.CSID.ToString() };
            b.PutStringArray("keyPartsInfo", str);

            var intent = new Android.Content.Intent(this, typeof(OrderParts));
            intent.PutExtras(b);
            StartActivity(intent);
        }
    }
}