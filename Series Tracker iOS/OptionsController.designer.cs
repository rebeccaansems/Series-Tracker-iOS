// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Series_Tracker_iOS
{
	[Register ("OptionsController")]
	partial class OptionsController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem b_BarOptions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem b_BarScan { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch b_IncludeAllBooks { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch b_ShowBookCovers { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch b_ShowPublicationDates { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBar v_TabBar { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (b_BarOptions != null) {
				b_BarOptions.Dispose ();
				b_BarOptions = null;
			}
			if (b_BarScan != null) {
				b_BarScan.Dispose ();
				b_BarScan = null;
			}
			if (b_IncludeAllBooks != null) {
				b_IncludeAllBooks.Dispose ();
				b_IncludeAllBooks = null;
			}
			if (b_ShowBookCovers != null) {
				b_ShowBookCovers.Dispose ();
				b_ShowBookCovers = null;
			}
			if (b_ShowPublicationDates != null) {
				b_ShowPublicationDates.Dispose ();
				b_ShowPublicationDates = null;
			}
			if (v_TabBar != null) {
				v_TabBar.Dispose ();
				v_TabBar = null;
			}
		}
	}
}