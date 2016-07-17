using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace Series_Tracker_iOS
{
    public partial class DetailViewController : UIViewController
    {
        public int itemSelected { get; set; }

        public DetailViewController(IntPtr handle) : base(handle)
        {
        }

        public void SetDetailItem(int newItemSelected)
        {
            itemSelected = newItemSelected;
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            t_Title.Text = BarcodeScanController.TitleURL[itemSelected];
            i_Image.Image = FromUrl(BarcodeScanController.ImgURL[itemSelected]);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            {
                using (var data = NSData.FromUrl(url))
                {
                    return UIImage.LoadFromData(data);
                }
            }
        }
    }
}


