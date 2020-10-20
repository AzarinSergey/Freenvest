using Prism.Mvvm;
using System.Windows.Input;

namespace Client.Wpf.Main.ViewModels
{
    public class MainMenuViewModel : BindableBase
    {
        private string _showConsoleLabelText = "Show Console";

        public string ShowConsoleLabelText
        {
            get => _showConsoleLabelText;
            set => SetProperty(ref _showConsoleLabelText, value);
        }

        public ICommand ExitApplicationCommand { get; set; }
        public ICommand ShowConsoleCommand { get; set; }
    }
}
