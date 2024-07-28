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
            Assert.That(ApplicationExtensions.StartApplicationOnStartupEnabled(),Is.True);
        }

        [Test]
        public void RemoveFromStartup()
        {
            ApplicationExtensions.RemoveApplicationFromStartup();
            Assert.That(ApplicationExtensions.StartApplicationOnStartupEnabled(),Is.False);
        }


        [Test]
        public void UpdatePath()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(Application.ProductName, "\"" + "DummyPath" + "\"");
            }
            Assert.That(GetStartupKeyValue().Contains("DummyPath"),Is.True);

            ApplicationExtensions.CheckAndUpdateApplicationStartupPath();

            Assert.That(GetStartupKeyValue().Contains("DummyPath"),Is.False);
        }


        [Test]
        public void UpdatePathWithArg()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(Application.ProductName, "\"" + "DummyPath" + "\" -a -d Data=\"Test Data\"");
            }
            Assert.That(GetStartupKeyValue().Contains("DummyPath"),Is.True);

            ApplicationExtensions.CheckAndUpdateApplicationStartupPath();

            Assert.That(GetStartupKeyValue().Contains(" -a -d Data=\"Test Data\""),Is.True);
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
