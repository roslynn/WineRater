using System;
using System.Collections.Generic;
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
  public class ImageOcrExtractor : IDisposable
  {
    private static readonly string TESDATA_NAME = "tessdata";
    private bool _debug;
    private TesseractEngine _engine;
    private Dictionary<PageSegMode, string> _processedText = new Dictionary<PageSegMode, string>();

    public ImageOcrExtractor(bool debug = false)
    {
      _debug = debug;
      _engine = new TesseractEngine(ImageOcrExtractor.GetTestDataPath, "eng", EngineMode.Default);
    }

    private static string GetTestDataPath =>
      Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ImageOcrExtractor)).Location), TESDATA_NAME);

    public Dictionary<PageSegMode, string> Results => _processedText;

    //todo: once all experimenting with debugging the engine is finished 
    // that should be the main property to populate (not _processedText)
    public string Text => _processedText[PageSegMode.Auto];


    public void Process(string imageNameWithExtension)
    {
      try
      {
        using (Pix img = Pix.LoadFromFile(imageNameWithExtension))
        {
          DoProcess(img);
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

    public void Process(byte[] rawImage)
    {
      try
      {
        using (Pix img = Pix.LoadTiffFromMemory(rawImage))
        {
          DoProcess(img);
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

    private void DoProcess(Pix img)
    {
      if (_debug)
      {
        foreach (var mode in Enum.GetNames(typeof(PageSegMode)))
        {
          Enum.TryParse<PageSegMode>(mode, out var modeEnum);
          using (var page = _engine.Process(img, modeEnum))
          {
            try
            {
              _processedText.Add(page.PageSegmentMode, page.GetText());
              //var mean = page.GetMeanConfidence();
            }
            catch (Exception e)
            {
              _processedText.Add(page.PageSegmentMode, "<error>");
              continue;
            }
          }
        }
      }
      else
      {
        using (var page = _engine.Process(img, PageSegMode.Auto))
        {
          try
          {
            _processedText.Add(page.PageSegmentMode, page.GetText());
            //var mean = page.GetMeanConfidence();
          }
          catch (Exception e)
          {
            _processedText.Add(page.PageSegmentMode, "<error>");
          }
        }
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _engine.Dispose();
      }
    }

  }
}
