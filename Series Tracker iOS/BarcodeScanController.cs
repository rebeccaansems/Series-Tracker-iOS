using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Net.Http;
using System.Threading.Tasks;

namespace Series_Tracker_iOS
{
    partial class BarcodeScanController : UIViewController
    {
        public static string ISBN, ImgURL, TitleURL, PubDateURL;
        public static int numberSeries = 3;

        public BarcodeScanController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            b_Scan.TouchUpInside += BarcodeButtonClicked;
            b_Submit.TouchUpInside += SubmitButtonClicked;
        }

        void SubmitButtonClicked(object sender, EventArgs e)
        {
            ISBN = t_ISBN.Text.ToString();

            FindBookInformation();
        }

        async void BarcodeButtonClicked(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var isbn = await scanner.Scan();

            ISBN = isbn.ToString();

            FindBookInformation();
        }

        async void FindBookInformation()
        {
            var client = new HttpClient();
            string GG_url = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + ISBN;
            var GG_Json = await client.GetStringAsync(GG_url);
            ImgURL = getBetween(GG_Json.ToString(), "\"thumbnail\": \"", "\"");
            TitleURL = getBetween(GG_Json.ToString(), "\"title\": \"", "\"");
            PubDateURL = getBetween(GG_Json.ToString(), "\"publishedDate\": \"", "-");

            //get Series information
            string GR_url = "https://www.goodreads.com/book/isbn_to_id/" + ISBN;
            var GR_html = await client.GetStringAsync(GR_url);
            string GR_SeriesCode = getBetween(GR_html, "<meta property=\"og:url\" content=\"https://www.goodreads.com/work/best_book/", "\"/>");

            GR_url = "https://www.goodreads.com/work/" + GR_SeriesCode + "/series?format=xml&key=" + Config.GR_Key;
            var GR_XML = await client.GetStringAsync(GR_url);
            string GR_SeriesID = getBetween(GR_XML, "<series>", "<title>");
            GR_SeriesID = getBetween(GR_SeriesID, "<id>", "</id>");

            GR_url = "https://www.goodreads.com/series/" + GR_SeriesID + "?format=xml&key=" + Config.GR_Key;
            GR_XML = await client.GetStringAsync(GR_url);
            numberSeries = Int32.Parse(getBetween(GR_XML, "<series_works_count>", "</series_works_count>"));
            
            this.PerformSegue("ScanComplete", this);
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
