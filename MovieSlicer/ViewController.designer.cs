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
		AppKit.NSButton endButton { get; set; }

		[Outlet]
		AppKit.NSImageView EndImage { get; set; }

		[Outlet]
		AppKit.NSTextField endTimeLabel { get; set; }

		[Outlet]
		AppKit.NSButton exportButton { get; set; }

		[Outlet]
		AppKit.NSButton invertButton { get; set; }

		[Outlet]
		AppKit.NSSliderTouchBarItem LocationSlider { get; set; }

		[Outlet]
		AVKit.AVPlayerView MoviePlayer { get; set; }

		[Outlet]
		AppKit.NSStepper speedStepper { get; set; }

		[Outlet]
		AppKit.NSTextField speedText { get; set; }

		[Outlet]
		AppKit.NSButton startButton { get; set; }

		[Outlet]
		AppKit.NSImageView StartImage { get; set; }

		[Outlet]
		AppKit.NSTextField startTimeLabel { get; set; }

		[Action ("EndButtonClicked:")]
		partial void EndButtonClicked (Foundation.NSObject sender);

		[Action ("EndToStartClicked:")]
		partial void EndToStartClicked (Foundation.NSObject sender);

		[Action ("exportButtonClicked:")]
		partial void exportButtonClicked (Foundation.NSObject sender);

		[Action ("LocationSliderSlided:")]
		partial void LocationSliderSlided (Foundation.NSObject sender);

		[Action ("SpeedChanged:")]
		partial void SpeedChanged (AppKit.NSTextField sender);

		[Action ("SpinValueChanged:")]
		partial void SpinValueChanged (AppKit.NSStepper sender);

		[Action ("StartButtonClicked:")]
		partial void StartButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ClickedLabel != null) {
				ClickedLabel.Dispose ();
				ClickedLabel = null;
			}

			if (endButton != null) {
				endButton.Dispose ();
				endButton = null;
			}

			if (EndImage != null) {
				EndImage.Dispose ();
				EndImage = null;
			}

			if (endTimeLabel != null) {
				endTimeLabel.Dispose ();
				endTimeLabel = null;
			}

			if (exportButton != null) {
				exportButton.Dispose ();
				exportButton = null;
			}

			if (invertButton != null) {
				invertButton.Dispose ();
				invertButton = null;
			}

			if (LocationSlider != null) {
				LocationSlider.Dispose ();
				LocationSlider = null;
			}

			if (MoviePlayer != null) {
				MoviePlayer.Dispose ();
				MoviePlayer = null;
			}

			if (speedStepper != null) {
				speedStepper.Dispose ();
				speedStepper = null;
			}

			if (speedText != null) {
				speedText.Dispose ();
				speedText = null;
			}

			if (startButton != null) {
				startButton.Dispose ();
				startButton = null;
			}

			if (StartImage != null) {
				StartImage.Dispose ();
				StartImage = null;
			}

			if (startTimeLabel != null) {
				startTimeLabel.Dispose ();
				startTimeLabel = null;
			}
		}
	}
}
