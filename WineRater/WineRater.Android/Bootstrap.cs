using System;
using System.Threading.Tasks;
using Android.Content;
using Ninject;
using Tesseract;
using Tesseract.Droid;

namespace WineRater.Droid
{
  public static class Bootstrap
  {
    public async static void Initialize(Context context)
    {
      WineRater.Bootstrap.Initialize();// Create Ninject DI Kernel 
      RegisterServices(context); // Tell ASP.NET MVC 3 to use our Ninject DI Container
    }

    private async static Task RegisterServices(Context context)
    {
      try
      {
        WineRater.Bootstrap.IoC.Bind<ITesseractApi>().ToMethod((cont) =>
        {
          return new TesseractApi(context, AssetsDeployment.OncePerInitialization);
        });
        //(new TesseractApi(context, AssetsDeployment.OncePerVersion));
      }
      catch (Exception e)
      {
        Console.WriteLine("Unhandled exception during initialization process of the image processing engine");
        Console.WriteLine(e.Message);
      }
    }
  }
}