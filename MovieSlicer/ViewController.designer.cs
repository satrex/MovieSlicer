// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MovieSlicer
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField ClickedLabel { get; set; }

		[Outlet]
		AppKit.NSImageView EndImage { get; set; }

		[Outlet]
		AppKit.NSTextField endTimeLabel { get; set; }

		[Outlet]
		AppKit.NSSliderTouchBarItem LocationSlider { get; set; }

		[Outlet]
		AVKit.AVPlayerView MoviePlayer { get; set; }

		[Outlet]
		AppKit.NSImageView StartImage { get; set; }

		[Outlet]
		AppKit.NSTextField startTimeLabel { get; set; }

		[Action ("ClickedButton:")]
		partial void ClickedButton (Foundation.NSObject sender);

		[Action ("ClickedEndButton:")]
		partial void ClickedEndButton (Foundation.NSObject sender);

		[Action ("EndToStartClicked:")]
		partial void EndToStartClicked (Foundation.NSObject sender);

		[Action ("LocationSliderSlided:")]
		partial void LocationSliderSlided (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ClickedLabel != null) {
				ClickedLabel.Dispose ();
				ClickedLabel = null;
			}

			if (endTimeLabel != null) {
				endTimeLabel.Dispose ();
				endTimeLabel = null;
			}

			if (LocationSlider != null) {
				LocationSlider.Dispose ();
				LocationSlider = null;
			}

			if (MoviePlayer != null) {
				MoviePlayer.Dispose ();
				MoviePlayer = null;
			}

			if (StartImage != null) {
				StartImage.Dispose ();
				StartImage = null;
			}

			if (EndImage != null) {
				EndImage.Dispose ();
				EndImage = null;
			}

			if (startTimeLabel != null) {
				startTimeLabel.Dispose ();
				startTimeLabel = null;
			}
		}
	}
}
