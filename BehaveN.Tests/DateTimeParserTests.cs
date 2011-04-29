using System;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class DateTimeParserTests
    {
        private static readonly TimeSpan _allowedDelta = TimeSpan.FromSeconds(2);

        internal static void AssertThatDateTimeIsCloseEnoughToNow(DateTime actual)
        {
            AssertThatDateTimeIsCloseEnough(actual, DateTime.Now);
        }

        [CoverageExclude]
        internal static void AssertThatDateTimeIsCloseEnough(DateTime actual, DateTime expected)
        {
            if (expected - actual > _allowedDelta)
            {
                // We know this will fail, but we like the error message.
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void InvalidDateTime()
        {
            DateTime now = DateTime.Now;
            DateTime dt = DateTimeParser.ParseDateTime("xxx", now);
            Assert.That(dt, Is.EqualTo(now));
        }

        [Test]
        public void Now()
        {
            DateTime dt = DateTimeParser.ParseDateTime(DateTime.Now.ToString());
            AssertThatDateTimeIsCloseEnoughToNow(dt);
        }

        [Test]
        public void LowerCaseNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("now");
            AssertThatDateTimeIsCloseEnoughToNow(dt);
        }

        [Test]
        public void UpperCaseNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("NOW");
            AssertThatDateTimeIsCloseEnoughToNow(dt);
        }

        [Test]
        public void MixedCaseNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("Now");
            AssertThatDateTimeIsCloseEnoughToNow(dt);
        }

        [Test]
        public void Today()
        {
            DateTime dt = DateTimeParser.ParseDateTime("Today");
            Assert.That(dt, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void Tomorrow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("Tomorrow");
            Assert.That(dt, Is.EqualTo(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void Yesterday()
        {
            DateTime dt = DateTimeParser.ParseDateTime("Yesterday");
            Assert.That(dt, Is.EqualTo(DateTime.Today.AddDays(-1)));
        }

        [Test]
        public void InvalidUnit()
        {
            DateTime now = DateTime.Now;
            DateTime dt = DateTimeParser.ParseDateTime("In 1 Foo", now);
            Assert.That(dt, Is.EqualTo(now));
        }

        [Test]
        public void In1Day()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 1 Day");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(1));
        }

        [Test]
        public void In1Days()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 1 Days");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(1));
        }

        [Test]
        public void In30Days()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 30 Days");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(30));
        }

        [Test]
        public void _1DayAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("1 Day Ago");
            Assert.That(dt, Is.EqualTo(DateTime.Today.AddDays(-1)));
        }

        [Test]
        public void _30DaysAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("30 Days Ago");
            Assert.That(dt, Is.EqualTo(DateTime.Today.AddDays(-30)));
        }

        [Test]
        public void _1DayFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("1 Day From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddDays(1));
        }

        [Test]
        public void _30DaysFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("30 Days From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddDays(30));
        }

        [Test]
        public void In5Seconds()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Seconds");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddSeconds(5));
        }

        [Test]
        public void In5Minutes()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Minutes");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddMinutes(5));
        }

        [Test]
        public void In5Hours()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Hours");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddHours(5));
        }

        [Test]
        public void In5Days()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Days");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(5));
        }

        [Test]
        public void In5Months()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Months");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddMonths(5));
        }

        [Test]
        public void In5Years()
        {
            DateTime dt = DateTimeParser.ParseDateTime("In 5 Years");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddYears(5));
        }

        [Test]
        public void _5SecondsAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Seconds Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddSeconds(-5));
        }

        [Test]
        public void _5MinutesAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Minutes Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddMinutes(-5));
        }

        [Test]
        public void _5HoursAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Hours Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddHours(-5));
        }

        [Test]
        public void _5DaysAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Days Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(-5));
        }

        [Test]
        public void _5MonthsAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Months Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddMonths(-5));
        }

        [Test]
        public void _5YearsAgo()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Years Ago");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddYears(-5));
        }

        [Test]
        public void _5SecondsFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Seconds From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddSeconds(5));
        }

        [Test]
        public void _5MinutesFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Minutes From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddMinutes(5));
        }

        [Test]
        public void _5HoursFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Hours From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddHours(5));
        }

        [Test]
        public void _5DaysFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Days From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddDays(5));
        }

        [Test]
        public void _5MonthsFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Months From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddMonths(5));
        }

        [Test]
        public void _5YearsFromNow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Years From Now");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Now.AddYears(5));
        }

        [Test]
        public void _5DaysFromToday()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Days From Today");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(5));
        }

        [Test]
        public void _5MonthsBeforeToday()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Months Before Today");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddMonths(-5));
        }

        [Test]
        public void _5MonthsBeforeTomorrow()
        {
            DateTime dt = DateTimeParser.ParseDateTime("5 Months Before Tomorrow");
            AssertThatDateTimeIsCloseEnough(dt, DateTime.Today.AddDays(1).AddMonths(-5));
        }
    }
}
