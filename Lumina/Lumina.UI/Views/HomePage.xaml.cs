using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Lumina.UI.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void OpenEditor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav?.Navigate(new EditorPage());
        }
    }
}
