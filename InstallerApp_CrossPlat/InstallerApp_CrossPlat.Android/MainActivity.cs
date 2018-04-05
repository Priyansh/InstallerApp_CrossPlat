using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace InstallerApp_CrossPlat.Droid
{
    //[Activity(Label = "InstallerApp", Icon = "@drawable/FrendelLogo", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, MainLauncher = true)]
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        ImageButton imgbtnJobs;
        public byte[] GetFile { get; private set; }
        int installerId;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            //TODO Call REST API
            getAllInstallers("http://192.168.3.135:3000");
            
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
                installerId = int.Parse(Intent.GetStringExtra("getInstallerId"));
                intent.PutExtra("getInstallerId", installerId.ToString());
                StartActivity(intent);
                Finish();
            };
        }

        public async Task<bool> getAllInstallers(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage resMsg = await httpClient.GetAsync("api/installers");
                //Checking the response is successful or not which is sent using HttpClient
                if (resMsg.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var resp = resMsg.Content.ReadAsStringAsync().Result;
                    List<InstallerInfoList> lst = JsonConvert.DeserializeObject<List<InstallerInfoList>>(resp);
                }
                var response = await httpClient.GetStringAsync(url);
                //List<InstallerInfoList> lst = JsonConvert.DeserializeObject<List<InstallerInfoList>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.ToString());
                return false;
            }

            return true;
        }

    } //End MainActivity
}


