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
	public class AppDelegate : Xamarin.Forms.Platform.Mac.FormsApplicationDelegate
	{
		public AppDelegate ()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			Forms.Init ();
			var app = new App ();
			LoadApplication (app);
			base.DidFinishLaunching (notification);
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}

