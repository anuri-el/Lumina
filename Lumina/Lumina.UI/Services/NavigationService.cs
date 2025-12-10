using System.Windows;
using System.Windows.Controls;

namespace Lumina.UI.Services
{
    public class NavigationService
    {
        private readonly Frame _frame;
        private readonly Dictionary<string, Uri> _routes = new()
        {
            { "HomePage", new Uri("Views/HomePage.xaml", UriKind.Relative) },
            { "EditorPage", new Uri("Views/EditorPage.xaml", UriKind.Relative) },
            { "ServerEditorPage", new Uri("Views/ServerEditorPage.xaml", UriKind.Relative) }
        };

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(string route)
        {
            try
            {
                string pageName = route.Split('?')[0];
                if (_routes.TryGetValue(pageName, out var uri))
                {
                    // Завантажуємо сторінку з XAML напряму
                    var page = (Page)Application.LoadComponent(uri);

                    // Передача параметрів
                    if (route.Contains("?"))
                    {
                        string query = route.Split('?')[1];
                        (page as dynamic)?.OnNavigated(query);
                    }

                    _frame.Navigate(page);
                }
                else
                {
                    MessageBox.Show($"Route not found: {pageName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Navigation error: {ex.Message}");
            }
        }
    }
}
