using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Chroomsoft.Top2000.Apps.Common;
using System.Linq;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace XamarinForms.Droid.Common
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        private Toolbar? modalToolbar;

        public CustomNavigationPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (Element.CurrentPage is IModalPage && Context is FormsAppCompatActivity activity)
            {
                var content = activity.FindViewById(Android.Resource.Id.Content);

                if (content is ViewGroup viewGroup)
                {
                    var toolbars = viewGroup.GetChildrenOfType<Toolbar>();

                    modalToolbar = toolbars.Last();
                    modalToolbar.NavigationClick += OnModalToolbarNavigationClick;
                }
            }
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            if (modalToolbar != null)
            {
                modalToolbar.NavigationClick -= OnModalToolbarNavigationClick;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (Element.CurrentPage is IModalPage && modalToolbar != null)
            {
                modalToolbar.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_material);
            }
        }

        private void OnModalToolbarNavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            if (Element.CurrentPage is IModalPage modalPage)
                modalPage.DismissAsync();
            else
                Element.SendBackButtonPressed();
        }
    }
}
