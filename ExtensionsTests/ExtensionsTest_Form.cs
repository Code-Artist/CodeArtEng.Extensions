using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using NUnit.Framework;

namespace ExtensionsTests
{
    [TestFixture]
    class ExtensionsTest_Form
    {
        [Test]
        public void Form_CheckBound()
        {
            using (Form tForm = new Form())
            {
                tForm.Show();
                tForm.Location = new Point(10, 10);
                Assert.IsFalse(tForm.IsOutsideViewRegion());
            }
        }

        [Test]
        public void Form_CheckBoud_OutOfScreen()
        {
            using (Form tForm = new Form())
            {
                tForm.Show();
                tForm.Location = new Point(9000, 9000);
                Assert.IsTrue(tForm.IsOutsideViewRegion());
            }
        }
    }
}
