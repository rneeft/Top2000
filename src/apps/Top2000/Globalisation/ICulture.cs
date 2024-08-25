namespace Chroomsoft.Top2000.Apps.Globalisation
{
    public interface ICulture
    {
        string Name { get; }
    }

    public class SupportedCulture : ICulture
    {
        public SupportedCulture(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
