using AmaScan.App.Services;
using AmaScan.App.ViewModels;
using Ninject.Modules;

namespace AmaScan.App.Module
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
            Bind<IHistoryService>().To<HistoryService>().InSingletonScope();
        }
    }
}
