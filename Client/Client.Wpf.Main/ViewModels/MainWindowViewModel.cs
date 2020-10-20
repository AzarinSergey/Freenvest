using Client.Wpf.Main.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Client.Wpf.Main.EventAggregator;
using Prism.Events;

namespace Client.Wpf.Main.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IRegion ChartConfigRegion => _regionManager.Regions["DefaultChartConfigurationRegion"];

        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _ea;

        public MainWindowViewModel(IContainerExtension container, IRegionManager regionManager, IEventAggregator ea)
        {
            _container = container;
            _regionManager = regionManager;
            _ea = ea;

            NewChartConfigCommand = new DelegateCommand(OnNewChartConfigCommand);
            ActiveChartConfigCommand = new DelegateCommand<string>(OnActiveChartConfigCommand);
            ChartButtons = new ObservableCollection<MainWindowModel>();
        }

        public ICommand NewChartConfigCommand { get; set; }
        public ICommand ActiveChartConfigCommand { get; set; }

        public string Title => "Strategy tester";
        public ObservableCollection<MainWindowModel> ChartButtons { get; set; }

        private void OnNewChartConfigCommand()
        {
            var uuid = Guid.NewGuid().ToString();
            
            var configView = _container.Resolve<ChartConfiguration>();
            var configVm = (ChartConfigurationViewModel) configView.DataContext;
            configVm.Identifier = uuid;

            ChartConfigRegion.Add(configView, uuid);
            ChartConfigRegion.Activate(configView);

            var model = new MainWindowModel
            {
                Identifier = uuid,
                ChartButton = new Button
                {
                    Command = new DelegateCommand<string>(x => OnActiveChartConfigCommand(uuid))
                }
            };

            model.ChartButton.SetBinding(ContentControl.ContentProperty, new Binding
            {
                Source = configVm,
                Path = new PropertyPath(nameof(configVm.ChartName))
            });
            ChartButtons.Add(model);

            _ea.GetEvent<CloseChartEvent>().Subscribe(OnCloseChartEvent);
        }

        private void OnActiveChartConfigCommand(string uuid)
        {
            ChartConfigRegion.Activate(ChartConfigRegion.GetView(uuid));
        }

        private void OnCloseChartEvent(string uuid)
        {
            var toRm = ChartButtons.FirstOrDefault(x => x.Identifier.Equals(uuid));

            if (toRm != null)
            {
                foreach (var view in ChartConfigRegion.ActiveViews)
                {
                    ChartConfigRegion.Remove(view);
                }

                ChartButtons.Remove(toRm);
            }

            var toActivate = ChartConfigRegion.Views.FirstOrDefault();

            if (toActivate != null)
                ChartConfigRegion.Activate(toActivate);
        }
    }
}
