using Client.Wpf.Main.EventAggregator;
using Client.Wpf.Main.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using StrategyTester.Common.Enums;
using StrategyTester.Contract;
using StrategyTester.Contract.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Wpf.Main.ViewModels
{
    public class ChartConfigurationViewModel : BindableBase
    {
        private readonly IEventAggregator _ea;
        private readonly IStrategyTester _strategyTesterDomain;

        private CandleContractModel[] _currentStockItemHistory;
        private string _identifier;
        private string _chartName = "Chart";
        private string _selectedTimeFrame;
        private string _selectedDataFile;
        private string _createOrUpdateChartButtonContent = " VIEW CHART ";
        private readonly ChartWindow _chartWindow;
        private ChartWindowViewModel _chartWindowViewModel;

        public string Identifier
        {
            get => _identifier;
            set => SetProperty(ref _identifier, value);
        }

        public string ChartName
        {
            get => _chartName;
            set => SetProperty(ref _chartName, value);
        }

        public string CreateOrUpdateChartButtonContent
        {
            get => _createOrUpdateChartButtonContent;
            set => SetProperty(ref _createOrUpdateChartButtonContent, value);
        }

        public string[] TimeFrameList => Enum.GetNames(typeof(CandlePeriod));

        public string SelectedTimeFrame
        {
            get => _selectedTimeFrame;
            set => SetProperty(ref _selectedTimeFrame, value);
        }

        public ObservableCollection<string> CurrentStockItemHistoryView { get; set; }
        public ObservableCollection<string> DataFileList { get; set; }

        public string SelectedDataFile
        {
            get => _selectedDataFile;
            set => SetProperty(ref _selectedDataFile, value);
        }

        public ChartConfigurationViewModel(IEventAggregator ea, IStrategyTester strategyTesterDomain, ChartWindow chartWindow)
        {
            _ea = ea;
            _strategyTesterDomain = strategyTesterDomain;
            _chartWindow = chartWindow;
            RemoveChart = new DelegateCommand(OnRemoveChart);
            LoadData = new DelegateCommand(OnLoadData);
            CreateOrUpdateChart = new DelegateCommand(OnCreateOrUpdateChart);

            SelectedTimeFrame = TimeFrameList.FirstOrDefault();
            CurrentStockItemHistoryView = new ObservableCollection<string>();

            LoadStockItems()
                .GetAwaiter()
                .GetResult();
        }

        private void OnCreateOrUpdateChart()
        {
            if (CurrentStockItemHistoryView.Count < 1)
                OnLoadData();

            if (!_chartWindow.IsLoaded)
            {
                _chartWindowViewModel = (ChartWindowViewModel)_chartWindow.DataContext;
                _chartWindowViewModel.Identifier = Identifier;
                _chartWindow.Closed += (sender, args) => OnRemoveChart();
                CreateOrUpdateChartButtonContent = " UPDATE VIEW ";

                _chartWindow.Show();
            }

            ChartName = $"{SelectedDataFile} ({SelectedTimeFrame})";
            _chartWindowViewModel.Title = ChartName;
            _chartWindowViewModel.Data = _currentStockItemHistory;
            _chartWindowViewModel.ChartPeriod = Enum.Parse<CandlePeriod>(SelectedTimeFrame);
            _chartWindowViewModel.RenderChart();
            _chartWindow.Activate();
        }

        private async Task LoadStockItems()
        {
            var titles = await _strategyTesterDomain.GetStockTitles(CancellationToken.None);
            DataFileList = new ObservableCollection<string>(titles);
            SelectedDataFile = titles.FirstOrDefault();
        }

        public ICommand RemoveChart { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand CreateOrUpdateChart { get; set; }

        private bool _chartWindowClosed;
        private void OnRemoveChart()
        {
            if (!_chartWindowClosed)
            {
                _chartWindowClosed = true;
                _chartWindow.Close();
            }

            _ea.GetEvent<CloseChartEvent>().Publish(Identifier);
        }

        /// <summary>
        /// Everyone's first instinct is that they need an AsyncCommand, but that assumption is wrong.
        /// ICommand by nature is synchronous, and the Execute and CanExecute delegates should be considered events.
        /// This means that async void is a perfectly valid syntax to use for commands.
        /// </summary>
        private async void OnLoadData()
        {
            var hist = await _strategyTesterDomain.GetStockHistory(SelectedDataFile, Enum.Parse<CandlePeriod>(SelectedTimeFrame), CancellationToken.None);
            _currentStockItemHistory = hist.Data.ToArray();

            CurrentStockItemHistoryView.Clear();
            CurrentStockItemHistoryView
                .AddRange(_currentStockItemHistory.Select((x,i) => $"№{i}: O:{x.Open}, H:{x.High}, C: {x.Close}, L:{x.Low}, Vol:{x.Volume}"));
        }
    }
}