namespace Chroomsoft.Top2000.Apps.Globalisation;

public interface ILocalisationService
{
    void SetCulture(ICulture cultureInfo);

    void SetCultureFromSetting();

    ICulture GetCurrentCulture();
}