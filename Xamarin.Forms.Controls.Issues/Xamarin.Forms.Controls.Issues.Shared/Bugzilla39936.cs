using System;

using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 39936, "Out of memory error when navigating through list views")]
	public class Bugzilla39936 : TestContentPage
	{
		const string Main_Image = "crimson.jpg";
		const string ListView_Id = "photos";
		const string ListView1_Button = "ListView 1";
		const string ListView2_Button = "ListView 2";
		const string ListView3_Button = "ListView 3";
		const int Number_of_Items = 100;

		[Preserve(AllMembers = true)]
		public class Photo
		{
			public string Image { get; set; }
			public int Number { get; set; }
		}

		[Preserve(AllMembers = true)]
		public class PhotoViewModel : INotifyPropertyChanged
		{
			#region INotifyPropertyChanged implementation
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion

			protected void INotifyPropertyChanged([CallerMemberName] string propertyName = "")
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			IList<Photo> _data;

			public IList<Photo> Data
			{
				get { return _data; }
				set
				{
					_data = value;
					INotifyPropertyChanged();
				}
			}

			public PhotoViewModel()
			{
				Data = new List<Photo>();
			}

			public void LoadData()
			{
				for (int i = 1; i <= Number_of_Items; i++)
				{
					Data.Add(new Photo()
					{
						Image = Main_Image,
						Number = i
					});
				}
			}
		}

		[Preserve(AllMembers = true)]
		public class PhotoView : ContentPage
		{
			PhotoViewModel viewModel { get; set; }
			public PhotoView()
			{
				var newListView = new ListView(ListViewCachingStrategy.RecycleElement)
				{
					AutomationId = ListView_Id,
					HasUnevenRows = true,
					ItemTemplate = new DataTemplate(() =>
					{
						var image = new Image();
						image.SetBinding(Image.SourceProperty, nameof(Photo.Image));
						var label = new Label();
						label.SetBinding(Label.TextProperty, nameof(Photo.Number));
						var stackLayout = new StackLayout { Children = { image, label } };
						return new ViewCell { View = stackLayout };
					})
				};
				newListView.SetBinding(ListView.ItemsSourceProperty, nameof(viewModel.Data));
				Content = newListView;
			}

			protected override void OnAppearing()
			{
				base.OnAppearing();

				if (viewModel == null)
				{
					viewModel = new PhotoViewModel();
					viewModel.LoadData();
					BindingContext = viewModel;
				}
			}
		}

		protected override void Init()
		{
			Content = new StackLayout
			{
				Children = {
					GetButton(ListView1_Button, typeof(PhotoView)),
					GetButton(ListView2_Button, typeof(PhotoView)),
					GetButton(ListView3_Button, typeof(PhotoView)),
				}
			};
		}

		Button GetButton(string title, Type pageType)
		{
			Button button = new Button()
			{
				Text = title
			};

			button.Clicked += (object sender, EventArgs e) =>
			{
				Page page = (Page)Activator.CreateInstance(pageType);
				page.Title = title;

				Navigation.PushAsync(page);
			};

			return button;
		}

#if UITEST
		[Test]
		public void Bugzilla39936Test()
		{
			RunningApp.WaitForElement(q => q.Marked(ListView1_Button));
			RunningApp.Tap(q => q.Marked(ListView1_Button));
			RunningApp.WaitForElement(q => q.Marked(ListView_Id));
			for (int i = 10; i <= Number_of_Items; i += 10)
			{
				RunningApp.ScrollDownTo(toQuery: q => q.Marked(i.ToString()), withinQuery: q => q.Marked(ListView_Id), strategy: ScrollStrategy.Programmatically);
			}
			RunningApp.Back();
			RunningApp.WaitForElement(q => q.Marked(ListView2_Button));
		}
#endif
	}
}
