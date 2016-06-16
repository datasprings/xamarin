using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	#region Nav test
	//[Preserve(AllMembers = true)]
	//[Issue(IssueTracker.Bugzilla, 40955, "Issue Description")]
	//public class Bugzilla40955 : TestNavigationPage
	//{
	//	protected override void Init()
	//	{
	//		PushAsync(new _409555_ContactsPage());
	//	}

	//	[Preserve(AllMembers = true)]
	//	public class ContentPageEx : ContentPage
	//	{
	//		static int _counter = 0;

	//		public ContentPageEx()
	//		{
	//			_counter++;
	//			Debug.WriteLine($"Constructor called for {GetType()}; {_counter} ContentPageEX allocated");
	//		}

	//		~ContentPageEx()
	//		{
	//			_counter--;
	//			Debug.WriteLine($"Destructor called for {GetType()}; {_counter} ContentPageEx allocated");
	//		}
	//	}

	//	[Preserve(AllMembers = true)]
	//	public class _409555_ContactsPage : ContentPageEx
	//	{
	//		public _409555_ContactsPage()
	//		{
	//			Title = "Contacts";
	//			var button = new Button { Text = "Collect" };
	//			var next = new Button() { Text = "Next" };
	//			button.Clicked += (sender, args) => { GC.Collect(); };
	//			next.Clicked += (sender, args) => { Navigation.PushAsync(new _409555_TodoListPage()); };
	//			Content = new StackLayout { Children = { button, next } };
	//		}
	//	}

	//	[Preserve(AllMembers = true)]
	//	public class _409555_TodoListPage : ContentPageEx
	//	{
	//		public _409555_TodoListPage()
	//		{
	//			Title = "To Do";
	//			var button = new Button { Text = "Collect" };
	//			var next = new Button() { Text = "Next" };
	//			button.Clicked += (sender, args) => { GC.Collect(); };
	//			next.Clicked += (sender, args) => { Navigation.PushAsync(new _409555_ReminderPage()); };
	//			Content = new StackLayout { Children = { button, next } };
	//		}
	//	}

	//	[Preserve(AllMembers = true)]
	//	public class _409555_ReminderPage : ContentPageEx
	//	{
	//		public _409555_ReminderPage()
	//		{
	//			Title = "Reminders";
	//			var button = new Button { Text = "Collect" };
	//			var next = new Button() { Text = "Next" };
	//			button.Clicked += (sender, args) => { GC.Collect(); };
	//			next.Clicked += (sender, args) => { Navigation.PushAsync(new _409555_ContactsPage()); };
	//			Content = new StackLayout { Children = { button, next } };
	//		}
	//	}
	//}

	#endregion

	#region full test

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 40955, "Issue Description")]
	public class Bugzilla40955 : TestMasterDetailPage
	{
		protected override void Init()
		{
			var masterPage = new MasterPage();
			Master = masterPage;
			masterPage.ListView.ItemSelected += (sender, e) =>
			{
				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{
					Detail = new NavigationPageEX((Page)Activator.CreateInstance(item.TargetType));
					masterPage.ListView.SelectedItem = null;
					IsPresented = false;
				}
			};

			Detail = new NavigationPage(new _409555_ContactsPage());
		}

		[Preserve(AllMembers = true)]
		public class MasterPageItem
		{
			public string IconSource { get; set; }
			public Type TargetType { get; set; }
			public string Title { get; set; }
		}

		[Preserve(AllMembers = true)]
		public class MasterPage : ContentPage
		{
			public ListView ListView { get; }

			public MasterPage()
			{
				Title = "Menu";
				ListView = new ListView() { VerticalOptions = LayoutOptions.FillAndExpand, SeparatorVisibility = SeparatorVisibility.None };

				ListView.ItemTemplate = new DataTemplate(() =>
				{
					var ic = new ImageCell();
					ic.SetBinding(ImageCell.TextProperty, "Title");
					return ic;
				});

				Content = new StackLayout()
				{
					Children = { ListView }
				};

				var masterPageItems = new List<MasterPageItem>();
				masterPageItems.Add(new MasterPageItem
				{
					Title = "Contacts",
					TargetType = typeof(_409555_ContactsPage)
				});
				masterPageItems.Add(new MasterPageItem
				{
					Title = "TodoList",
					TargetType = typeof(_409555_TodoListPage)
				});
				masterPageItems.Add(new MasterPageItem
				{
					Title = "Reminders",
					TargetType = typeof(_409555_ReminderPage)
				});

				ListView.ItemsSource = masterPageItems;
			}
		}

		[Preserve(AllMembers = true)]
		public class NavigationPageEX : NavigationPage
		{
			static int _counter = 0;

			public NavigationPageEX(Page root) : base(root)
			{
				_counter++;
				Debug.WriteLine($"{_counter} NavigationPageEX allocated");
			}

			~NavigationPageEX()
			{
				_counter--;
				Debug.WriteLine($"Destructor called; {_counter} NavigationPageEx allocated");
			}
		}

		[Preserve(AllMembers = true)]
		public class ContentPageEX : ContentPage
		{
			static int _counter = 0;

			public ContentPageEX() 
			{
				_counter++;
				Debug.WriteLine($"{_counter} ContentPageEX allocated");
			}

			~ContentPageEX()
			{
				_counter--;
				Debug.WriteLine($"Destructor called; {_counter} ContentPageEx allocated");
			}
		}

		[Preserve(AllMembers = true)]
		public class _409555_ContactsPage : ContentPageEX
		{
			public _409555_ContactsPage()
			{
				Title = "Contacts";
			}
		}

		[Preserve(AllMembers = true)]
		public class _409555_TodoListPage : ContentPageEX
		{
			public _409555_TodoListPage()
			{
				Title = "To Do";
			}
		}

		[Preserve(AllMembers = true)]
		public class _409555_ReminderPage : ContentPageEX
		{
			public _409555_ReminderPage()
			{
				Title = "Reminders";
			}

			protected override void OnAppearing()
			{
				base.OnAppearing();
				GC.Collect();
				GC.Collect();
				GC.Collect();
			}
		}
	}

	#endregion
}