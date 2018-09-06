using AppKit;
using Foundation;

namespace MovieSlicer
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }
        public ViewController viewController;

        partial void OpenMenuClicked(NSObject sender)
        {
            var appDelegate = NSApplication.SharedApplication.Delegate
                                           as AppDelegate;
            appDelegate.viewController.OpenFile();
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
