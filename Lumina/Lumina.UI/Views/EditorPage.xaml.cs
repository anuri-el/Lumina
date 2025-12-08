using Lumina.Core.EditorLogic;
using Lumina.UI.States;
using Lumina.UI.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lumina.UI.Views
{
    public partial class EditorPage : Page
    {
        public EditorRootViewModel ViewModel { get; }

        private readonly Lumina.UI.States.EditorContext _stateContext = new();
        private readonly ImageEditorOriginator _originator = new();
        private readonly EditorHistory _history = new();

        private double _imgX = 50;
        private double _imgY = 50;
        private double _imgW = 400;
        private double _imgH = 300;

        private List<Ellipse> ResizeHandles = new List<Ellipse>();
        private Image? ActiveImage;

        public EditorPage()
        {
            InitializeComponent();
            ViewModel = new EditorRootViewModel();
            DataContext = ViewModel;

            // Підписуємось на лог з Core.Patterns.EditorContext
            ViewModel.Tools.Context.OnLog += Log;

            Log($"Current state: {_stateContext.State.Name}");
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
                        LoadImage(value);
                        PreviewImage.Source = new BitmapImage(new Uri(value));
                        InfoText.Text = $"Opened: {System.IO.Path.GetFileName(value)}";
                        Log($"Loaded image: {value}");
                    }
                    else if (key == "collage" && value == "new")
                    {
                        InfoText.Text = "New empty collage created.";
                        Log("Created new collage");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening image: {ex.Message}");
                Log($"Error: {ex.Message}");
            }
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            _stateContext.SetState(new NormalState());
            Log(_stateContext.PerformAction());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _stateContext.SetState(new EditState());
            Log(_stateContext.PerformAction());
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            _stateContext.SetState(new PreviewState());
            Log(_stateContext.PerformAction());
        }

        private void Log(string message)
        {
            LogBox.Items.Add(message);
            LogBox.ScrollIntoView(LogBox.Items[LogBox.Items.Count - 1]);
        }

        private void LoadImage(string path)
        {
            _originator.SetState(path, 50, 50, 400, 300);
            _history.Push(_originator.Save());
            Log("Image state saved to history");
        }

        private void ApplyResize(double widthDelta, double heightDelta)
        {
            _originator.SetState(
                _originator.CurrentImagePath,
                _originator.PosX,
                _originator.PosY,
                _originator.Width + widthDelta,
                _originator.Height + heightDelta);

            _history.Push(_originator.Save());
            Log($"Resized image: {_originator.Width}x{_originator.Height}");
        }

        private void ApplyEditorStateToUI()
        {
            _imgX = _originator.PosX;
            _imgY = _originator.PosY;
            _imgW = _originator.Width;
            _imgH = _originator.Height;

            if (File.Exists(_originator.CurrentImagePath))
            {
                PreviewImage.Source = new BitmapImage(new Uri(_originator.CurrentImagePath));
                PreviewImage.Width = _imgW;
                PreviewImage.Height = _imgH;
                Canvas.SetLeft(PreviewImage, _imgX);
                Canvas.SetTop(PreviewImage, _imgY);
            }

            UpdateHandles();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            var state = _history.Undo();
            if (state != null)
            {
                _originator.Restore(state);
                ApplyEditorStateToUI();
                Log("Undo executed");
            }
            else
            {
                Log("Nothing to undo");
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            var state = _history.Redo();
            if (state != null)
            {
                _originator.Restore(state);
                ApplyEditorStateToUI();
                Log("Redo executed");
            }
            else
            {
                Log("Nothing to redo");
            }
        }

        private void UpdateHandles()
        {
            if (ActiveImage == null || ResizeHandles == null || ResizeHandles.Count == 0)
                return;

            double x = Canvas.GetLeft(ActiveImage);
            double y = Canvas.GetTop(ActiveImage);
            double w = ActiveImage.Width;
            double h = ActiveImage.Height;

            if (ResizeHandles.Count >= 8)
            {
                // Кути
                MoveHandle(ResizeHandles[0], x - 5, y - 5);               // Лівий верх
                MoveHandle(ResizeHandles[1], x + w - 5, y - 5);           // Правий верх
                MoveHandle(ResizeHandles[2], x - 5, y + h - 5);           // Лівий низ
                MoveHandle(ResizeHandles[3], x + w - 5, y + h - 5);       // Правий низ

                // Центри сторін
                MoveHandle(ResizeHandles[4], x + w / 2 - 5, y - 5);       // Верх центр
                MoveHandle(ResizeHandles[5], x + w - 5, y + h / 2 - 5);   // Право центр
                MoveHandle(ResizeHandles[6], x + w / 2 - 5, y + h - 5);   // Низ центр
                MoveHandle(ResizeHandles[7], x - 5, y + h / 2 - 5);       // Ліво центр
            }
        }

        private void MoveHandle(Ellipse handle, double left, double top)
        {
            Canvas.SetLeft(handle, left);
            Canvas.SetTop(handle, top);
        }
    }
}
