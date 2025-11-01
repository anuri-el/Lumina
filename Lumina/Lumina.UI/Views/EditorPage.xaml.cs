using Lumina.UI.States;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lumina.UI.Views
{
    public partial class EditorPage : Page
    {
        private readonly EditorContext _context = new EditorContext();

        public EditorPage()
        {
            InitializeComponent();
            Log($"Current state: {_context.State.Name}");
        }

        public void OnNavigated(string query)
        {
            try
            {
                var parts = query.Split('=');
                if (parts.Length == 2)
                {
                    string key = parts[0];
                    string value = parts[1];

                    if (key == "file" && File.Exists(value))
                    {
                        PreviewImage.Source = new BitmapImage(new Uri(value));
                        InfoText.Text = $"Opened: {System.IO.Path.GetFileName(value)}";
                    }
                    else if (key == "collage" && value == "new")
                    {
                        InfoText.Text = "New empty collage created.";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error opening image: {ex.Message}");
            }
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            _context.SetState(new NormalState());
            Log(_context.PerformAction());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _context.SetState(new EditState());
            Log(_context.PerformAction());
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            _context.SetState(new PreviewState());
            Log(_context.PerformAction());
        }

        private void Log(string message)
        {
            LogBox.Items.Add(message);
        }
    }
}
