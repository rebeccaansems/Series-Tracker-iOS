using Foundation;
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

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
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

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
                else if (UIScreen.MainScreen.Bounds.Size.Height == 736) //iPhone 6+
                {
                    Window = new UIWindow(UIScreen.MainScreen.Bounds);
                    UIStoryboard board = UIStoryboard.FromName("Main6P", null);
                    UIViewController rootView = (UIViewController)board.InstantiateViewController("NavigationController");

                    Window.RootViewController = rootView;
                    Window.MakeKeyAndVisible();
                }
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


