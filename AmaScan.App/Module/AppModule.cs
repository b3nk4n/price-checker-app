using AmaScan.App.ViewModels;
using Ninject.Modules;
using System;

namespace AmaScan.App.Module
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
        }
    }
}
