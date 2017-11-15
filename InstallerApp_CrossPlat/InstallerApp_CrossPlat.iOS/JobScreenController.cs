using Foundation;
using System;
using UIKit;

namespace InstallerApp_CrossPlat.iOS
{
    public partial class JobScreenController : UIViewController
    {
        public JobScreenController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnBack.Clicked += delegate (object sender, EventArgs eventArgs)
            {
                ViewController viewController = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                if (viewController != null)
                {
                    this.NavigationController.PushViewController(viewController, true);
                }
            };
        }

    }
}