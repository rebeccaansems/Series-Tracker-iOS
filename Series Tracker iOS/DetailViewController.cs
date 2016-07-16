using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace Series_Tracker_iOS
{
    public partial class DetailViewController : UIViewController
    {
        public object DetailItem { get; set; }

        public DetailViewController(IntPtr handle) : base(handle)
        {
        }

        public void SetDetailItem(object newDetailItem)
        {
            if (DetailItem != newDetailItem)
            {
                DetailItem = newDetailItem;

                // Update the view
                ConfigureView();
            }
        }

        void ConfigureView()
        {
            // Update the user interface for the detail item
            if (IsViewLoaded && DetailItem != null)
            {
                if (!DetailItem.ToString().Equals("YO"))
                {
                    i_Image.Image = FromUrl(BarcodeScanController.ImgURL);
                }
                else
                {
                    i_Image.Image = FromUrl("http://books.google.ca/books/content?id=JlOgAwAAQBAJ&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");
                }
            }

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            ConfigureView();
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


