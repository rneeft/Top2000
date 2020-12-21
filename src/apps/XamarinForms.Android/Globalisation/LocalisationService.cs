using Chroomsoft.Top2000.Apps.Globalisation;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;

namespace XamarinForms.Droid.Globalisation
{
    public class LocalisationService : ILocalisationService
    {
        public const string CulturePreferenceName = "Culture";
        private readonly IEnumerable<ICulture> cultures;
        private ICulture activeCulture;

        public LocalisationService(IEnumerable<ICulture> cultures)
        {
            this.cultures = cultures;
            this.activeCulture = cultures.Single(x => x.Name == "nl");
        }

        public ICulture GetCurrentCulture() => activeCulture;

        public void SetCulture(ICulture cultureInfo)
        {
            if (cultureInfo != GetCurrentCulture())
            {
                Preferences.Set(CulturePreferenceName, cultureInfo.Name);

                activeCulture = cultureInfo;

                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureInfo.Name);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureInfo.Name);

                Translator.Instance.Invalidate();
            }
        }

        public void SetCultureFromSetting()
        {
            var name = Preferences.Get(CulturePreferenceName, "nl");
            activeCulture = FindCulture(name);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);

            Translator.Instance.Invalidate();
        }

        private ICulture FindCulture(string name)
            => cultures.SingleOrDefault(x => x.Name == name) ?? cultures.Single(x => x.Name == "nl");
    }
}
