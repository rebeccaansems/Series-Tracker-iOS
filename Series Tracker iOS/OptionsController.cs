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

            NavigationItem.HidesBackButton = true;
            NavigationItem.Title = "Options";

            TabBar.SelectedItem = b_BarOptions;
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(227, 118, 2);
            
            b_IncludeAllBooks.On = BarcodeScanController.k_showAllBooks;
            b_ShowPublicationDates.On = BarcodeScanController.k_showPublicationDates;
            b_ShowBookCovers.On = BarcodeScanController.k_showBookCovers;

            b_IncludeAllBooks.ValueChanged += BooksChanged;
            b_ShowPublicationDates.ValueChanged += ShowPublicationDates;
            b_ShowBookCovers.ValueChanged += ShowBookCovers;

            TabBar.ItemSelected += TabBarSelected;
        }

        void TabBarSelected(object sender, EventArgs e)
        {
            if (TabBar.SelectedItem == b_BarScan)
            {
                this.PerformSegue("ScanPressed", this);
            }
        }

        public void BooksChanged(object sender, EventArgs e)
        {
            BarcodeScanController.k_showAllBooks = b_IncludeAllBooks.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_IncludeAllBooks.On, "showAllBooks");
        }

        public void ShowPublicationDates(object sender, EventArgs e)
        {
            BarcodeScanController.k_showPublicationDates = b_ShowPublicationDates.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_ShowPublicationDates.On, "showPublicationDates");
        }

        public void ShowBookCovers(object sender, EventArgs e)
        {
            BarcodeScanController.k_showBookCovers = b_ShowBookCovers.On;
            NSUserDefaults.StandardUserDefaults.SetBool(b_ShowBookCovers.On, "showBookCovers");
        }
    }
}
