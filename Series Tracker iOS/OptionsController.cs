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

            TabBar.SelectedItem = b_BarOptions;

            b_IncludeAllBooks.On = BarcodeScanController.showAllBooks;
            b_ShowPublicationDates.On = BarcodeScanController.showPublicationDates;
            b_ShowBookCovers.On = BarcodeScanController.showBookCovers;

            b_IncludeAllBooks.ValueChanged += BooksChanged;
            b_ShowPublicationDates.ValueChanged += ShowPublicationDates;
            b_ShowBookCovers.ValueChanged += ShowBookCovers;
            TabBar.ItemSelected += TabBarSelected;
        }

        void TabBarSelected(object sender, EventArgs e)
        {
            this.PerformSegue("ScanPressed", this);
        }

        public void BooksChanged(object sender, EventArgs e)
        {
            BarcodeScanController.showAllBooks = b_IncludeAllBooks.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_IncludeAllBooks.On, "showAllBooks");
        }

        public void ShowPublicationDates(object sender, EventArgs e)
        {
            BarcodeScanController.showPublicationDates = b_ShowPublicationDates.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_ShowPublicationDates.On, "showPublicationDates");
        }

        public void ShowBookCovers(object sender, EventArgs e)
        {
            BarcodeScanController.showBookCovers = b_ShowBookCovers.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_ShowBookCovers.On, "showBookCovers");
        }
    }
}
