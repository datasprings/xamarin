using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 40955, "Issue Description")]
	public class Bugzilla40955 : TestMasterDetailPage 
	{
		protected override void Init()
		{
			var masterPage = new MasterPage();
			Master = masterPage;
			masterPage.ListView.ItemSelected += (sender, e) => {
				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{
					Detail = new NavigationPageEX((Page)Activator.CreateInstance(item.TargetType));
					masterPage.ListView.SelectedItem = null;
					IsPresented = false;
				}
			};

			Detail = new NavigationPage(new ContactsPage());
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
				ListView = new ListView() {VerticalOptions = LayoutOptions.FillAndExpand, SeparatorVisibility = SeparatorVisibility.None };

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
		public class _409555_ContactsPage : ContentPage { }

		[Preserve(AllMembers = true)]
		public class _409555_TodoListPage : ContentPage { }

		[Preserve(AllMembers = true)]
		public class _409555_ReminderPage : ContentPage
		{
			protected override void OnAppearing()
			{
				base.OnAppearing();
				GC.Collect();
			}
		}
	}
}