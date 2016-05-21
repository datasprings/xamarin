using AppKit;
using Foundation;
using CoreGraphics;

namespace Xamarin.Forms.ControlGallery.Mac
{
	public class App : Application
	{
		public App ()
		{
			MainPage = new MasterDetailPage {
				Master = new ContentPage { Title = "Master", BackgroundColor = Color.Red },
				Detail = new ContentPage { Title = "Hello", BackgroundColor = Color.Yellow }
			};
		}

	}

	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			Forms.Init ();
			var app = new App ();

			//var x = NSApplication.SharedApplication.MainWindow.ContentViewController;

			var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
			var rect = NSWindow.FrameRectFor (new CGRect (100, 100, 640, 800), style);
			var window = new NSWindow (rect, style, NSBackingStore.Buffered, false);
			window.Display ();
			window.MakeKeyAndOrderFront (NSApplication.SharedApplication);
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}

