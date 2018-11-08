using System;
using System.IO;
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
    public delegate void PhotoResultEventHandler(PhotoResultEventArgs result);
    public event PhotoResultEventHandler OnPhotoResult;

    public ExamineWinePage()
    {
      InitializeComponent();
    }

    public void SetPhotoResult(byte[] image, int width = -1, int height = -1)
    {
      WinePicture.Source = ImageSource.FromStream(() => new MemoryStream(image));
      OnPhotoResult?.Invoke(new PhotoResultEventArgs(image, width, height));
    }

    public void Cancel()
    {
      OnPhotoResult?.Invoke(new PhotoResultEventArgs());
    }
  }
}