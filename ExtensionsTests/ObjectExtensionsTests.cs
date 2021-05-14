using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.AreEqual(5, b2.Padding.All);
            Assert.AreEqual(123, b2.Width);
        }
    }
}
