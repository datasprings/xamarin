using System;

namespace Xamarin.Forms
{
	public class ToolbarItemEventArgs : EventArgs
	{
		public ToolbarItemEventArgs(ToolbarItem item)
		{
			ToolbarItem = item;
		}

		public ToolbarItem ToolbarItem { get; private set; }
	}
}