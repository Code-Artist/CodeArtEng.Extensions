using System;
using NUnit.Framework;
using System.Windows.Forms;

namespace ExtensionsTests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void CloneObject()
        {
            Button b1 = new Button() { Name = "BtTest", Width = 123, Height = 50, Padding = new Padding(5) };
            Button b2 = b1.Copy();

            b1.Padding = new Padding(0);
            b1.Width = 10;
            Assert.That(b2.Padding.All,Is.EqualTo(5));
            Assert.That(b2.Width,Is.EqualTo(123));
        }
    }
}
