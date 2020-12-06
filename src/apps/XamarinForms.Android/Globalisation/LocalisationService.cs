using Chroomsoft.Top2000.Apps.Globalisation;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;

namespace XamarinForms.Droid.Globalisation
{
    public class LocalisationService : ILocalisationService
    {
        public const string CulturePreferenceName = "Culture";
        private readonly ICulture[] cultures;

        public LocalisationService(ICulture[] cultures)
        {
            this.cultures = cultures;
        }

        public ICulture GetCurrentCulture() => FindCulture(Thread.CurrentThread.CurrentCulture.Name);

        public void SetCulture(ICulture cultureInfo)
        {
            if (cultureInfo != GetCurrentCulture())
            {
                Preferences.Set(CulturePreferenceName, cultureInfo.Name);

                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureInfo.Name);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureInfo.Name);

                Translator.Instance.Invalidate();
            }
        }

        public void SetCultureFromSetting()
        {
            if (Preferences.ContainsKey(CulturePreferenceName))
            {
                var name = Preferences.Get(CulturePreferenceName, "nl");
                var culture = FindCulture(name);

                SetCulture(culture);
            }
        }

        private ICulture FindCulture(string name)
            => cultures.SingleOrDefault(x => x.Name == name) ?? cultures.Single(x => x.Name == "nl");
    }
}
