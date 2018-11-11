using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ImageProcessing.Tests
{
  [TestClass]
  public class ImageOcrExtractorTests
  {
    [TestMethod]
    public void ProcessTest()
    {
      var picName = "phototest.tif";
      var testImagePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\{picName}");
      Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(testImagePath)));

      var processor = new ImageOcrExtractor();
      processor.Process(picName);
    }
  }
}
