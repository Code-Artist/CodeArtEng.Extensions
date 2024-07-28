using System.IO;
using NUnit.Framework;

namespace ExtensionsTests
{
    [TestFixture]
    class ExtensionsTest_Path
    {
        [Test]
        public void ShortPath()
        {
            string filePath = "D:\\CKMAI_Documents\\Programming\\ClassLibraryNET\\CodeArtEng\\CodeArtEng.Controls\\CodeArtEng.Controls\\obj\\Debug\\CodeArtEng.Controls.csproj";
            string shortPath = PathEx.GetShortPath(filePath, 30);
            Assert.That(shortPath,Is.EqualTo("D:\\...\\CodeArtEng.Controls.csproj"));
        }
    }
}
