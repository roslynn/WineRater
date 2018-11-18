using Ninject;

namespace WineRater
{
  public static class Bootstrap
  {
    public static IKernel IoC { get; } = new StandardKernel(); // Register services with our Ninject DI Container 
    
    public static void Initialize()
    {
      //todo init here
    }
  }
}
