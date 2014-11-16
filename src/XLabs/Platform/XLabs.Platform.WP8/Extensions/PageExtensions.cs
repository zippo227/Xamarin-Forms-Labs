namespace XLabs.Platform.WP8.Extensions
{
	using Microsoft.Phone.Controls;

	public static class PageExtensions
    {
        public static void SetOrientation(this PhoneApplicationPage page, PageOrientation? orientation = null)
        {
            var app = Resolver.Resolve<IXFormsApp>() as XFormsAppWP;

            if (app != null)
            {
                app.SetOrientation(orientation ?? page.Orientation);
            }
        }
    }
}
