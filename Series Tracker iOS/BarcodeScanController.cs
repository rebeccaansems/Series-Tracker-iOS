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
        public static string ISBN, ImgURL;

		public BarcodeScanController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            b_Scan.TouchUpInside += BarcodeButtonClicked;
        }

        async void BarcodeButtonClicked(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var isbn = await scanner.Scan();

            ISBN = isbn.ToString();

            var client = new HttpClient();
            string url = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + ISBN;
            var GG_Json = await client.GetStringAsync(url);
            ImgURL = getBetween(GG_Json.ToString(), "\"thumbnail\": \"", "\"");

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
