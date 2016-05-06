using System;
using System.Collections.Concurrent;
using System.Threading;
using Xamarin.Forms.Internals;
using AppKit;
using CoreVideo;
using CoreAnimation;
using Foundation;

namespace Xamarin.Forms.Platform.Mac
{
	internal class CVDisplayLinkTicker : Ticker
	{
		readonly BlockingCollection<Action> _queue = new BlockingCollection<Action> ();
		CVDisplayLink _link;

		public CVDisplayLinkTicker ()
		{
			var thread = new Thread (StartThread);
			thread.Start ();
		}

		internal new static CVDisplayLinkTicker Default {
			get { return Ticker.Default as CVDisplayLinkTicker; }
		}

		public void Invoke (Action action)
		{
			_queue.Add (action);
		}

		protected override void DisableTimer ()
		{
			if (_link != null) {
				//_link.RemoveFromRunLoop (NSRunLoop.Current, NSRunLoop.NSRunLoopCommonModes);
				_link.Dispose ();
			}
			_link = null;
		}

		protected override void EnableTimer ()
		{
			_link = new CVDisplayLink ();

			// CVDisplayCADisplayLink.Create (() => SendSignals ());
			// _link.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSRunLoopCommonModes);
		}

		void StartThread ()
		{
			while (true) {
				var action = _queue.Take ();

				CATransaction.Begin ();
				action.Invoke ();

				while (_queue.TryTake (out action))
					action.Invoke ();
				CATransaction.Commit ();
			}
		}
	}
}