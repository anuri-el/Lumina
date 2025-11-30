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

        private readonly EditorContext _context = new EditorContext();

        private readonly ImageEditorOriginator _editor = new();
        private readonly EditorHistory _history = new();

        private readonly ImageEditorOriginator _originator = new();

        private double _imgX = 50;
        private double _imgY = 50;
        private double _imgW = 400;
        private double _imgH = 300;

        private readonly Point[] _handleOffsets =
        {
            new(-1,-1), new(0,-1), new(1,-1),
            new(-1,0),              new(1,0),
            new(-1,1),  new(0,1),  new(1,1)
        };

        private List<Ellipse> ResizeHandles = new List<Ellipse>();
        private Image ActiveImage;

        public EditorPage()
        {
            InitializeComponent();
            Log($"Current state: {_context.State.Name}");
            ViewModel = new EditorRootViewModel();
            DataContext = ViewModel;
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

        private void LoadImage(string path)
        {
            _editor.SetState(path, 50, 50, 400, 300);
            _history.Push(_editor.Save());   // зберігаємо початковий стан
        }

        private void ApplyResize()
        {
            _editor.SetState(
                _editor.CurrentImagePath,
                _editor.PosX,
                _editor.PosY,
                _editor.Width + 20,
                _editor.Height + 20);

            _history.Push(_editor.Save());
        }

        private void ApplyEditorStateToUI()
        {
            _imgX = _originator.PosX;
            _imgY = _originator.PosY;
            _imgW = _originator.Width;
            _imgH = _originator.Height;

            if (File.Exists(_originator.CurrentImagePath))
                PreviewImage.Source = new BitmapImage(new Uri(_originator.CurrentImagePath));

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

            // Точки по кутах
            MoveHandle(ResizeHandles[0], x - 5, y - 5);               // Лівий верх
            MoveHandle(ResizeHandles[1], x + w - 5, y - 5);           // Правий верх
            MoveHandle(ResizeHandles[2], x - 5, y + h - 5);           // Лівий низ
            MoveHandle(ResizeHandles[3], x + w - 5, y + h - 5);       // Правий низ

            // Точки по центру сторін
            MoveHandle(ResizeHandles[4], x + w / 2 - 5, y - 5);       // Верх центр
            MoveHandle(ResizeHandles[5], x + w - 5, y + h / 2 - 5);   // Право центр
            MoveHandle(ResizeHandles[6], x + w / 2 - 5, y + h - 5);   // Низ центр
            MoveHandle(ResizeHandles[7], x - 5, y + h / 2 - 5);       // Ліво центр
        }

        private void MoveHandle(Ellipse handle, double left, double top)
        {
            Canvas.SetLeft(handle, left);
            Canvas.SetTop(handle, top);
        }
    }
}
