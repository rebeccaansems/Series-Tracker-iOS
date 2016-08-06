using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UIKit;
using System.Net.Http;
using System.Threading.Tasks;

namespace Series_Tracker_iOS
{
    partial class BarcodeScanController : UIViewController
    {

        public static string k_ISBN, k_SeriesName;
        public static List<string> k_TitleURL = new List<string>();
        public static List<string> k_ImgURL = new List<string>();
        public static List<string> k_PubDateURL = new List<string>();
        public static List<string> k_isbnURL = new List<string>();
        public static List<string> k_DescriptionsURL = new List<string>();
        public static int k_numberSeries;

        public static bool k_showAllBooks = false, k_showPublicationDates = true, k_showBookCovers = true;

        private bool userTypedISBN = false;

        public BarcodeScanController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.HidesBackButton = true;

            TabBar.SelectedItem = b_BarScan;
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(227, 118, 2);

            b_Scan.TouchUpInside += BarcodeButtonClicked;
            b_Submit.TouchUpInside += SubmitButtonClicked;
            TabBar.ItemSelected += TabBarSelected;
            
            b_Spinner.Hidden = true;

            k_showAllBooks = NSUserDefaults.StandardUserDefaults.BoolForKey("showAllBooks");
            k_showPublicationDates = NSUserDefaults.StandardUserDefaults.BoolForKey("showPublicationDates");
            k_showBookCovers = NSUserDefaults.StandardUserDefaults.BoolForKey("showBookCovers");

            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false; //for iOS5
            View.AddGestureRecognizer(g);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            b_Submit.Enabled = true;
            b_Scan.Enabled = true;
            b_BarOptions.Enabled = true;
        }

        void SubmitButtonClicked(object sender, EventArgs e)
        {
            k_ISBN = t_InputISBN.Text;

            userTypedISBN = true;

            FindBookInformation();
        }

        void TabBarSelected(object sender, EventArgs e)
        {
            if (TabBar.SelectedItem == b_BarOptions)
            {
                this.PerformSegue("OptionsPressed", this);
            }
        }

        async void BarcodeButtonClicked(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var isbn = await scanner.Scan();

            userTypedISBN = false;

            if (isbn != null)
            {
                k_ISBN = isbn.ToString();
                FindBookInformation();
            }
        }

        async void FindBookInformation()
        {
            b_Submit.Enabled = false;
            b_Scan.Enabled = false;
            b_BarOptions.Enabled = false;

            b_Spinner.Hidden = false;
            b_Spinner.StartAnimating();

            k_TitleURL = new List<string>();
            k_ImgURL = new List<string>();
            k_PubDateURL = new List<string>();
            k_DescriptionsURL = new List<string>();

            var client = new HttpClient();

            //get Series information

            string GG_url = "https://www.googleapis.com/books/v1/volumes?q=+isbn:" + k_ISBN;
            string GG_Json = await client.GetStringAsync(GG_url);

            if (GG_Json.Length != 47)
            {
                string GR_url = "https://www.goodreads.com/book/isbn_to_id/" + k_ISBN;
                var GR_html = await client.GetStringAsync(GR_url);
                string GR_SeriesCode = getBetween(GR_html, "<meta property=\"og:url\" content=\"https://www.goodreads.com/work/best_book/", "\"/>");

                GR_url = "https://www.goodreads.com/work/" + GR_SeriesCode + "/series?format=xml&key=" + Config.GR_Key;
                var GR_XML = await client.GetStringAsync(GR_url);
                string GR_SeriesID = getBetween(GR_XML, "<series>", "<title>");
                GR_SeriesID = getBetween(GR_SeriesID, "<id>", "</id>");

                //check to ensure book is part of a series
                if (GR_SeriesID.Equals(""))
                {
                    string title = getBetween(GR_html, "<title>", " by").Replace(System.Environment.NewLine, string.Empty);
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Series Nonexistent",
                        Message = title + " is not part of a book series."
                    };
                    alert.AddButton("OK");
                    alert.Show();

                    b_Spinner.Hidden = true;
                    b_Spinner.StopAnimating();
                }
                else
                {
                    string seriesName = getBetween(GR_XML, "<title>", "</title>");
                    k_SeriesName = seriesName.Remove(0, 10).Replace("]", "").Replace(">", "");

                    GR_url = "https://www.goodreads.com/series/" + GR_SeriesID + "?format=xml&key=" + Config.GR_Key;
                    GR_XML = await client.GetStringAsync(GR_url);

                    if (k_showAllBooks)
                    {
                        k_numberSeries = Int32.Parse(getBetween(GR_XML, "<series_works_count>", "</series_works_count>"));
                    }
                    else
                    {
                        k_numberSeries = Int32.Parse(getBetween(GR_XML, "<primary_work_count>", "</primary_work_count>"));
                    }

                    GR_XML = RemoveTop(GR_XML);

                    BookInformation(GR_XML, k_showAllBooks);

                    b_Spinner.Hidden = true;
                    b_Spinner.StopAnimating();

                    this.PerformSegue("ScanComplete", this);
                }
            }
            else
            {
                UIAlertView alert;

                if (userTypedISBN)
                {
                    alert = new UIAlertView()
                    {
                        Title = "Invalid ISBN",
                        Message = "ISBN must be 13 numbers long, start with 9, and exist, like: 9780439554930."
                    };
                }
                else
                {
                    alert = new UIAlertView()
                    {
                        Title = "Barcode Scan Failed",
                        Message = "Scan failed to register a valid ISBN, please type in the ISBN-13 manually."
                    };
                }

                alert.AddButton("OK");
                alert.Show();

                k_ISBN = null;

                b_Spinner.Hidden = true;
                b_Spinner.StopAnimating();

                b_Submit.Enabled = true;
                b_Scan.Enabled = true;
                b_BarOptions.Enabled = true;
            }
        }

        void BookInformation(string XML, bool includeAll)
        {
            for (int i = 0; i < k_numberSeries; i++)
            {
                k_DescriptionsURL.Add("");
                k_isbnURL.Add("");

                if (getBetween(XML, "<user_position>", "</user_position>").Length <= 2 && !includeAll)
                {
                    GetBookInformation(XML, i);
                }
                else if (!includeAll)
                {
                    i--;
                }
                else
                {
                    GetBookInformation(XML, i);
                }

                XML = RemoveLastBook(XML);
            }
        }

        async void GetBookInformation(string XML, int i)
        {
            k_TitleURL.Add(getBetween(XML, "<title>", " (").Replace("</title>", ""));
            k_ImgURL.Add(getBetween(XML, "<![CDATA[", "]]>"));
            k_PubDateURL.Add(getBetween(XML, "<original_publication_year>", "</original_publication_year>"));

            var client = new HttpClient();

            string XMLFirstPass = getBetween(XML, "<best_book>", "<title>");
            string XMLId = getBetween(XMLFirstPass, "<id>", "</id>");
            string GR_url = "https://www.goodreads.com/book/show/" + XMLId + ".xml?key=" + Config.GR_Key;
            var GR_XML = await client.GetStringAsync(GR_url);

            XMLFirstPass = getBetween(GR_XML, "<isbn13>", "</isbn13>"); ;
            k_isbnURL[i] = getBetween(XMLFirstPass, "<![CDATA[", "]]>");

            XMLFirstPass = getBetween(GR_XML, "<description>", "</description>");
            k_DescriptionsURL[i] = getBetween(XMLFirstPass, "<![CDATA[", "]]>");
        }

        string RemoveTop(string text)
        {
            string finalString = text.Remove(0, text.IndexOf("<series_work>"));
            return finalString;
        }

        string RemoveLastBook(string text)
        {
            string finalString = text.Remove(0, text.IndexOf("</work>") + 7);
            return finalString;
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
