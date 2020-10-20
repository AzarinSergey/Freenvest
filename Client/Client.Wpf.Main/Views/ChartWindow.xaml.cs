using Client.Wpf.Main.EventAggregator;
using Prism.Events;
using System.Windows;
using Client.Wpf.Main.ViewModels;

namespace Client.Wpf.Main.Views
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        private readonly IEventAggregator _ea;

        public ChartWindow(IEventAggregator ea)
        {
            _ea = ea;
            InitializeComponent();

            DataContext = new ChartWindowViewModel(ChartViewer);

            _ea.GetEvent<MainWindowClosedEvent>()
                .Subscribe(Close);
        }
    }
}
