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
	[Register ("BarcodeScanController")]
	partial class BarcodeScanController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton b_Scan { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (b_Scan != null) {
				b_Scan.Dispose ();
				b_Scan = null;
			}
		}
	}
}
