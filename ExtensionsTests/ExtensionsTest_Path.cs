using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.AreEqual("D:\\...\\CodeArtEng.Controls.csproj", shortPath);
        }
    }
}
