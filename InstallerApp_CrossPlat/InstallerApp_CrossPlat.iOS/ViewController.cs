using System;
using Foundation;
using UIKit;

namespace InstallerApp_CrossPlat.iOS
{
	public partial class ViewController : UIViewController
	{
		int count = 1;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			//btnHello.AccessibilityIdentifier = "myButton";
			//btnHello.TouchUpInside += delegate {
			//	var title = string.Format ("{0} clicks!", count++);
			//	btnHello.SetTitle (title, UIControlState.Normal);
			//};

            //btnShowDetails click event
            btnCalculateBMI.TouchUpInside += BtnCalculateBMI_TouchUpInside;
		}

        private void BtnCalculateBMI_TouchUpInside(object sender, EventArgs e)
        {
            float height = float.Parse(txtHeight.Text);
            float weight = float.Parse(txtWeight.Text);
            float bmi = weight / (height * height);
            lblBmi.Text = bmi.ToString();
        }
        
        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.View.EndEditing(true);
        }
    }
}

