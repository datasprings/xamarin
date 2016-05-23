using AppKit;
using Foundation;
using CoreGraphics;

namespace Xamarin.Forms.ControlGallery.Mac
{
	public class App : Application
	{
		public App ()
		{
			//MainPage = 
			MainPage = new ContentPage { 
				Title = "Master", 
				BackgroundColor = Color.Red,
				Content = new StackLayout {
					Padding = new Thickness (10, 40, 10, 10),
					Children = {
						new Label { Text = "Login", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) },
						new Label { Text = "Username" },
						new Entry { Text = "" },
						new Label { Text = "Password" },
						new Entry { Text = ""},
						new Button { Text = "Login" },

						new Button { Text = "Create Account" }
					}
				}
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

