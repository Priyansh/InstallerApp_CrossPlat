using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    public class PartsInfoAdapter : BaseAdapter<PartsInfoList>
    {
        List<PartsInfoList> items;
        Activity context;

        public PartsInfoAdapter(Activity context, List<PartsInfoList> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override PartsInfoList this[int position]
        {
            get { return items[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
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
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomPartInfo, null);
            }

            view.FindViewById<TextView>(Resource.Id.txtCabinetName).Text = item.CabinetName;
            view.FindViewById<TextView>(Resource.Id.txtLRFinish).Text = "Left : " + item.LFinish + "           Right : " + item.RFinish;

            if(item.OrderPartsStatus != 0)
            {
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).Text = "Order Parts:" + item.OrderPartsStatus.ToString();
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).SetTextColor(Android.Graphics.Color.LightGreen);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).Text = "Order Parts >";
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).SetTextColor(Android.Graphics.Color.Brown);
            }
            return view;
        }

    }

}