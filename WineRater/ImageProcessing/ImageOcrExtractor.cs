using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tesseract;

/// <summary>
/// Example taken from https://github.com/charlesw/tesseract-samples/tree/master/src/Tesseract.ConsoleDemo
/// charlesw/tesseract-samples is licensed under the
/// Apache License 2.0
/// </summary>

namespace ImageProcessing
{
  public class ImageOcrExtractor
  {
    private static readonly string TESDATA_NAME = "tessdata";

    public ImageOcrExtractor()
    {
    }

    public void Process(string testImagePath)
    {
      try
      {
        using (var engine = new TesseractEngine(ImageOcrExtractor.GetTestDataPath, "eng", EngineMode.Default))
        {
          using (Pix img = Pix.LoadFromFile(testImagePath))
          {
            using (var page = engine.Process(img))
            {
              var text = page.GetText();
              Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

              Console.WriteLine("Text (GetText): \r\n{0}", text);
              Console.WriteLine("Text (iterator):");
              using (var iter = page.GetIterator())
              {
                iter.Begin();

                do
                {
                  do
                  {
                    do
                    {
                      do
                      {
                        if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                        {
                          Console.WriteLine("<BLOCK>");
                        }

                        Console.Write(iter.GetText(PageIteratorLevel.Word));
                        Console.Write(" ");

                        if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                        {
                          Console.WriteLine();
                        }
                      } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                      if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                      {
                        Console.WriteLine();
                      }
                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                  } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                } while (iter.Next(PageIteratorLevel.Block));
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        Trace.TraceError(e.ToString());
        Console.WriteLine("Unexpected Error: " + e.Message);
        Console.WriteLine("Details: ");
        Console.WriteLine(e.ToString());
      }
    }

    private static string GetTestDataPath
    {
      get
      {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ImageOcrExtractor)).Location), TESDATA_NAME);
      }
    }
  }
}
