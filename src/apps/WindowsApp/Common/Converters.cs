using System;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public static class Converters
    {
        public static DateTime ToLocal(DateTime dateTime) => dateTime.ToLocalTime();
    }
}
