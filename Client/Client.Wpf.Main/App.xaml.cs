using Client.Wpf.Main.Modules;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using StrategyTester.Contract;
using StrategyTester.Domain;

namespace Client.Wpf.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IStrategyTester, StrategyTesterImplementation>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainMenuModule>();
            //moduleCatalog.AddModule<ChartConfigurationModule>();
        }
    }
}
