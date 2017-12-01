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

namespace InstallerApp_CrossPlat.Droid
{
    public class IndividualRoomAdapter : BaseAdapter<IndividualRoomList>
    {
        List<IndividualRoomList> items;
        Activity context;

        public IndividualRoomAdapter(Activity context, List<IndividualRoomList> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override IndividualRoomList this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get{ return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomIndividualRoom, null);

            if (position == 0)
            {
                view.FindViewById<TextView>(Resource.Id.txtStyleColor).Text = item.Style + " / " + item.Colour;
                view.FindViewById<TextView>(Resource.Id.txtHardware).Text = item.Hardware;
                view.FindViewById<TextView>(Resource.Id.txtCounterTop).Text = item.CounterTop;
                view.FindViewById<ImageView>(Resource.Id.Image).SetBackgroundResource(Resource.Drawable.Barcode128);
            }
            else if (position == 1)
            {
                view.FindViewById<TextView>(Resource.Id.txtStyleColor).Text = item.Style + " " + item.Colour;
                view.FindViewById<TextView>(Resource.Id.txtHardware).Text = item.Hardware;
                view.FindViewById<TextView>(Resource.Id.txtCounterTop).Text = item.CounterTop;
                view.FindViewById<ImageView>(Resource.Id.Image).SetBackgroundResource(Resource.Drawable.CameraDigital128);
                view.FindViewById<TextView>(Resource.Id.txtImage).Text = "PHOTOS";
                view.FindViewById<TextView>(Resource.Id.txtPartsOrdered).Text = "";
                view.FindViewById<TextView>(Resource.Id.txtPartsCount).Text = item.installationPhoto.ToString();
            }
            return view;
        }

    }
}