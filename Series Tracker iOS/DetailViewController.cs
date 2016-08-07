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

            NavigationItem.Title = BarcodeScanController.k_TitleURL[itemSelected];

            t_Title.Text = BarcodeScanController.k_TitleURL[itemSelected];
            i_Image.Image = FromUrl(BarcodeScanController.k_ImgURL[itemSelected]);
            t_ISBN.Text = "ISBN: "+BarcodeScanController.k_isbnURL[itemSelected];

            string description = BarcodeScanController.k_DescriptionsURL[itemSelected];
            description = description.Replace("<italics>", "").Replace("</italics>", "");
            description = description.Replace("<strong>", "").Replace("</strong>", "");
            description = description.Replace("<em>", "").Replace("</em>", "");
            description = description.Replace("<p>", "").Replace("</p>", "");
            description = description.Replace("<br>", "\n");
            t_Description.Text = description;

            t_Description.Font = UIFont.FromName("Helvetica", 16f);
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


