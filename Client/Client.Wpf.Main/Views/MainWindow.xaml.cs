using System.Windows;
using Client.Wpf.Main.EventAggregator;
using Prism.Events;

namespace Client.Wpf.Main.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventAggregator _ea;

        public MainWindow(IEventAggregator ea)
        {
            _ea = ea;
            Closed += (sender, args) => _ea.GetEvent<MainWindowClosedEvent>().Publish();
            InitializeComponent();
        }
    }
}
