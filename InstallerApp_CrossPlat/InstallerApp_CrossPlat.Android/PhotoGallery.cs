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

using Android.Provider;
using Android.Content.PM;
using Java.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace InstallerApp_CrossPlat.Droid
{
    [Activity(Label = "",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PhotoGallery : Activity
    {
        List<InstallerInfoList> lstInstallerInfoClass = new List<InstallerInfoList>();
        ListView listViewInstallerInfo;
        List<byte[]> lstByteArryImages;
        List<BitmapImagesList> lstBitmapImagesClass;
        string[] getstringRooms, getSelectedInstaller;
        TextView textViewRoomInfo;
        File file;
        File dir;
        Bitmap bitmap;
        byte[] imageBytes;
        FrendelWebService.phonegap serviceInstaller = new FrendelWebService.phonegap();
        ProgressDialog progressDialog;
        int alertDialog = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PhotoGallery);
            getstringRooms = Intent.GetStringArrayExtra("keyRoomInfo");
            getSelectedInstaller = Intent.GetStringArrayExtra("keyselectedInstaller");
            
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
            ThreadPool.QueueUserWorkItem(q => longRunningMethod());
            //longRunningMethod();
            footerIconClick();
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
            /*serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
            displayWebServiceInfo(serviceInstaller);*/

            new Thread(new ThreadStart(async delegate
            {
                await Task.Delay(50);
                RunOnUiThread(() =>
                {
                    serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
                    displayWebServiceInfo(serviceInstaller);
                    //progressDialog.Dismiss();
                });
            })).Start();

            /*Thread.Sleep(3000);
            RunOnUiThread(() =>
            {
                serviceInstaller.Url = "http://ws.frendel.com/mobile/phonegap.asmx";
                displayWebServiceInfo(serviceInstaller);
                progressDialog.Dismiss();
            });*/
        }

        public void footerIconClick()
        {
            csFooterGeneralInfo footerGeneralInfo = new csFooterGeneralInfo(this);
            if (IsThereAnAppToTakePictures())
            {
                dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "InstallerAppPics");
                if (!dir.Exists())
                {
                    dir.Mkdirs();
                }
            }
            footerGeneralInfo.footerImgBtnCamera.Click += delegate
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                file = new File(dir, System.String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
                StartActivityForResult(intent,0);
                /*new Thread(new ThreadStart(async delegate
                {
                    await Task.Delay(50);
                    ProgressDialog camProgressDialog = ProgressDialog.Show(this, "Loading...", "Please Wait!!!", true);
                    camProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                    RunOnUiThread(() =>
                    {
                        camProgressDialog.Dismiss();
                    });
                })).Start(); */
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                ProgressDialog camProgressDialog = ProgressDialog.Show(this, "Loading...", "Please Wait!!!", true);
                camProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                new Thread(new ThreadStart(async delegate
                {
                    await Task.Delay(50);
                    base.OnActivityResult(requestCode, resultCode, data);
                    //Make it available in the gallery
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri = Android.Net.Uri.FromFile(file);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);

                    //Resize bitmap image & Display in grid ImageView
                    int height = 350;
                    int width = 350;
                    bitmap = file.Path.LoadAndResizeBitmap(width, height);
                    GC.Collect();

                    //Convert bitmap into byte[]
                    using (var stream = new System.IO.MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 80, stream);
                        imageBytes = stream.ToArray();
                    }

                    //Insert photos in DB and fetch info from DB at same time
                    //Store images in bitmap array, then upload to Grid
                    lstByteArryImages = new List<byte[]>();
                    lstByteArryImages = serviceInstaller.insKP_InsertInstallerImages(getstringRooms[1], imageBytes, getstringRooms[0]).ToList<byte[]>();

                    RunOnUiThread(() =>
                    {
                        if (lstByteArryImages.Count > 0)
                        {
                            //Fillgrid
                            uploadImagesGrid();
                            camProgressDialog.Dismiss();
                        }
                    });

                })).Start();
            }

        }

        public void uploadImagesGrid()
        {
            //Grid Images code
            var gridPhotoGallery = FindViewById<GridView>(Resource.Id.gridPhotoGallery);
            lstBitmapImagesClass = new List<BitmapImagesList>();
            foreach (var imageData in lstByteArryImages)
            {
                var bmp = new BitmapImagesList
                {
                    bitmapImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length)
                };
                lstBitmapImagesClass.Add(bmp);
            }
            gridPhotoGallery.Adapter = new PhotoGalleryAdapter(this, lstBitmapImagesClass);
            gridPhotoGallery.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                View viewGridItem = args.View; //view from gridPhotoGallery
                View viewCustom = LayoutInflater.Inflate(Resource.Layout.CustomDialogBox, null); //view from CustomDialogBox

                if(viewGridItem != null && alertDialog == 0){
                    Bitmap bmp = Bitmap.CreateBitmap(viewGridItem.Width, viewGridItem.Height, Bitmap.Config.Argb8888);
                    Canvas canvas = new Canvas(bmp);
                    Drawable bgDrawable = viewGridItem.Background;
                    if (bgDrawable != null)
                        bgDrawable.Draw(canvas);
                    else
                        canvas.DrawColor(Android.Graphics.Color.White);
                    viewGridItem.Draw(canvas);

                    ImageView iv = viewCustom.FindViewById<ImageView>(Resource.Id.imageViewCustom);
                    iv.SetPadding(0, 0, 0, 0);
                    iv.SetImageBitmap(bmp);

                    AlertDialog builder = new AlertDialog.Builder(this).Create();
                    builder.SetView(viewCustom, 0, 0, 0, 0);
                    builder.SetCanceledOnTouchOutside(false);
                    Button button = viewCustom.FindViewById<Button>(Resource.Id.btnCancel);
                    button.Click += delegate
                    {
                        builder.Dismiss();
                        alertDialog = 0;
                    };
                    builder.Show();
                    alertDialog = 1;
                }
            };
        }

        //When physical back button pressed
        //public override void OnBackPressed()
        //{
        //    alertDialog = 0;
        //    System.Console.WriteLine("AlertDialog : " + alertDialog);
        //    base.OnBackPressed();
        //}

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            switch (keyCode)
            {
                case Keycode.Back:
                    alertDialog = 0;
                    return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        public bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
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
                    //Populate Installer Images based on Room i.e. Kitchen / Main / Ensuit
                    lstByteArryImages = serviceInstaller.insKP_getInstallerImages(getstringRooms[0]).ToList<byte[]>();
                    uploadImagesGrid();
                    progressDialog.Dismiss();
                });
            })).Start();
        }
    }
}