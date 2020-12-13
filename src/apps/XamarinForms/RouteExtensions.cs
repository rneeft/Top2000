using Xamarin.Forms;

namespace Chroomsoft.Top2000.Apps.XamarinForms
{
    public static class RouteExtensions
    {
        public static RouteFactory For<TView>(this RouteFactory _) where TView : Element => new PageRouteFactor<TView>();

        public static int Count(this RouteFactory f)
        {
            return 123;
        }
    }

    public class PageRouteFactor<TView> : RouteFactory where TView : Element
    {
        public override Element GetOrCreate() => App.GetService<TView>();
    }
}
