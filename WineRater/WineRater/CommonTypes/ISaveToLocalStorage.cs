using System.IO;

namespace WineRater.CommonTypes
{
  public interface ISaveToLocalStorage
  {
    string PicturesFolder { get; }
    string DocumentsFolder { get; }

    void SaveImageInPicturesFolder(string name, byte[] imageData);
    void SaveImageInPicturesFolder(string fileName, Stream resource);
  }
}
