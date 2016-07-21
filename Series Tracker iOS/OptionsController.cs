using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Xamarin.Forms;

namespace Series_Tracker_iOS
{
	partial class OptionsController : UIViewController
	{
		public OptionsController (IntPtr handle) : base (handle)
		{
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            b_IncludeAllBooks.ValueChanged += BooksChanged;
            b_ShowPublicationDates.ValueChanged += ShowPublicationDates;
            b_ShowBookCovers.ValueChanged += ShowBookCovers;
        }

        public void BooksChanged(object sender, EventArgs e)
        {
            BarcodeScanController.showAllBooks = b_IncludeAllBooks.On;
        }

        public void ShowPublicationDates(object sender, EventArgs e)
        {
            BarcodeScanController.showPublicationDates = b_ShowPublicationDates.On;
        }

        public void ShowBookCovers(object sender, EventArgs e)
        {
            BarcodeScanController.showBookCovers = b_ShowBookCovers.On;
        }
    }
}
