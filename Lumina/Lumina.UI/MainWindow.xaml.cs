using Lumina.UI.Services;
using Lumina.UI.Views;
using System.Windows;

namespace Lumina.UI
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;

        public MainWindow()
        {
            InitializeComponent();

            _navigationService = new NavigationService(MainFrame);

            var homePage = new HomePage();
            MainFrame.Navigate(homePage);
        }
    }
}