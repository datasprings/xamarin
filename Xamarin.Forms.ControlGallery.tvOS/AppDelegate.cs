using System;
using System.IO;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.ControlGallery.tvOS;
using Xamarin.Forms.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(TestCloudService))]
[assembly: Dependency(typeof(StringProvider))]
[assembly: Dependency(typeof(CacheService))]
[assembly: ExportRenderer(typeof(DisposePage), typeof(DisposePageRenderer))]
[assembly: ExportRenderer(typeof(DisposeLabel), typeof(DisposeLabelRenderer))]
namespace Xamarin.Forms.ControlGallery.tvOS
{

	public class CacheService : ICacheService
	{
		public void ClearImageCache()
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var cache = Path.Combine(documents, ".config", ".isolated-storage", "ImageLoaderCache");
			foreach (var file in Directory.GetFiles(cache))
			{
				File.Delete(file);
			}
		}
	}

	public class DisposePageRenderer : PageRenderer
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((DisposePage)Element).SendRendererDisposed();
			}
			base.Dispose(disposing);

		}
	}

	public class DisposeLabelRenderer : LabelRenderer
	{
		protected override void Dispose(bool disposing)
		{

			if (disposing)
			{
				((DisposeLabel)Element).SendRendererDisposed();
			}
			base.Dispose(disposing);
		}
	}

	public class StringProvider : IStringProvider
	{
		public string CoreGalleryTitle
		{
			get { return "tvOS Core Gallery"; }
		}
	}

	public class TestCloudService : ITestCloudService
	{
		public bool IsOnTestCloud()
		{
			var isInTestCloud = Environment.GetEnvironmentVariable("XAMARIN_TEST_CLOUD");

			return isInTestCloud != null && isInTestCloud.Equals("1");
		}

		public string GetTestCloudDeviceName()
		{
			return Environment.GetEnvironmentVariable("XTC_DEVICE_NAME");
		}

		public string GetTestCloudDevice()
		{
			return Environment.GetEnvironmentVariable("XTC_DEVICE");
		}
	}
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			Forms.Init();
			var app = new App();


			LoadApplication(app);
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			return base.FinishedLaunching(application, launchOptions);
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


