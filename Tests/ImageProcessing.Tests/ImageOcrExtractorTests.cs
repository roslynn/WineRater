using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ImageProcessing.Tests
{
  [TestClass]
  public class ImageOcrExtractorTests
  {
    [TestMethod]
    public void ProcessExampleFromSourceTest()
    {
      var picName = "phototest.tif";
      var testImagePath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\{picName}");
      Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(testImagePath)));

      var processor = new ImageOcrExtractor(true);
      processor.Process(picName);
    }

    [TestMethod]
    public void ProcessRedtreeWineTest()
    {
      var picName = "RedtreeWineLabelCabernetSauvignon.jpg";
      var testImagePath = Path.Combine(Directory.GetCurrentDirectory(), picName);
      Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(testImagePath)));
      Assert.IsTrue(File.Exists(testImagePath));

      var processor = new ImageOcrExtractor(true);
      processor.Process(picName);
    }
  }
}
