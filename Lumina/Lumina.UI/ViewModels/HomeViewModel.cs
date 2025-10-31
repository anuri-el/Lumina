using Lumina.UI.Commands;
using Lumina.UI.Services;
using System.Windows.Input;

namespace Lumina.UI.ViewModels
{
    public class HomeViewModel
    {
        private readonly NavigationService _navigationService;

        public ICommand OpenImageCommand { get; }
        public ICommand CreateCollageCommand { get; }
        public ICommand GoToEditorCommand { get; }
        public ICommand ExitCommand { get; }

        public HomeViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenImageCommand = new RelayCommand(OpenImage);
            CreateCollageCommand = new RelayCommand(CreateCollage);
            GoToEditorCommand = new RelayCommand(GoToEditor);
            ExitCommand = new RelayCommand(ExitApp);
        }

        private void OpenImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                _navigationService.NavigateTo($"EditorPage?file={dialog.FileName}");
            }
        }

        private void CreateCollage()
        {
            _navigationService.NavigateTo("EditorPage?collage=new");
        }

        private void GoToEditor()
        {
            _navigationService.NavigateTo("EditorPage");
        }

        private void ExitApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
