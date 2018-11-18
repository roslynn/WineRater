using Ninject;
using System;
using System.Threading.Tasks;
using Tesseract;

namespace ImageProcessing
{
  public class ImageOcrExtractor : IDisposable
  {
    private static readonly string TESDATA_NAME = "tessdata";
    private bool _debug;
    private ITesseractApi _engine;

    public ImageOcrExtractor(IKernel ioC)
    {
      var engine = ioC.Get<ITesseractApi>();
      if (engine == null)
        throw new ArgumentException($"Parameter {engine} is null");

      _engine = engine;
    }

    public async Task<bool> Init()
    {
      try
      {
        bool initialised = await _engine.Init("eng");
        return initialised;
      }
      catch (Exception e)
      {
        Console.WriteLine("UnexpectedException");
        Console.WriteLine(e.Message);
        return false;
      }
    }

    public async Task<string> Process(byte[] image)
    {
      if (!_engine.Initialized)
        return "Engine not initialized";

      try
      {
        string textResult = "";
        bool success = await _engine.SetImage(image);
        if (success)
        {
          textResult = _engine.Text;
        }
        Console.WriteLine($"RESULT = {textResult}");
        return textResult;
      }
      catch (Exception e)
      {
        Console.WriteLine("UnexpectedException");
        Console.WriteLine(e.Message);
        return "";
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
