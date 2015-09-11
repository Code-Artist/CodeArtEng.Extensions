using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.AreEqual(new int[] { 0, 2, 1, 3 }, TestItems.ToArray());
        }

        [Test]
        public void IList_MoveUp_FirstItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(0);
            Assert.AreEqual(new int[] { 0, 1, 2, 3 }, TestItems.ToArray());
        }

        [Test]
        public void IList_MoveUp_LastItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(3);
            Assert.AreEqual(new int[] { 0, 1, 3, 2 }, TestItems.ToArray());
        }

        [Test][ExpectedException(typeof(IndexOutOfRangeException))]
        public void IList_MoveUp_IndexOutOfRange()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveUp(5);
        }

        [Test]
        public void IList_MoveDown_MoveItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(2);
            Assert.AreEqual(new int[] { 0, 1, 3, 2 }, TestItems.ToArray());
        }

        [Test]
        public void IList_MoveDown_FirstItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(0);
            Assert.AreEqual(new int[] { 1, 0, 2, 3 }, TestItems.ToArray());
        }

        [Test]
        public void IList_MoveDown_LastItem()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(3);
            Assert.AreEqual(new int[] { 0, 1, 2, 3 }, TestItems.ToArray());
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IList_MoveDown_IndexOutOfRange()
        {
            List<int> TestItems = new List<int>() { 0, 1, 2, 3 };
            TestItems.MoveDown(5);
        }
    }
}
