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

        public static string ISBN;
        public static List<string> TitleURL = new List<string>();
        public static List<string> ImgURL = new List<string>();
        public static List<string> PubDateURL = new List<string>();
        public static List<string> DescriptionsURL = new List<string>();
        public static int numberSeries;

        public static bool showAllBooks = true, showPublicationDates = true, showBookCovers = true;

        public BarcodeScanController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.HidesBackButton = true;

            TabBar.SelectedItem = b_BarScan;

            b_Scan.TouchUpInside += BarcodeButtonClicked;
            b_Submit.TouchUpInside += SubmitButtonClicked;
            TabBar.ItemSelected += TabBarSelected;
            
            b_Spinner.Hidden = true;

            showAllBooks = NSUserDefaults.StandardUserDefaults.BoolForKey("showAllBooks");
            showPublicationDates = NSUserDefaults.StandardUserDefaults.BoolForKey("showPublicationDates");
            showBookCovers = NSUserDefaults.StandardUserDefaults.BoolForKey("showBookCovers");

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
            ISBN = t_InputISBN.Text;

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

            if (isbn != null)
            {
                ISBN = isbn.ToString();
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

            TitleURL = new List<string>();
            ImgURL = new List<string>();
            PubDateURL = new List<string>();
            DescriptionsURL = new List<string>();

            var client = new HttpClient();

            //get Series information

            string GG_url = "https://www.googleapis.com/books/v1/volumes?q=+isbn:" + ISBN;
            string GG_Json = await client.GetStringAsync(GG_url);

            if (GG_Json.Length != 47)
            {
                string GR_url = "https://www.goodreads.com/book/isbn_to_id/" + ISBN;
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
                    GR_url = "https://www.goodreads.com/series/" + GR_SeriesID + "?format=xml&key=" + Config.GR_Key;
                    GR_XML = await client.GetStringAsync(GR_url);

                    if (showAllBooks)
                    {
                        numberSeries = Int32.Parse(getBetween(GR_XML, "<series_works_count>", "</series_works_count>"));
                    }
                    else
                    {
                        numberSeries = Int32.Parse(getBetween(GR_XML, "<primary_work_count>", "</primary_work_count>"));
                    }

                    GR_XML = RemoveTop(GR_XML);

                    BookInformation(GR_XML, showAllBooks);

                    b_Spinner.Hidden = true;
                    b_Spinner.StopAnimating();

                    this.PerformSegue("ScanComplete", this);
                }
            }
            else
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Invalid ISBN",
                    Message = "ISBN must be 13 numbers long, start with 9, and exist, like: 9780439023481"
                };
                alert.AddButton("OK");
                alert.AddButton("Retry");
                alert.Show();

                b_Spinner.Hidden = true;
                b_Spinner.StopAnimating();
            }
        }

        void BookInformation(string XML, bool includeAll)
        {
            for (int i = 0; i < numberSeries; i++)
            {
                if (getBetween(XML, "<user_position>", "</user_position>").Length <= 2 && !includeAll)
                {
                    GetBookInformation(XML);
                }
                else if (!includeAll)
                {
                    i--;
                }
                else
                {
                    GetBookInformation(XML);
                }
                XML = RemoveLastBook(XML);
            }
        }

        async void GetBookInformation(string XML)
        {
            TitleURL.Add(getBetween(XML, "<title>", " (").Replace("</title>", ""));
            ImgURL.Add(getBetween(XML, "<![CDATA[", "]]>"));
            PubDateURL.Add(getBetween(XML, "<original_publication_year>", "</original_publication_year>"));

            var client = new HttpClient();

            string XMLFirstPass = getBetween(XML, "<best_book>", "<title>");
            string XMLId = getBetween(XMLFirstPass, "<id>", "</id>");
            string GR_url = "https://www.goodreads.com/book/show/" + XMLId + ".xml?key=" + Config.GR_Key;
            var GR_XML = await client.GetStringAsync(GR_url);
            XMLFirstPass = getBetween(GR_XML, "<description>", "</description>");
            DescriptionsURL.Add(getBetween(XMLFirstPass, "<![CDATA[", "]]>"));
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
