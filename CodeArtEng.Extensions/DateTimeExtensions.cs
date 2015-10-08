using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Round Up <see cref="DateTime"/> to nearest timespan.
        /// </summary>
        /// <param name="dt">Input object</param>
        /// <param name="d">Time span unit. Example: TimeSpan.FromMinutes(1) rounded to 1 minute.</param>
        /// <returns></returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        /// <summary>
        /// Round Down <see cref="DateTime"/> to nearest timespan.
        /// </summary>
        /// <param name="dt">Input object</param>
        /// <param name="d">Time span unit. Example: TimeSpan.FromMinutes(1) rounded to 1 minute.</param>
        /// <returns></returns>
        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks / d.Ticks) * d.Ticks);
        }

    }
}
