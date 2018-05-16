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
        List<PartsInfoList> PartsInfoList;
        Activity context;
        CheckBox cbPartsInfo;

        public PartsInfoAdapter(Activity context, List<PartsInfoList> items) : base()
        {
            this.context = context;
            this.PartsInfoList = items;
        }
        public override PartsInfoList this[int position]
        {
            get { return PartsInfoList[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return PartsInfoList.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = PartsInfoList[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomPartInfo, null);
            }

            view.FindViewById<TextView>(Resource.Id.txtCabinetName).Text = item.CabinetName;
            view.FindViewById<TextView>(Resource.Id.txtLRFinish).Text = "Left : " + item.LFinish + "           Right : " + item.RFinish;

            cbPartsInfo = view.FindViewById<CheckBox>(Resource.Id.cbPartsInfo);
            
            if (item.OrderPartsStatus != 0)
            {
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).Text = "Order Parts:" + item.OrderPartsStatus.ToString();
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).Text = "Order Parts >";
                view.FindViewById<TextView>(Resource.Id.txtOrderParts).SetTextColor(Android.Graphics.Color.LightGreen);
            }
            cbPartsInfo.CheckedChange += CbPartsInfo_CheckedChange;
            return view;
        }

        private void CbPartsInfo_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var obj = sender as CheckBox;
            var row = obj?.Parent as View;
            var parent = row?.Parent as ListView;

            if (parent == null)
                return;

            var position = parent.GetPositionForView(row);

            var item = PartsInfoList[position];
            item.IsCbSelected = e.IsChecked;
        }

        public List<PartsInfoList> GetCheckedItems()
        {
            return PartsInfoList
                    .Where(a => a.IsCbSelected == true)
                    .Select(p => new PartsInfoList{
                         CabinetName = p.CabinetName,
                         LFinish = p.LFinish,
                         RFinish = p.RFinish,
                         IsCbSelected = true
                    }).ToList();
        }
    }

}