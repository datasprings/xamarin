using System;
using AppKit;
using CoreGraphics;

namespace Xamarin.Forms.Platform.Mac
{
	internal class ModalWrapper : NSViewController
	{
		IVisualElementRenderer _modal;

		internal ModalWrapper(IVisualElementRenderer modal)
		{
			_modal = modal;

			#if TODO
			View.BackgroundColor = NSColor.White;
			#endif
			View.AddSubview(modal.ViewController.View);
			AddChildViewController(modal.ViewController);

			#if TODO
			modal.ViewController.DidMoveToParentViewController(this);
			#endif
		}


		public override void ViewDidLayout ()
		{
			base.ViewDidLayout ();
			if (_modal != null)
				_modal.SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
		}

		public override void ViewWillAppear()
		{
			#if TODO
			View.BackgroundColor = UIColor.White;
			#endif
			base.ViewWillAppear();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_modal = null;
			base.Dispose(disposing);
		}
	}

	internal class PlatformRenderer : NSViewController
	{
		internal PlatformRenderer(Platform platform) : base (nibNameOrNull: null, nibBundleOrNull: null)
		{
			Platform = platform;

			// When chaining to a base view controller with null nibname and bundles, we are 
			// resposible for either setting the View here, or implementing LoadView.
			View = new NSView (new CGRect (0, 0, 800, 800));
		}

		public Platform Platform { get; set; }

		public override void ViewDidAppear()
		{
			Platform.DidAppear();
			base.ViewDidAppear();
		}

		public override void ViewDidLayout ()
		{
			base.ViewDidLayout ();
			Platform.LayoutSubviews();
		}

		public override void ViewWillAppear()
		{
			#if TODO
			View.BackgroundColor = UIColor.White;
			#endif
			Platform.WillAppear();
			base.ViewWillAppear();
		}
	}
}