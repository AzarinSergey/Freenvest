using System.Windows.Controls;
using Prism.Mvvm;

namespace Client.Wpf.Main.ViewModels
{
    public class MainWindowModel : BindableBase
    {
        private string _identifier;
        private Button _chartButton;

        public string Identifier
        {
            get => _identifier;
            set => SetProperty(ref _identifier, value);
        }

        public Button ChartButton
        {
            get => _chartButton;
            set => SetProperty(ref _chartButton, value);
        }
    }
}