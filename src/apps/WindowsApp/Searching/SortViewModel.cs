#nullable enable

using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public class SortViewModel
    {
        public SortViewModel(ISort value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public ISort Value { get; }

        public string Name { get; }
    }
}
