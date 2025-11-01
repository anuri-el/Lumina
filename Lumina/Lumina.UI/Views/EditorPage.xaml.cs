using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Lumina.UI.Views
{
    public partial class EditorPage : Page
    {
        public EditorPage()
        {
            InitializeComponent();
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
    }
}
