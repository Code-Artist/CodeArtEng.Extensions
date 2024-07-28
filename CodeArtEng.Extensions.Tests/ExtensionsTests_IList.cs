using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ExtensionsTests
{
    [TestFixture]
    public class ExtensionsTests_IList
    {
        [Test]
        public void IList_MoveUp_MoveItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(2);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 0, 2, 1, 3 }));
        }

        [Test]
        public void IList_MoveUp_FirstItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(0);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void IList_MoveUp_LastItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(3);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 0, 1, 3, 2 }));
        }

        [Test]
        public void IList_MoveUp_IndexOutOfRange()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
                        Assert.Throws<IndexOutOfRangeException>(()=> { TestItems.MoveUp(5); });
        }

        [Test]
        public void IList_MoveDown_MoveItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(2);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 0, 1, 3, 2 }));
        }

        [Test]
        public void IList_MoveDown_FirstItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(0);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 1, 0, 2, 3 }));
        }

        [Test]
        public void IList_MoveDown_LastItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(3);
            Assert.That(TestItems.ToArray(),Is.EqualTo(new int[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void IList_MoveDown_IndexOutOfRange()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
                        Assert.Throws<IndexOutOfRangeException>(() => { TestItems.MoveDown(5); });
        }
    }
}
