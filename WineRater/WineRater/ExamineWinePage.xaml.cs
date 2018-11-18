using ImageProcessing;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WineRater
{
  public class PhotoResultEventArgs : EventArgs
  {
    public PhotoResultEventArgs()
    {
      Success = false;
    }

    public PhotoResultEventArgs(byte[] image, int width, int height)
    {
      Success = true;
      Image = image;
      Width = width;
      Height = height;
    }

    public byte[] Image { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public bool Success { get; private set; }
  }

  public partial class ExamineWinePage : ContentPage
  {
    private ImageOcrExtractor _imageProcessor;

    public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);
    public event PhotoResultEventHandler OnPhotoResult;

    public ExamineWinePage()
    {
      InitializeComponent();
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
      WinePicture.Source = ImageSource.FromStream(() => new MemoryStream(image));
      OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height));

      try
      {
        var message = await ProcessImageAnalysisAsync(image);
        ReviewLabel.LabelContent = message;
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

    public void Cancel()
    {
      OnPhotoResult?.Invoke(new PhotoResultEventArgs());
    }

  }
}