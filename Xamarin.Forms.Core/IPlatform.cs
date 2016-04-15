namespace Xamarin.Forms
{
	public interface IPlatform
	{
		SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint);
	}
}