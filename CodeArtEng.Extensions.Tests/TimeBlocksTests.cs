using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using CodeArtEng.Extensions;

namespace ExtensionsTests
{
    [TestFixture]
    public class TimeBlockTests
    {
        [Test]
        public void TestStartTime()
        {
            var timeBlock = new TimeBlock();
            Assert.That(timeBlock.StartTime,Is.EqualTo(DateTime.MinValue));

            var newStartTime = DateTime.Now;
            timeBlock.StartTime = newStartTime;
            Assert.That(timeBlock.StartTime,Is.EqualTo(newStartTime));
        }

        [Test]
        public void TestEndTime()
        {
            var timeBlock = new TimeBlock();
            Assert.That(timeBlock.EndTime,Is.EqualTo(DateTime.MinValue));

            var newEndTime = DateTime.Now;
            timeBlock.EndTime = newEndTime;
            Assert.That(timeBlock.EndTime,Is.EqualTo(newEndTime));
        }

        [Test]
        public void TestDuration()
        {
            var timeBlock = new TimeBlock();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(1);
            timeBlock.StartTime = startTime;
            timeBlock.EndTime = endTime;

            Assert.That(timeBlock.Duration,Is.EqualTo(TimeSpan.FromHours(1)));

            var newDuration = TimeSpan.FromMinutes(30);
            timeBlock.Duration = newDuration;
            Assert.That(timeBlock.EndTime,Is.EqualTo(startTime.AddMinutes(30)));
        }

        [Test]
        public void TestChildrens()
        {
            var timeBlock = new TimeBlock();
            Assert.That(timeBlock.Childrens,Is.Null);

            var children = new List<TimeBlock> { new TimeBlock(), new TimeBlock() };
            timeBlock.Childrens = children;
            Assert.That(timeBlock.Childrens,Is.EqualTo(children));
        }

        [Test]
        public void TestHasChildren()
        {
            var timeBlock = new TimeBlock();
            Assert.That(timeBlock.HasChildren,Is.False);

            timeBlock.Childrens = new List<TimeBlock> { new TimeBlock() };
            Assert.That(timeBlock.HasChildren,Is.True);
        }

        [Test]
        public void TestDurationInSeconds()
        {
            var timeBlock = new TimeBlock();
            var startTime = DateTime.Now;
            timeBlock.StartTime = startTime;

            timeBlock.DurationInSeconds(60);
            Assert.That(timeBlock.EndTime,Is.EqualTo(startTime.AddSeconds(60)));
        }

        [Test]
        public void TestGroups()
        {
            var startTime1 = DateTime.Now;
            var endTime1 = startTime1.AddHours(1);
            var timeBlock1 = new TimeBlock { StartTime = startTime1, EndTime = endTime1 };

            var startTime2 = endTime1.AddMinutes(30);
            var endTime2 = startTime2.AddHours(1);
            var timeBlock2 = new TimeBlock { StartTime = startTime2, EndTime = endTime2 };

            var grouped = TimeBlock.Groups(timeBlock1, timeBlock2);
            Assert.That(grouped.Length,Is.EqualTo(2));
            Assert.That(grouped[0].Duration,Is.EqualTo(timeBlock1.Duration));
            Assert.That(grouped[1].Duration,Is.EqualTo(timeBlock2.Duration));
        }

        [Test]
        public void TestGroupsWithOverlappingTimeBlocks()
        {
            // Create 5 TimeBlock objects with overlapping times
            var timeBlock1 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 0, 0), EndTime = new DateTime(2024, 7, 14, 9, 0, 0) };
            var timeBlock2 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 30, 0), EndTime = new DateTime(2024, 7, 14, 9, 30, 0) };
            var timeBlock3 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 9, 0, 0), EndTime = new DateTime(2024, 7, 14, 10, 0, 0) };

            var timeBlock4 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 10, 30, 0), EndTime = new DateTime(2024, 7, 14, 11, 30, 0) };
            var timeBlock5 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 11, 0, 0), EndTime = new DateTime(2024, 7, 14, 12, 0, 0) };

            // Group the TimeBlock objects
            var grouped = TimeBlock.Groups(timeBlock3, timeBlock1, timeBlock5, timeBlock2, timeBlock4);

            // Verify the groups
            Assert.That(grouped.Length,Is.EqualTo(2));

            // Verify the first group
            var group1 = grouped[0];
            Assert.That(group1.StartTime,Is.EqualTo(new DateTime(2024, 7, 14, 8, 0, 0)));
            Assert.That(group1.EndTime,Is.EqualTo(new DateTime(2024, 7, 14, 10, 0, 0)));
            Assert.That(group1.Childrens.Count,Is.EqualTo(3));
            Assert.That(group1.Duration,Is.EqualTo(TimeSpan.FromHours(2)));

            // Verify the second group
            var group2 = grouped[1];
            Assert.That(group2.StartTime,Is.EqualTo(new DateTime(2024, 7, 14, 10, 30, 0)));
            Assert.That(group2.EndTime,Is.EqualTo(new DateTime(2024, 7, 14, 12, 0, 0)));
            Assert.That(group2.Childrens.Count,Is.EqualTo(2));
            Assert.That(group2.Duration,Is.EqualTo(TimeSpan.FromHours(1.5)));
        }

        [Test]
        public void TestInvalidDurationInSeconds()
        {
                        Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var timeBlock = new TimeBlock();
                timeBlock.DurationInSeconds(-10); // Negative duration should throw an exception
            });
        }

        [Test]
        public void TestNullChildrens()
        {
            var timeBlock = new TimeBlock();
            Assert.That(timeBlock.Childrens,Is.Null); // Childrens should be null by default
            Assert.That(timeBlock.HasChildren,Is.False); // HasChildren should be false when Childrens is null
        }

        [Test]
        public void TestGroupsWithNullItems()
        {
            var grouped = TimeBlock.Groups(null);
            Assert.That(grouped,Is.Null); // Grouping null items should return null
        }

        [Test]
        public void TestGroupsWithEmptyItems()
        {
            var grouped = TimeBlock.Groups();
            Assert.That(grouped.Length,Is.EqualTo(0)); // Grouping empty items should return an empty array
        }

        [Test]
        public void TestGroupsWithOverlappingTimeBlocks_AllOverlap()
        {
            var timeBlock1 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 0, 0), EndTime = new DateTime(2024, 7, 14, 9, 0, 0) };
            var timeBlock2 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 30, 0), EndTime = new DateTime(2024, 7, 14, 9, 30, 0) };
            var timeBlock3 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 9, 0, 0), EndTime = new DateTime(2024, 7, 14, 10, 0, 0) };

            var grouped = TimeBlock.Groups(timeBlock1, timeBlock2, timeBlock3);

            Assert.That(grouped.Length,Is.EqualTo(1)); // All time blocks overlap, so there should be only one group
            Assert.That(grouped[0].StartTime,Is.EqualTo(new DateTime(2024, 7, 14, 8, 0, 0)));
            Assert.That(grouped[0].EndTime,Is.EqualTo(new DateTime(2024, 7, 14, 10, 0, 0)));
        }

        [Test]
        public void TestInvalidStartTimeAndEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 7, 14, 10, 0, 0),
                EndTime = new DateTime(2024, 7, 14, 9, 0, 0) // EndTime is before StartTime
            };

            Assert.That(timeBlock.Duration < TimeSpan.Zero,Is.True); // Duration should be negative
        }

        [Test]
        public void Offset_PositiveSeconds_ShouldShiftBothStartAndEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Offset(3600); // Offset by 1 hour

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 13, 0, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 14, 0, 0)));
        }

        [Test]
        public void Offset_NegativeSeconds_ShouldShiftBothStartAndEndTimeBackwards()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Offset(-1800); // Offset by -30 minutes

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 11, 30, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 30, 0)));
        }

        [Test]
        public void Offset_WithTimeSpan_ShouldShiftBothStartAndEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Offset(TimeSpan.FromMinutes(45));

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 45, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 13, 45, 0)));
        }

        [Test]
        public void Extend_PositiveSeconds_ShouldExtendEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Extend(1800); // Extend by 30 minutes

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 0, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 13, 30, 0)));
        }

        [Test]
        public void Extend_NegativeSeconds_ShouldShortenEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Extend(-900); // Extend by -15 minutes

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 0, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 45, 0)));
        }

        [Test]
        public void Extend_WithTimeSpan_ShouldExtendEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Extend(TimeSpan.FromMinutes(15));

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 0, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 13, 15, 0)));
        }

        [Test]
        public void Extend_NegativeTimeSpan_ShouldShortenEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 13, 0, 0)
            };

            timeBlock.Extend(TimeSpan.FromMinutes(-20));

            Assert.That(timeBlock.StartTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 0, 0)));
            Assert.That(timeBlock.EndTime,Is.EqualTo(new DateTime(2024, 1, 1, 12, 40, 0)));
        }
    }

}
