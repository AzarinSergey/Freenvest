using Client.Wpf.Main.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Client.Wpf.Main.Modules
{
    public class MainMenuModule : IModule
    {
        public const string Region = "MainMenuRegion";

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Region, typeof(MainMenu));
        }
    }
}
