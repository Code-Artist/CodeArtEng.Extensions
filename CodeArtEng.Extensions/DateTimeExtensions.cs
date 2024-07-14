using System.Globalization;

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


        /// <summary>
        /// Return week number based on ISO8601 format.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int CalendarWeekISO8601(this DateTime date)
        {
            return date.GetCalendarWeek(CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// Return week number based on North America date system
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int CalendarWeekNA(this DateTime date)
        {
            return date.GetCalendarWeek(CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
        }

        private static int GetCalendarWeek(this DateTime date, CalendarWeekRule weekRule, DayOfWeek firstDayofWeek)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, weekRule, firstDayofWeek);
        }
    }
}
