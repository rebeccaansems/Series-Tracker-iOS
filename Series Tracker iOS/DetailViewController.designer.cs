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
	[Register ("DetailViewController")]
	partial class DetailViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView i_Image { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView t_Description { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel t_Title { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (i_Image != null) {
				i_Image.Dispose ();
				i_Image = null;
			}
			if (t_Description != null) {
				t_Description.Dispose ();
				t_Description = null;
			}
			if (t_Title != null) {
				t_Title.Dispose ();
				t_Title = null;
			}
		}
	}
}
