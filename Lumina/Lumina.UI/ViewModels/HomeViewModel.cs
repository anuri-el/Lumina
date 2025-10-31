using Lumina.UI.Commands;
using Lumina.UI.Services;
using Lumina.UI.Views;
using System.Windows.Input;

namespace Lumina.UI.ViewModels
{
    public class HomeViewModel
    {
        private readonly NavigationService _navigation;

        public ICommand OpenImageCommand { get; }
        public ICommand NewCollageCommand { get; }
        public ICommand GoToEditorCommand { get; }

        public HomeViewModel(NavigationService navigation)
        {
            _navigation = navigation;
            OpenImageCommand = new RelayCommand(OpenImage);
            NewCollageCommand = new RelayCommand(CreateCollage);
            GoToEditorCommand = new RelayCommand(() =>
                _navigation.Navigate(new EditorPage())
            );
        }

        private void OpenImage() { }
        private void CreateCollage() { }
    }
}
