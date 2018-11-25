using System;
using Android.Content;
using Tesseract;
using Tesseract.Droid;
using WineRater.CommonTypes;

namespace WineRater.Droid
{
  public static class Bootstrap
  {
    public static void Initialize(Context context)
    {
      WineRater.Bootstrap.Initialize();// Create Ninject DI Kernel 
      RegisterServices(context); // Tell ASP.NET MVC 3 to use our Ninject DI Container
      //WineRater.Bootstrap.InjectResources();
    }

    private static void RegisterServices(Context context)
    {
      try
      {
        WineRater.Bootstrap.IoC.Bind<ITesseractApi>().ToMethod((cont) =>
        {
          return new TesseractApi(context, AssetsDeployment.OncePerInitialization);
        });
        WineRater.Bootstrap.IoC.Bind<ISaveToLocalStorage>().To<SaveToLocalStorage>();
      }
      catch (Exception e)
      {
        Console.WriteLine("Unhandled exception during initialization process of the image processing engine");
        Console.WriteLine(e.Message);
      }
    }
  }
}