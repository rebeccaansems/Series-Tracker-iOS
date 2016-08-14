using Foundation;
using Newtonsoft.Json;
using System.Collections.Generic;
using UIKit;

namespace Series_Tracker_iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            // Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif
            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void OnActivated(UIApplication application)
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                if (UIScreen.MainScreen.Bounds.Size.Height == 480) //iPhone 4
                {
                    Window = new UIWindow(UIScreen.MainScreen.Bounds);
                    UIStoryboard board = UIStoryboard.FromName("Main4", null);

                    UIViewController rootView = (UIViewController)board.InstantiateViewController("NavigationController");
                    if (NSUserDefaults.StandardUserDefaults.BoolForKey("saveMVC"))
                    {
                        rootView = (UIViewController)board.InstantiateViewController("MasterViewController");
                    }

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
                else if (UIScreen.MainScreen.Bounds.Size.Height == 568) //iPhone 5
                {
                    Window = new UIWindow(UIScreen.MainScreen.Bounds);
                    UIStoryboard board = UIStoryboard.FromName("Main5", null);

                    UIViewController rootView = (UIViewController)board.InstantiateViewController("NavigationController");

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
                else if (UIScreen.MainScreen.Bounds.Size.Height == 667) //iPhone 6
                {
                    Window = new UIWindow(UIScreen.MainScreen.Bounds);
                    UIStoryboard board = UIStoryboard.FromName("Main6", null);

                    UIViewController rootView = (UIViewController)board.InstantiateViewController("NavigationController");
                    if (NSUserDefaults.StandardUserDefaults.BoolForKey("saveMVC"))
                    {
                        rootView = (UIViewController)board.InstantiateViewController("MasterViewController");
                    }

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
                else if (UIScreen.MainScreen.Bounds.Size.Height == 736) //iPhone 6+
                {
                    Window = new UIWindow(UIScreen.MainScreen.Bounds);
                    UIStoryboard board = UIStoryboard.FromName("Main6P", null);

                    UIViewController rootView = (UIViewController)board.InstantiateViewController("NavigationController");
                    if (NSUserDefaults.StandardUserDefaults.BoolForKey("saveMVC"))
                    {
                        rootView = (UIViewController)board.InstantiateViewController("MasterViewController");
                    }

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
            }
        }

        public override void DidEnterBackground(UIApplication application)
        {
            //save state so going into background doesn't go back to main screen
            if (NSUserDefaults.StandardUserDefaults.BoolForKey("saveMVC"))
            {
                NSUserDefaults.StandardUserDefaults.SetString(BarcodeScanController.k_SeriesName, "seriesName");
                NSUserDefaults.StandardUserDefaults.SetString(BarcodeScanController.k_ScannedBookName, "scannedBookName");

                NSUserDefaults.StandardUserDefaults.SetInt(BarcodeScanController.k_numberSeries, "numberSeries");

                string titleURL = JsonConvert.SerializeObject(BarcodeScanController.k_TitleURL);
                string pupDateURL = JsonConvert.SerializeObject(BarcodeScanController.k_PubDateURL);
                string imgURL = JsonConvert.SerializeObject(BarcodeScanController.k_ImgURL);
                string isbnURL = JsonConvert.SerializeObject(BarcodeScanController.k_isbnURL);
                string descriptionURL = JsonConvert.SerializeObject(BarcodeScanController.k_DescriptionsURL);

                NSUserDefaults.StandardUserDefaults.SetString(titleURL, "titleURL");
                NSUserDefaults.StandardUserDefaults.SetString(pupDateURL, "pupDateURL");
                NSUserDefaults.StandardUserDefaults.SetString(imgURL, "imgURL");
                NSUserDefaults.StandardUserDefaults.SetString(isbnURL, "isbnURL");
                NSUserDefaults.StandardUserDefaults.SetString(descriptionURL, "descriptionURL");
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(false, "saveMVC");
        }
    }
}


