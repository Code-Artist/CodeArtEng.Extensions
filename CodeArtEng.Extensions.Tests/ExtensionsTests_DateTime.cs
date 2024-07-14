using System;
using NUnit.Framework;

namespace ExtensionsTests
{
    [TestFixture]
    class ExtensionsTests_DateTime
    {
        [Test]
        public void DateTime_RoundUp_NearestMinute_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundUp(TimeSpan.FromMinutes(1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 12, 39, 00), val);
        }

        [Test]
        public void DateTime_RoundUp_NearestDate_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundUp(TimeSpan.FromDays(1));
            Assert.AreEqual(new DateTime(2015, 1, 2, 00, 00, 00), val);
        }

        [Test]
        public void DateTime_RoundUp_NearestHour_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundUp(TimeSpan.FromHours(1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 13, 00, 00), val);
        }

        [Test]
        public void DateTime_RoundDown_NearestMinute_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundDown(TimeSpan.FromMinutes(1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 12, 38, 00), val);
        }

        [Test]
        public void DateTime_RoundDown_NearestDate_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundDown(TimeSpan.FromDays(1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 00, 00, 00), val);
        }

        [Test]
        public void DateTime_RoundDown_NearestHour_Calculate()
        {
            DateTime val = new DateTime(2015, 1, 1, 12, 38, 25);
            val = val.RoundDown(TimeSpan.FromHours(1));
            Assert.AreEqual(new DateTime(2015, 1, 1, 12, 00, 00), val);
        }

        [Test]
        public void DateTime_CalendarWeekISO8601()
        {
            DateTime val = new DateTime(2024, 1, 1);
            Assert.AreEqual(1, val.CalendarWeekISO8601());
        }

        [Test]
        public void DateTime_CalendarWeekNA()
        {
            DateTime val = new DateTime(2024, 1, 1);
            Assert.AreEqual(53, val.CalendarWeekNA());
        }
    }
}
