using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Series_Tracker_iOS
{
    public partial class MasterViewController : UITableViewController
    {
        DataSource dataSource;

        public MasterViewController(IntPtr handle) : base(handle)
        {
            Title = NSBundle.MainBundle.LocalizedString("Master", "Master");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            TableView.Source = dataSource = new DataSource(this);

            dataSource.Objects.Insert(0, BarcodeScanController.ISBN??"yo");

            dataSource.Objects.Insert(0, "YO");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "showDetail")
            {
                var indexPath = TableView.IndexPathForSelectedRow;
                var item = dataSource.Objects[indexPath.Row];

                ((DetailViewController)segue.DestinationViewController).SetDetailItem(item);
            }
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("Cell");
            readonly List<object> objects = new List<object>();
            readonly MasterViewController controller;

            public DataSource(MasterViewController controller)
            {
                this.controller = controller;
            }

            public IList<object> Objects
            {
                get { return objects; }
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return objects.Count;
            }

            public static string ImageURL, ISBN;

            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath);

                cell.TextLabel.Text = objects[indexPath.Row].ToString();
                cell.DetailTextLabel.Text = "sup";

                if (!objects[indexPath.Row].ToString().Equals("YO"))
                {
                    cell.ImageView.Image = FromUrl(BarcodeScanController.ImgURL);
                }
                else
                {
                    cell.ImageView.Image = FromUrl("http://books.google.ca/books/content?id=JlOgAwAAQBAJ&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");
                }

                return cell;
            }
        }

        // ------------------------------------------------------
        
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

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}

