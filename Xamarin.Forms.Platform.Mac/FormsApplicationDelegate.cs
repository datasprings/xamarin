using System;
using System.ComponentModel;
using Foundation;
using AppKit;
using CoreGraphics;

namespace Xamarin.Forms.Platform.Mac
{
	public class FormsApplicationDelegate : NSApplicationDelegate
	{
		Application application;
		NSWindow window;

		protected FormsApplicationDelegate ()
		{
		}

		// finish initialization before display to user
		public override void DidFinishLaunching (NSNotification notification)
		{
			window = NSApplication.SharedApplication.Windows [0];
			SetMainPage ();
			application.SendStart ();
		}


		protected override void Dispose (bool disposing)
		{
			if (disposing && application != null)
				application.PropertyChanged -= ApplicationOnPropertyChanged;

			base.Dispose (disposing);
		}

		protected void LoadApplication (Application application)
		{
			if (application == null)
				throw new ArgumentNullException ("application");

			this.application = application;
			Application.Current = application;

			application.PropertyChanged += ApplicationOnPropertyChanged;
		}

		void ApplicationOnPropertyChanged (object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "MainPage")
				UpdateMainPage ();
		}

		void SetMainPage ()
		{
			UpdateMainPage ();
			window.MakeKeyAndOrderFront (NSApplication.SharedApplication);
		}

		void UpdateMainPage ()
		{
			if (application.MainPage == null)
				return;

			var platformRenderer = window.ContentViewController as PlatformRenderer;
			window.ContentViewController.AddChildViewController (application.MainPage.CreateViewController ());
			if (platformRenderer != null)
				((IDisposable)platformRenderer.Platform).Dispose ();
		}
	}
}
