using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;
using AppKit;
using CoreGraphics;

namespace Xamarin.Forms.Platform.Mac
{
	public static class NSViewExtensions
	{
		public static IEnumerable<NSView> Descendants(this NSView self)
		{
			if (self.Subviews == null)
				return Enumerable.Empty<NSView>();
			return self.Subviews.Concat(self.Subviews.SelectMany(s => s.Descendants()));
		}

		public static SizeRequest GetSizeRequest(this NSView self, double widthConstraint, double heightConstraint, double minimumWidth = -1, double minimumHeight = -1)
		{
			var fit = self.FittingSize;
			if (fit.Width >= widthConstraint)
				fit.Width = (nfloat) widthConstraint;
			if (fit.Height > heightConstraint)
				fit.Height = (nfloat) heightConstraint;
			var s = new CGSize (100, 100);
			var request = new Size (s.Width == float.PositiveInfinity ? double.PositiveInfinity : s.Width, s.Height == float.PositiveInfinity ? double.PositiveInfinity : s.Height);
			var minimum = new Size (minimumWidth < 0 ? request.Width : minimumWidth, minimumHeight < 0 ? request.Height : minimumHeight);
			Console.WriteLine ("GetSizeRequest -> {0} {1}", request, minimum);
			return new SizeRequest (request, minimum);
		}

		internal static T FindDescendantView<T>(this NSView view) where T : NSView
		{
			var queue = new Queue<NSView>();
			queue.Enqueue(view);

			while (queue.Count > 0)
			{
				var descendantView = queue.Dequeue();

				var result = descendantView as T;
				if (result != null)
					return result;

				for (var i = 0; i < descendantView.Subviews.Length; i++)
					queue.Enqueue(descendantView.Subviews[i]);
			}

			return null;
		}

		internal static NSView FindFirstResponder(this NSView view)
		{
			
			return view;
			#if TODO
			if (view.IsFirstResponder)
				return view;

			foreach (var subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}

			return null;
			#endif
		}
	}
}