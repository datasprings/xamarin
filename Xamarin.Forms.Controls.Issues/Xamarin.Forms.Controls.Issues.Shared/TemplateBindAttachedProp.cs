using Xamarin.Forms.CustomAttributes;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.None, 0, "Can bind to AttachedProperties with TemplateBindings", PlatformAffected.All)]
	public class TemplateBindAttachedProp : TestContentPage
	{
		public class CustomLayout : Layout
		{
			Label _label = new Label();

			public static readonly BindableProperty AttachedStringProperty = BindableProperty.CreateAttached("AttachedString", typeof(string), typeof(CustomLayout), default(string), propertyChanged: OnTextChanged);
			public static string GetAttachedString(BindableObject bindable)
			{
				return (string)bindable.GetValue(AttachedStringProperty);
			}
			public static void SetAttachedString(BindableObject bindable, string value)
			{
				bindable.SetValue(AttachedStringProperty, value);
			}

			public CustomLayout()
			{
				_label.SetBinding(Label.TextProperty, new TemplateBinding("AttachedString"));
				InternalChildren.Add(_label);
			}

			protected override void LayoutChildren(double x, double y, double width, double height)
			{
				for (var i = 0; i < LogicalChildren.Count; i++)
				{
					Element element = LogicalChildren[i];
					var child = element as View;
					if (child != null)
						LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, width, height));
				}
			}

			static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
			{
				var self = (CustomLayout)bindable;
				self._label.Text = (string)newValue;
			}
		}

		protected override void Init()
		{
			CustomLayout newCustomLayout = new CustomLayout();
			Content = newCustomLayout;

			CustomLayout.SetAttachedString(newCustomLayout, "success");
		}

#if UITEST
		[Test]
		public void TemplateBindAttachedPropTest ()
		{
			RunningApp.WaitForElement (q => q.Marked ("success"));
		}
#endif
	}
}
