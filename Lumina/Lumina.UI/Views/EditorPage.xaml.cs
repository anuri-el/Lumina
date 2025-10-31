using Lumina.UI.ViewModels;
using System.Windows.Controls;

namespace Lumina.UI.Views
{
    public partial class EditorPage : Page
    {
        public EditorPage()
        {
            InitializeComponent();
            DataContext = new EditorViewModel();
        }
    }
}
