using System;
using Xamarin.Forms;
using CoreGraphics;

namespace Xamarin.Forms.Platform.Mac
{
	public static class FormsExtensions
	{
		public static CGSize ToCGSize (this Size size)
		{
			return new CGSize (size.Width, size.Height);
		}
	}
}

