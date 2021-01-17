namespace Chroomsoft.Top2000.Tools.Marketing
{
    public class ScreenshotSettings
    {
        public string Language { get; set; } = string.Empty;
        public string LanguageShortName { get; set; } = string.Empty;
        public string SettingsName { get; set; } = string.Empty;
        public string OverviewName { get; set; } = string.Empty;
        public string SearchName { get; set; } = string.Empty;
        public string SearchPlaceholder { get; set; } = string.Empty;
        public string DateName { get; set; } = string.Empty;
        public string Main { get; internal set; }
        public string Date { get; internal set; }
        public string Search { get; internal set; }
        public string Info { get; internal set; }
        public string Editions { get; internal set; }
        public string DarkMode { get; internal set; }
    }
}
