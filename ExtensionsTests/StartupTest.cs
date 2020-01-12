using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Win32;

namespace System.Windows.Forms
{
    [TestFixture]
    public class StartupTest
    {
        [Test]
        public void AddToStartup()
        {
            ApplicationExtensions.AddApplicationToStartup();
            Assert.IsTrue(ApplicationExtensions.StartApplicationOnStartupEnabled());
        }

        [Test]
        public void RemoveFromStartup()
        {
            ApplicationExtensions.RemoveApplicationFromStartup();
            Assert.IsFalse(ApplicationExtensions.StartApplicationOnStartupEnabled());
        }


        [Test]
        public void UpdatePath()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(Application.ProductName, "\"" + "DummyPath" + "\"");
            }
            Assert.IsTrue(GetStartupKeyValue().Contains("DummyPath"));

            ApplicationExtensions.CheckAndUpdateApplicationStartupPath();

            Assert.IsFalse(GetStartupKeyValue().Contains("DummyPath"));
        }

        private string GetStartupKeyValue()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
            {
                return key.GetValue(Application.ProductName).ToString();
            }
        }

        [OneTimeTearDown]
        public void TestEnd()
        {
            ApplicationExtensions.RemoveApplicationFromStartup();
        }
    }
}
