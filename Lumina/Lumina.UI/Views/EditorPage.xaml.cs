using Lumina.Core.EditorLogic;
using Lumina.UI.States;
using Lumina.UI.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        // Список зображень для колажу
        private List<CollageImageInfo> _collageImages = new();

        public EditorPage()
        {
            InitializeComponent();
            ViewModel = new EditorRootViewModel();
            DataContext = ViewModel;

            // Підписуємось на лог з Core.Patterns.EditorContext
            ViewModel.Tools.Context.OnLog += Log;

            // Підписуємось на подію відкриття зображення
            ViewModel.Tools.OnImageOpened += AddImageToCollage;

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
                        AddImageToCollage(value);
                        Log($"Loaded initial image: {value}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening image: {ex.Message}");
                Log($"Error: {ex.Message}");
            }
        }

        private void AddImageToCollage(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    Log($"File not found: {imagePath}");
                    return;
                }

                // Створюємо нове зображення на Canvas
                var bitmap = new BitmapImage(new Uri(imagePath));
                var image = new Image
                {
                    Source = bitmap,
                    Width = 200,
                    Height = 200,
                    Stretch = Stretch.Uniform
                };

                // Позиціонуємо зображення з невеликим зсувом від попереднього
                double offsetX = _collageImages.Count * 20;
                double offsetY = _collageImages.Count * 20;

                Canvas.SetLeft(image, 50 + offsetX);
                Canvas.SetTop(image, 50 + offsetY);

                // Додаємо на Canvas
                MainCanvas.Children.Add(image);

                // Зберігаємо інформацію
                var collageImage = new CollageImageInfo
                {
                    Image = image,
                    Path = imagePath,
                    InitialX = 50 + offsetX,
                    InitialY = 50 + offsetY
                };
                _collageImages.Add(collageImage);

                // Зберігаємо стан в історію
                _originator.SetState(imagePath, 50 + offsetX, 50 + offsetY, 200, 200);
                _history.Push(_originator.Save());

                InfoText.Text = $"Images in collage: {_collageImages.Count}";
                Log($"Added image to collage: {System.IO.Path.GetFileName(imagePath)} at position ({50 + offsetX}, {50 + offsetY})");

                // Додаємо можливість переміщення зображення
                AddDragBehavior(image, collageImage);
            }
            catch (Exception ex)
            {
                Log($"Error adding image: {ex.Message}");
                MessageBox.Show($"Error adding image: {ex.Message}");
            }
        }

        private void AddDragBehavior(Image image, CollageImageInfo info)
        {
            bool isDragging = false;
            Point clickPosition = new Point();

            image.MouseLeftButtonDown += (s, e) =>
            {
                isDragging = true;
                clickPosition = e.GetPosition(MainCanvas);
                image.CaptureMouse();
                Log($"Started dragging image: {System.IO.Path.GetFileName(info.Path)}");
            };

            image.MouseMove += (s, e) =>
            {
                if (isDragging && image.IsMouseCaptured)
                {
                    Point currentPosition = e.GetPosition(MainCanvas);
                    double newX = Canvas.GetLeft(image) + (currentPosition.X - clickPosition.X);
                    double newY = Canvas.GetTop(image) + (currentPosition.Y - clickPosition.Y);

                    Canvas.SetLeft(image, newX);
                    Canvas.SetTop(image, newY);

                    clickPosition = currentPosition;

                    info.InitialX = newX;
                    info.InitialY = newY;
                }
            };

            image.MouseLeftButtonUp += (s, e) =>
            {
                if (isDragging)
                {
                    isDragging = false;
                    image.ReleaseMouseCapture();

                    // Зберігаємо новий стан в історію
                    _originator.SetState(info.Path, info.InitialX, info.InitialY, image.Width, image.Height);
                    _history.Push(_originator.Save());

                    Log($"Moved image to ({info.InitialX:F0}, {info.InitialY:F0})");
                }
            };
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
            if (LogBox.Items.Count > 0)
                LogBox.ScrollIntoView(LogBox.Items[LogBox.Items.Count - 1]);
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            var state = _history.Undo();
            if (state != null)
            {
                _originator.Restore(state);
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
                Log("Redo executed");
            }
            else
            {
                Log("Nothing to redo");
            }
        }

        // Клас для зберігання інформації про зображення в колажі
        private class CollageImageInfo
        {
            public Image Image { get; set; } = null!;
            public string Path { get; set; } = "";
            public double InitialX { get; set; }
            public double InitialY { get; set; }
        }
    }
}
