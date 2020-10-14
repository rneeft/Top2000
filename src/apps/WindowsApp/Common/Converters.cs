using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public static class Converters
    {
        public static DateTime ToLocal(DateTime dateTime) => dateTime.ToLocalTime();
    }
}
