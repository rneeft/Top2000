using System.Collections.Immutable;

namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests
{
    public static class Create
    {
        public static ImmutableSortedSet<T> ImmutableSortedSetFrom<T>(params T[] items)
        {
            return items.ToImmutableSortedSet();
        }
    }
}
