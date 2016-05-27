using System.Collections.Generic;
using Xamarin.Forms.Internals;
using CoreGraphics;
using AppKit;


namespace Xamarin.Forms.Platform.Mac
{
	public class NativeViewWrapperRenderer : ViewRenderer<NativeViewWrapper, NSView>
	{
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (Element?.GetDesiredSizeDelegate == null)
				return base.GetDesiredSize(widthConstraint, heightConstraint);

			// The user has specified a different implementation of GetDesiredSize
			var result = Element.GetDesiredSizeDelegate(this, widthConstraint, heightConstraint);

			// If the GetDesiredSize delegate returns a SizeRequest, we use it; if it returns null,
			// fall back to the default implementation
			return result ?? base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public override void Layout ()
		{
			if (Element?.LayoutSubViews == null)
			{
				((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
				base.Layout();
				return;
			}

			// The user has specified a different implementation of LayoutSubviews
			var handled = Element.LayoutSubViews();

			if (!handled)
			{
				// If the delegate wasn't able to handle the request, fall back to the default implementation
				base.Layout();
			}
		}

		#if false
		public override CGSize SizeThatFits(CGSize size)
		{
			if (Element?.SizeThatFitsDelegate == null)
				return base.SizeThatFits(size);

			// The user has specified a different implementation of SizeThatFits
			var result = Element.SizeThatFitsDelegate(size);

			// If the delegate returns a value, we use it; 
			// if it returns null, fall back to the default implementation
			return result ?? base.SizeThatFits(size);
		}
		#endif
		protected override void OnElementChanged(ElementChangedEventArgs<NativeViewWrapper> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
				SetNativeControl(Element.NativeView);
		}

		/// <summary>
		/// The native control we're wrapping isn't ours to dispose of
		/// </summary>
		protected override bool ManageNativeControlLifetime => false;
	}
}