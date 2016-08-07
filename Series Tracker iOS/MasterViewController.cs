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

            NavigationItem.Title = BarcodeScanController.k_SeriesName;
            TableView.Source = dataSource = new DataSource(this);

            for (int i = (BarcodeScanController.k_numberSeries - 1); i >= 0; i--)
            {
                dataSource.Objects.Insert(0, BarcodeScanController.k_TitleURL[i]);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "showDetail")
            {
                var indexPath = TableView.IndexPathForSelectedRow;
                var item = dataSource.Objects[indexPath.Row];

                ((DetailViewController)segue.DestinationViewController).SetDetailItem(indexPath.Row);
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

            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath);

                cell.TextLabel.Text = objects[indexPath.Row].ToString();

                if (BarcodeScanController.k_showPublicationDates)
                {
                    cell.DetailTextLabel.Text = BarcodeScanController.k_PubDateURL[indexPath.Row];
                }
                else
                {
                    cell.DetailTextLabel.Text = "";
                }

                if (BarcodeScanController.k_showBookCovers)
                {
                    cell.ImageView.Image = FromUrl(BarcodeScanController.k_ImgURL[indexPath.Row]);
                }
                else
                {
                    cell.ImageView.Image = null;
                }

                if(BarcodeScanController.k_ScannedBookName != null)
                {
                    if (BarcodeScanController.k_ScannedBookName.Equals(objects[indexPath.Row].ToString()))
                    {
                        cell.BackgroundColor = UIColor.FromRGB(255, 237, 217);
                    }
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

