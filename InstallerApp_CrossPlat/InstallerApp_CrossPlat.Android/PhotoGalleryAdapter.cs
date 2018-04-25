using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Views;
using Android.Widget;

namespace InstallerApp_CrossPlat.Droid
{
    public class PhotoGalleryAdapter : BaseAdapter<BitmapImagesList>
    {

        Activity context;
        List<BitmapImagesList> lstBitmapImages;

        public PhotoGalleryAdapter(Activity context, List<BitmapImagesList> lstBitmapImages) : base()
        {
            this.context = context;
            this.lstBitmapImages = lstBitmapImages;
        }

        public override BitmapImagesList this[int position]
        {
            get { return lstBitmapImages[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count {
            get { return lstBitmapImages.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var bitmapImage = lstBitmapImages[position].bitmapImage;
            var widthInDp = lstBitmapImages[position].widthInDp;
            var heightInDp = lstBitmapImages[position].heightInDp;
            ImageView imageView;

            if (convertView == null)
            {
                imageView = new ImageView(context);
                // Grid Images
                if (widthInDp == 360 && heightInDp == 640) //Samsung J3
                {
                    imageView.LayoutParameters = new GridView.LayoutParams(350, 350);
                }
                else //Bigger Devices
                {
                    imageView.LayoutParameters = new GridView.LayoutParams(490, 490);
                }
                imageView.SetScaleType(ImageView.ScaleType.FitXy);
                imageView.SetPadding(10, 10, 10, 10);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            imageView.SetImageBitmap(bitmapImage);
            return imageView;
        }

    }
}