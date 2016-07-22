using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Series_Tracker_iOS
{
    partial class NavigationController : UINavigationController
    {
        public NavigationController(IntPtr handle) : base(handle)
        {
        }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavBar.TintColor = UIColor.FromRGB(96.5f,96.5f,96.5f);
        }
    }
}
