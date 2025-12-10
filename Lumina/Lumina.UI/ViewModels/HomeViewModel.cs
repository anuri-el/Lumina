using Lumina.UI.Commands;
using Lumina.UI.Services;
using System.Windows.Input;

namespace Lumina.UI.ViewModels
{
    public class HomeViewModel
    {
        private readonly NavigationService _navigationService;
        public ICommand OpenEditorCommand { get; }
        public ICommand OpenServerEditorCommand { get; }
        public ICommand ExitCommand { get; }

        public HomeViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            OpenEditorCommand = new RelayCommand(OpenEditor);
            OpenServerEditorCommand = new RelayCommand(OpenServerEditor);
            ExitCommand = new RelayCommand(ExitApp);
        }

        private void OpenEditor()
        {
            _navigationService.NavigateTo("EditorPage");
        }

        private void OpenServerEditor()
        {
            _navigationService.NavigateTo("ServerEditorPage");
        }

        private void ExitApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
