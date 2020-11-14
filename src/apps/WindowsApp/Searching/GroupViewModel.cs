using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public class GroupViewModel
    {
        public GroupViewModel(IGroup value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public IGroup Value { get; }

        public string Name { get; }
    }
}
