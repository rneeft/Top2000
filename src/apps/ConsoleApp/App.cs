using Chroomsoft.Top2000.Apps.ConsoleApp.AllEditions;
using CommandDotNet;

namespace ConsoleApp
{
    public class App
    {
        [SubCommand]
        public EditionsCommand Editions { get; set; } = null!;
    }
}
