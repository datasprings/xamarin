using System;
using AppKit;

namespace Xamarin.Forms
{
	public class ViewInitializedEventArgs : EventArgs
	{
		public NSView NativeView { get; internal set; }

		public VisualElement View { get; internal set; }
	}
}