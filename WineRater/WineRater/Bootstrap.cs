using Ninject;
using System.IO;
using System.Linq;
using System.Reflection;
using WineRater.CommonTypes;

namespace WineRater
{
  public static class Bootstrap
  {
    public static readonly string Temp_File_Name = "temp";

    public static IKernel IoC { get; } = new StandardKernel(); // Register services with our Ninject DI Container 
    
    public static void Initialize()
    {
      // only internal setup
    }

    /// <summary>
    /// This method tried to overcome the Button.Image not accepting ImageSource
    /// but FileImageSource
    /// see: https://stackoverflow.com/questions/45212166/xamarin-add-image-to-my-button-from-pcl-not-from-resources
    /// </summary>
    public static void InjectResources()
    {
      var saver = IoC.Get<ISaveToLocalStorage>();
      var assembly = typeof(Bootstrap).GetTypeInfo().Assembly;
      var imgCollection = assembly.GetManifestResourceNames()
                                  .Where(img => img.Contains(".png"));

      foreach (var resourceNameToCopy in imgCollection)
      {
        using (var resource = assembly.GetManifestResourceStream(resourceNameToCopy))
        {
          saver.SaveImageInPicturesFolder(Path.Combine(Temp_File_Name, resourceNameToCopy), resource);
        }
      }
    }
  }
}
