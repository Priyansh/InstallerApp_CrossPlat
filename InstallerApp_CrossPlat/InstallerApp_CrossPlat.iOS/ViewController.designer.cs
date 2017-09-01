// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace InstallerApp_CrossPlat.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCalculateBMI { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShowDetails { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblBmi { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblWeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtWeight { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCalculateBMI != null) {
                btnCalculateBMI.Dispose ();
                btnCalculateBMI = null;
            }

            if (btnShowDetails != null) {
                btnShowDetails.Dispose ();
                btnShowDetails = null;
            }

            if (lblBmi != null) {
                lblBmi.Dispose ();
                lblBmi = null;
            }

            if (lblHeight != null) {
                lblHeight.Dispose ();
                lblHeight = null;
            }

            if (lblWeight != null) {
                lblWeight.Dispose ();
                lblWeight = null;
            }

            if (txtHeight != null) {
                txtHeight.Dispose ();
                txtHeight = null;
            }

            if (txtWeight != null) {
                txtWeight.Dispose ();
                txtWeight = null;
            }
        }
    }
}