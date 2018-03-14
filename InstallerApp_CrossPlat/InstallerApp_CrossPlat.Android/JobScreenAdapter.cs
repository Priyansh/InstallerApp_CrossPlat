using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    public class JobScreenAdapter : BaseAdapter<InstallerInfoList>
    {
        List<InstallerInfoList> items;
        Activity context;

        public JobScreenAdapter(Activity context, List<InstallerInfoList> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override InstallerInfoList this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomJobScreen, null);
            }

            // Only display jobs if InstallerJobStatus != 2
            if (item.InstallerJobStatus != 2) 
            {
                view.FindViewById<TextView>(Resource.Id.txtJobNo).Text = "Job Number: " + item.JobNum;
                view.FindViewById<TextView>(Resource.Id.txtCompany).Text = item.Company;
                view.FindViewById<TextView>(Resource.Id.txtProject).Text = item.Project;
                view.FindViewById<TextView>(Resource.Id.txtLot).Text = "Unit " + item.Lot;

                //Set StatusImages according to Status Change
                if (item.InstallerJobStatus == 0 || String.IsNullOrEmpty(item.InstallerJobStatus.ToString()))
                {
                    view.FindViewById<ImageView>(Resource.Id.Image).SetBackgroundResource(Resource.Drawable.imgSchedule);
                    view.FindViewById<TextView>(Resource.Id.txtJobStatus).Text = "Scheduled";
                    view.FindViewById<TextView>(Resource.Id.txtJobStatus).SetTextColor(Android.Graphics.Color.Red);
                }
                else if (item.InstallerJobStatus == 1)
                {
                    view.FindViewById<ImageView>(Resource.Id.Image).SetBackgroundResource(Resource.Drawable.imgProgress);
                    view.FindViewById<TextView>(Resource.Id.txtJobStatus).Text = "InProgress";
                    view.FindViewById<TextView>(Resource.Id.txtJobStatus).SetTextColor(Android.Graphics.Color.Green);
                }
            }
            return view;
        }

    }

}