using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Apps.Searching
{
    public class GroupViewModel
    {
        public GroupViewModel(IGroup value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public IGroup Value { get; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GroupViewModel svm && svm.Name == this.Name;
        }

        public override int GetHashCode() => this.Name.GetHashCode();
    }
}
