using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using AppKit;

namespace Xamarin.Forms.Platform.Mac
{
	public static class ColorExtensions
	{
		internal static readonly NSColor Black = NSColor.Black;
		internal static readonly NSColor SeventyPercentGrey = NSColor.FromRgba (0.7f, 0.7f, 0.7f, 1);

		public static CGColor ToCGColor (this Color color)
		{
			return new CGColor ((float)color.R, (float)color.G, (float)color.B, (float)color.A);
		}

		public static Color ToColor (this NSColor color)
		{
			nfloat red;
			nfloat green;
			nfloat blue;
			nfloat alpha;

			color.GetRgba (out red, out green, out blue, out alpha);
			return new Color (red, green, blue, alpha);
		}

		public static NSColor ToNSColor (this Color color)
		{
			return NSColor.FromRgba ((float)color.R, (float)color.G, (float)color.B, (float)color.A);
		}

		public static NSColor ToNSColor (this Color color, Color defaultColor)
		{
			if (color.IsDefault)
				return defaultColor.ToNSColor ();

			return color.ToNSColor ();
		}

		public static NSColor ToNSColor (this Color color, NSColor defaultColor)
		{
			if (color.IsDefault)
				return defaultColor;

			return color.ToNSColor ();
		}
	}

	public static class PointExtensions
	{
		public static Point ToPoint (this CGPoint point)
		{
			return new Point (point.X, point.Y);
		}
	}

	public static class SizeExtensions
	{
		public static SizeF ToSizeF (this CGSize size)
		{
			return new SizeF ((float)size.Width, (float)size.Height);
		}
	}

	public static class RectangleExtensions
	{
		public static Rectangle ToRectangle (this CGRect rect)
		{
			return new Rectangle (rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static CGRect ToCGRect (this Rectangle rect)
		{
			return new CGRect ((nfloat)rect.X, (nfloat)rect.Y, (nfloat)rect.Width, (nfloat)rect.Height);
		}
	}
}