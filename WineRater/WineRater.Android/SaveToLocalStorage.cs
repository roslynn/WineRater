using System.IO;
using WineRater.CommonTypes;

namespace WineRater.Droid
{
  public sealed class SaveToLocalStorage : ISaveToLocalStorage
  {
    private string _picturesFolder = null;

    public string PicturesFolder
    {
      get
      {
        if (_picturesFolder == null)
        {
          var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
          _picturesFolder = Path.Combine(dir.AbsolutePath, "WineRater");

          try
          {
            if (!Directory.Exists(_picturesFolder))
              Directory.CreateDirectory(_picturesFolder);
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine(e.ToString());
          }
        }
        return _picturesFolder;
      }
    }

    public string DocumentsFolder
    {
      get
      {
        if (_picturesFolder == null)
        {
          var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments);
          _picturesFolder = Path.Combine(dir.AbsolutePath, "WineRater");

          try
          {
            if (!Directory.Exists(_picturesFolder))
              Directory.CreateDirectory(_picturesFolder);
          }
          catch (System.Exception e)
          {
            System.Console.WriteLine(e.ToString());
          }
        }
        return _picturesFolder;
      }
    }

    public void SaveImageInPicturesFolder(string name, byte[] imageData)
    {
      string filePath = CheckAndCombinePath(name);
      try
      {
        File.WriteAllBytes(filePath, imageData);
      }
      catch (System.Exception e)
      {
        System.Console.WriteLine(e.ToString());
      }
    }

    public void SaveImageInPicturesFolder(string fileName, Stream resource)
    {
      try
      {
        var path = CheckAndCombinePath(fileName);

        using (var fileStream = File.Open(path, FileMode.OpenOrCreate))
        {
          resource.CopyTo(fileStream);
        }
      }
      catch (System.Exception e)
      {
        System.Console.WriteLine(e.ToString());
      }
    }

    private string CheckAndCombinePath(string fileName)
    {
      var combined = Path.Combine(DocumentsFolder, fileName);

      try
      {
        if (!Directory.Exists(combined))
          Directory.CreateDirectory(Path.GetDirectoryName(combined));

        //mediascan adds the saved image into the gallery  
        //var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
        //mediaScanIntent.SetData(Uri.FromFile(new File(filePath)));
        //Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
      }
      catch (System.Exception e)
      {
        System.Console.WriteLine(e.ToString());
      }

      return combined;
    }
  }
}