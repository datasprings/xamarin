using System.Collections.Generic;
using CoreGraphics;
using AppKit;

namespace Xamarin.Forms.Platform.Mac
{
	public class NativeViewWrapper : View
	{
		public NativeViewWrapper(NSView nativeView, GetDesiredSizeDelegate getDesiredSizeDelegate = null, SizeThatFitsDelegate sizeThatFitsDelegate = null, LayoutSubviewsDelegate layoutSubViews = null)
		{
			GetDesiredSizeDelegate = getDesiredSizeDelegate;
			SizeThatFitsDelegate = sizeThatFitsDelegate;
			LayoutSubViews = layoutSubViews;
			NativeView = nativeView;
		}

		public GetDesiredSizeDelegate GetDesiredSizeDelegate { get; }

		public LayoutSubviewsDelegate LayoutSubViews { get; set; }

		public NSView NativeView { get; }

		public SizeThatFitsDelegate SizeThatFitsDelegate { get; set; }
	}
}