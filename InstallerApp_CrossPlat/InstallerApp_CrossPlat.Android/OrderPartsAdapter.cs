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
    public class OrderPartsAdapter : BaseAdapter<PartsIssueList>
    {
        List<PartsIssueList> PartsIssueList;
        Activity context;

        public OrderPartsAdapter(Activity context, List<PartsIssueList> PartsIssueList) : base()
        {
            this.context = context;
            this.PartsIssueList = PartsIssueList;
        }


        public override PartsIssueList this[int position]
        {
            get { return PartsIssueList[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return PartsIssueList.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var PartsIssue = PartsIssueList[position];
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomOrderParts, null);
            }
            CheckBox cbOrderParts = view.FindViewById<CheckBox>(Resource.Id.cbOrderParts);
            cbOrderParts.Id = PartsIssue.PartIssueListID;
            cbOrderParts.Checked = PartsIssue.IsCbSelected;
            view.FindViewById<TextView>(Resource.Id.txtOrderPartName).Text = PartsIssue.PartDescription;

            cbOrderParts.CheckedChange += CbOrderParts_CheckedChange;
            return view;
        }

        private void CbOrderParts_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var obj = sender as CheckBox;
            var row = obj?.Parent as View;
            var parent = row?.Parent as ListView;

            if (parent == null)
                return;

            var position = parent.GetPositionForView(row);

            var item = PartsIssueList[position];
            item.IsCbSelected = e.IsChecked;
        }

        public List<int> GetCheckedItems()
        {
            return PartsIssueList
                    .Where(a => a.IsCbSelected)
                    .Select(b => b.PartIssueListID)
                    .ToList();
        }
    }
    
}