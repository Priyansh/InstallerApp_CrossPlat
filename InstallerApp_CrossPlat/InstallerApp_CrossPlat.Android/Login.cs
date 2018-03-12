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
    [Activity(Label = "InstallerApp", Icon = "@drawable/FrendelLogo", MainLauncher = true)]
    public class Login : Activity
    {
        EditText txtUsername;
        EditText txtPassword;
        TextView lblErrorMsg;
        Button btnLogin;
        ProgressDialog dialog;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);
            btnLogin = FindViewById<Button>(Resource.Id.btnlogin);
            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            lblErrorMsg = FindViewById<TextView>(Resource.Id.lblErrorMsg);
            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text)))
            {
                dialog = ProgressDialog.Show(this, "", "Authenticating...", true);
                dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(async delegate
                {
                    await Task.Delay(20);
                    RunOnUiThread(() =>
                    {
                        int installerId = serviceInstaller.InsKP_Login(txtUsername.Text, txtPassword.Text);
                        dialog.Dismiss();
                        if (installerId == 0)
                        {
                            lblErrorMsg.Text = "Invalid UserName/Password";
                        }
                        else
                        {
                            var intent = new Android.Content.Intent(this, typeof(MainActivity));
                            StartActivity(intent);
                            Finish();
                        }
                    });
                })).Start();
            }
            else
            {
                lblErrorMsg.Text = "Invalid UserName/Password";
            }
        }
    }
}