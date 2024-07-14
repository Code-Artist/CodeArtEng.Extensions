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
            Assert.AreEqual(DateTime.MinValue, timeBlock.StartTime);

            var newStartTime = DateTime.Now;
            timeBlock.StartTime = newStartTime;
            Assert.AreEqual(newStartTime, timeBlock.StartTime);
        }

        [Test]
        public void TestEndTime()
        {
            var timeBlock = new TimeBlock();
            Assert.AreEqual(DateTime.MinValue, timeBlock.EndTime);

            var newEndTime = DateTime.Now;
            timeBlock.EndTime = newEndTime;
            Assert.AreEqual(newEndTime, timeBlock.EndTime);
        }

        [Test]
        public void TestDuration()
        {
            var timeBlock = new TimeBlock();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(1);
            timeBlock.StartTime = startTime;
            timeBlock.EndTime = endTime;

            Assert.AreEqual(TimeSpan.FromHours(1), timeBlock.Duration);

            var newDuration = TimeSpan.FromMinutes(30);
            timeBlock.Duration = newDuration;
            Assert.AreEqual(startTime.AddMinutes(30), timeBlock.EndTime);
        }

        [Test]
        public void TestChildrens()
        {
            var timeBlock = new TimeBlock();
            Assert.IsNull(timeBlock.Childrens);

            var children = new List<TimeBlock> { new TimeBlock(), new TimeBlock() };
            timeBlock.Childrens = children;
            Assert.AreEqual(children, timeBlock.Childrens);
        }

        [Test]
        public void TestHasChildren()
        {
            var timeBlock = new TimeBlock();
            Assert.IsFalse(timeBlock.HasChildren);

            timeBlock.Childrens = new List<TimeBlock> { new TimeBlock() };
            Assert.IsTrue(timeBlock.HasChildren);
        }

        [Test]
        public void TestDurationInSeconds()
        {
            var timeBlock = new TimeBlock();
            var startTime = DateTime.Now;
            timeBlock.StartTime = startTime;

            timeBlock.DurationInSeconds(60);
            Assert.AreEqual(startTime.AddSeconds(60), timeBlock.EndTime);
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
            Assert.AreEqual(2, grouped.Length);
            Assert.AreEqual(timeBlock1.Duration, grouped[0].Duration);
            Assert.AreEqual(timeBlock2.Duration, grouped[1].Duration);
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
            Assert.AreEqual(2, grouped.Length);

            // Verify the first group
            var group1 = grouped[0];
            Assert.AreEqual(new DateTime(2024, 7, 14, 8, 0, 0), group1.StartTime);
            Assert.AreEqual(new DateTime(2024, 7, 14, 10, 0, 0), group1.EndTime);
            Assert.AreEqual(3, group1.Childrens.Count);
            Assert.AreEqual(TimeSpan.FromHours(2), group1.Duration);

            // Verify the second group
            var group2 = grouped[1];
            Assert.AreEqual(new DateTime(2024, 7, 14, 10, 30, 0), group2.StartTime);
            Assert.AreEqual(new DateTime(2024, 7, 14, 12, 0, 0), group2.EndTime);
            Assert.AreEqual(2, group2.Childrens.Count);
            Assert.AreEqual(TimeSpan.FromHours(1.5), group2.Duration);
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
            Assert.IsNull(timeBlock.Childrens); // Childrens should be null by default
            Assert.IsFalse(timeBlock.HasChildren); // HasChildren should be false when Childrens is null
        }

        [Test]
        public void TestGroupsWithNullItems()
        {
            var grouped = TimeBlock.Groups(null);
            Assert.IsNull(grouped); // Grouping null items should return null
        }

        [Test]
        public void TestGroupsWithEmptyItems()
        {
            var grouped = TimeBlock.Groups();
            Assert.AreEqual(0, grouped.Length); // Grouping empty items should return an empty array
        }

        [Test]
        public void TestGroupsWithOverlappingTimeBlocks_AllOverlap()
        {
            var timeBlock1 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 0, 0), EndTime = new DateTime(2024, 7, 14, 9, 0, 0) };
            var timeBlock2 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 8, 30, 0), EndTime = new DateTime(2024, 7, 14, 9, 30, 0) };
            var timeBlock3 = new TimeBlock { StartTime = new DateTime(2024, 7, 14, 9, 0, 0), EndTime = new DateTime(2024, 7, 14, 10, 0, 0) };

            var grouped = TimeBlock.Groups(timeBlock1, timeBlock2, timeBlock3);

            Assert.AreEqual(1, grouped.Length); // All time blocks overlap, so there should be only one group
            Assert.AreEqual(new DateTime(2024, 7, 14, 8, 0, 0), grouped[0].StartTime);
            Assert.AreEqual(new DateTime(2024, 7, 14, 10, 0, 0), grouped[0].EndTime);
        }

        [Test]
        public void TestInvalidStartTimeAndEndTime()
        {
            var timeBlock = new TimeBlock
            {
                StartTime = new DateTime(2024, 7, 14, 10, 0, 0),
                EndTime = new DateTime(2024, 7, 14, 9, 0, 0) // EndTime is before StartTime
            };

            Assert.IsTrue(timeBlock.Duration < TimeSpan.Zero); // Duration should be negative
        }
    }

}