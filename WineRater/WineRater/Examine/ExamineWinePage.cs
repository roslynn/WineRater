using ImageProcessing;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WineRater.Examine
{
  /// <summary>
  /// This page has its own platform specific view because it requires the camera
  /// Droid: CameraPageRenderer
  /// </summary>
  public class ExamineWinePage : ContentPage
  {
    private ImageOcrExtractor _imageProcessor;

    public ExamineWinePage()
    {
      _imageProcessor = new ImageOcrExtractor(Bootstrap.IoC);
    }

    internal async Task Init()
    {
      try
      {
        await _imageProcessor.Init();
      }
      catch (Exception e)
      {
        Console.WriteLine("Image processor initialization went wrong");
        Console.WriteLine(e.Message);
      }
    }

    public async void SetPhotoResultAsync(byte[] image, int width = -1, int height = -1)
    {
      try
      {
        var message = await ProcessImageAnalysisAsync(image);
        await Navigation.PushAsync(new CapturedWinePage(image, message));
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    private async Task<string> ProcessImageAnalysisAsync(byte[] picture)
    {
      try
      {
        var result = await _imageProcessor.Process(picture);
        return result;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return "?? Exception ??";
      }
    }

  }
}