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

        // Вибране зображення
        private CollageImageInfo? _selectedImage = null;
        private Border? _selectionBorder = null;

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
                    Stretch = Stretch.Uniform,
                    Cursor = System.Windows.Input.Cursors.Hand
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
                    X = 50 + offsetX,
                    Y = 50 + offsetY,
                    Width = 200,
                    Height = 200,
                    Rotation = 0
                };
                _collageImages.Add(collageImage);

                // Зберігаємо стан в історію
                _originator.SetState(imagePath, 50 + offsetX, 50 + offsetY, 200, 200);
                _history.Push(_originator.Save());

                InfoText.Text = $"Images in collage: {_collageImages.Count}";
                Log($"Added image to collage: {System.IO.Path.GetFileName(imagePath)} at position ({50 + offsetX}, {50 + offsetY})");

                // Додаємо можливість вибору та переміщення зображення
                AddInteractiveBehavior(image, collageImage);

                // Автоматично вибираємо нове зображення
                SelectImage(collageImage);
            }
            catch (Exception ex)
            {
                Log($"Error adding image: {ex.Message}");
                MessageBox.Show($"Error adding image: {ex.Message}");
            }
        }

        private void AddInteractiveBehavior(Image image, CollageImageInfo info)
        {
            bool isDragging = false;
            Point clickPosition = new Point();

            image.MouseLeftButtonDown += (s, e) =>
            {
                SelectImage(info);
                isDragging = true;
                clickPosition = e.GetPosition(MainCanvas);
                image.CaptureMouse();
                e.Handled = true;
            };

            image.MouseMove += (s, e) =>
            {
                if (isDragging && image.IsMouseCaptured)
                {
                    Point currentPosition = e.GetPosition(MainCanvas);
                    double newX = info.X + (currentPosition.X - clickPosition.X);
                    double newY = info.Y + (currentPosition.Y - clickPosition.Y);

                    Canvas.SetLeft(image, newX);
                    Canvas.SetTop(image, newY);

                    info.X = newX;
                    info.Y = newY;

                    UpdateSelectionBorder();

                    clickPosition = currentPosition;
                }
            };

            image.MouseLeftButtonUp += (s, e) =>
            {
                if (isDragging)
                {
                    isDragging = false;
                    image.ReleaseMouseCapture();

                    // Зберігаємо новий стан в історію
                    _originator.SetState(info.Path, info.X, info.Y, info.Width, info.Height);
                    _history.Push(_originator.Save());

                    Log($"Moved image to ({info.X:F0}, {info.Y:F0})");
                }
            };
        }

        private void SelectImage(CollageImageInfo info)
        {
            _selectedImage = info;

            // Видаляємо стару рамку
            if (_selectionBorder != null && MainCanvas.Children.Contains(_selectionBorder))
            {
                MainCanvas.Children.Remove(_selectionBorder);
            }

            // Створюємо нову рамку вибору
            _selectionBorder = new Border
            {
                BorderBrush = Brushes.DodgerBlue,
                BorderThickness = new Thickness(2),
                Width = info.Width,
                Height = info.Height,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(_selectionBorder, info.X);
            Canvas.SetTop(_selectionBorder, info.Y);
            MainCanvas.Children.Add(_selectionBorder);

            Log($"Selected image: {System.IO.Path.GetFileName(info.Path)}");
            InfoText.Text = $"Selected: {System.IO.Path.GetFileName(info.Path)} | Images: {_collageImages.Count}";
        }

        private void UpdateSelectionBorder()
        {
            if (_selectionBorder != null && _selectedImage != null)
            {
                Canvas.SetLeft(_selectionBorder, _selectedImage.X);
                Canvas.SetTop(_selectionBorder, _selectedImage.Y);
                _selectionBorder.Width = _selectedImage.Width;
                _selectionBorder.Height = _selectedImage.Height;
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

        private void Rotate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                Log("No image selected for rotation");
                return;
            }

            _selectedImage.Rotation += 15;

            // Застосовуємо трансформацію
            var rotateTransform = new RotateTransform(_selectedImage.Rotation,
                _selectedImage.Width / 2,
                _selectedImage.Height / 2);
            _selectedImage.Image.RenderTransform = rotateTransform;

            // Зберігаємо стан
            _originator.SetState(_selectedImage.Path, _selectedImage.X, _selectedImage.Y,
                _selectedImage.Width, _selectedImage.Height);
            _history.Push(_originator.Save());

            Log($"Rotated image to {_selectedImage.Rotation}°");
        }

        private void Scale_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                Log("No image selected for scaling");
                return;
            }

            double scaleFactor = 1.1;
            _selectedImage.Width *= scaleFactor;
            _selectedImage.Height *= scaleFactor;

            _selectedImage.Image.Width = _selectedImage.Width;
            _selectedImage.Image.Height = _selectedImage.Height;

            UpdateSelectionBorder();

            // Зберігаємо стан
            _originator.SetState(_selectedImage.Path, _selectedImage.X, _selectedImage.Y,
                _selectedImage.Width, _selectedImage.Height);
            _history.Push(_originator.Save());

            Log($"Scaled image to {_selectedImage.Width:F0}x{_selectedImage.Height:F0}");
        }

        private void ApplyBlur_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                Log("No image selected for blur effect");
                return;
            }

            var blur = new System.Windows.Media.Effects.BlurEffect { Radius = 5 };
            _selectedImage.Image.Effect = blur;

            Log($"Applied blur effect to {System.IO.Path.GetFileName(_selectedImage.Path)}");
        }

        private void ApplyGrayscale_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                Log("No image selected for grayscale effect");
                return;
            }

            var grayscale = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = Colors.Black,
                ShadowDepth = 0,
                BlurRadius = 0
            };
            _selectedImage.Image.Effect = grayscale;

            Log($"Applied grayscale effect to {System.IO.Path.GetFileName(_selectedImage.Path)}");
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
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public double Rotation { get; set; }
        }
    }
}
