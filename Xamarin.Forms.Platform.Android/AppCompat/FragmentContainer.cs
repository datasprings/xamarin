using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android.AppCompat
{
	internal class FragmentContainer : Fragment
	{
		readonly WeakReference _pageReference;

		Action<PageContainer> _onCreateCallback;
		bool? _isVisible;
		PageContainer _pageContainer;
		IVisualElementRenderer _visualElementRenderer;

		//static int _counter = 0;

		public FragmentContainer()
		{
			//_counter++;
			//System.Diagnostics.Debug.WriteLine($"FragmentContainer constructor, {_counter} allocated now");
		}

		public FragmentContainer(Page page) : this()
		{
			_pageReference = new WeakReference(page);
		}

		protected FragmentContainer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public Page Page => (Page)_pageReference?.Target;

		IPageController PageController => Page as IPageController;

		public override bool UserVisibleHint
		{
			get { return base.UserVisibleHint; }
			set
			{
				base.UserVisibleHint = value;
				if (_isVisible == value)
					return;
				_isVisible = value;
				if (_isVisible.Value)
					PageController?.SendAppearing();
				else
					PageController?.SendDisappearing();
			}
		}

		public static Fragment CreateInstance(Page page)
		{
			return new FragmentContainer(page) { Arguments = new Bundle() };
		}

		public void SetOnCreateCallback(Action<PageContainer> callback)
		{
			_onCreateCallback = callback;
		}

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (Page != null)
			{
				_visualElementRenderer = Android.Platform.CreateRenderer(Page, ChildFragmentManager);
				Android.Platform.SetRenderer(Page, _visualElementRenderer);

				_pageContainer = new PageContainer(Forms.Context, _visualElementRenderer, true);

				_onCreateCallback?.Invoke(_pageContainer);

				return _pageContainer;
			}

			return null;
		}
		
		public override void OnDestroyView()
		{
			if (Page != null)
			{
				//System.Diagnostics.Debug.WriteLine($"FragmentContainer OnDestroyView for {Page.GetType()} ({Page.Title})");

				IVisualElementRenderer renderer = _visualElementRenderer;
				PageContainer container = _pageContainer;	

				//if (container.Handle != IntPtr.Zero && renderer.ViewGroup.Handle != IntPtr.Zero)
				//{
				//	System.Diagnostics.Debug.WriteLine($"FragmentContainer OnDestroyView for {Page.GetType()} ({Page.Title}) inside intptr zero checks");

				//	container.RemoveFromParent();
				//	renderer.ViewGroup.RemoveFromParent();
				//	Page?.ClearValue(Android.Platform.RendererProperty);

				//	container.Dispose();
				//	renderer.Dispose();
				//}

				if (renderer.ViewGroup.Handle != IntPtr.Zero)
				{
					//System.Diagnostics.Debug.WriteLine($"FragmentContainer OnDestroyView for {Page.GetType()} ({Page.Title}) renderer.ViewGroup.Handle is non-zero");
					//System.Diagnostics.Debug.WriteLine($"preparing to remove renderer.ViewGroup from parent");
					renderer.ViewGroup.RemoveFromParent();
					//System.Diagnostics.Debug.WriteLine($"Done; preparing to dispose renderer");
					//renderer.Dispose();
					//System.Diagnostics.Debug.WriteLine($"renderer disposed");
				}

				//System.Diagnostics.Debug.WriteLine($"Preparing to dispose renderer");
				renderer.Dispose();
				//System.Diagnostics.Debug.WriteLine($"renderer disposed");

				if (container.Handle != IntPtr.Zero)
				{
					//System.Diagnostics.Debug.WriteLine($"FragmentContainer OnDestroyView for {Page.GetType()} ({Page.Title}) container.Handle is non-zero");

					//System.Diagnostics.Debug.WriteLine($"Preparing to remove container from parent");
					container.RemoveFromParent();
					//System.Diagnostics.Debug.WriteLine($"Done; preparing to dispose of container");
					container.Dispose();
					//System.Diagnostics.Debug.WriteLine($"container disposed");
				}

				Page?.ClearValue(Android.Platform.RendererProperty);
			}

			_onCreateCallback = null;
			_visualElementRenderer = null;
			_pageContainer = null;

			base.OnDestroyView();
		}

		public override void OnHiddenChanged(bool hidden)
		{
			base.OnHiddenChanged(hidden);

			if (Page == null)
				return;

			if (hidden)
				PageController?.SendDisappearing();
			else
				PageController?.SendAppearing();
		}

		public override void OnPause()
		{
			PageController?.SendDisappearing();
			base.OnPause();
		}
		
		public override void OnResume()
		{
			PageController?.SendAppearing();
			base.OnResume();
		}

		//~FragmentContainer()
		//{
		//	_counter--;
		//	System.Diagnostics.Debug.WriteLine($"FragmentContainer destructor, {_counter} allocated now");
		//}
	}
}