using Lumina.Core.Facade;
using Lumina.Core.Patterns;
using Lumina.UI.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace Lumina.UI.ViewModels
{
    public class EditorViewModel : INotifyPropertyChanged
    {
        public EditorContext Context { get; }

        private string _logText = "";
        public string LogText
        {
            get => _logText;
            set { _logText = value; OnPropertyChanged(); }
        }
        public string CurrentImagePath { get; set; } = "";
        public Transform CurrentTransform { get; set; } = new RotateTransform(0);
        private readonly ImageFacade _imageFacade;
        public ICommand RotateCommand { get; }
        public ICommand ScaleCommand { get; }
        public ICommand ApplyEffectCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand CropCommand { get; }
        public ICommand DrawCommand { get; }

        public EditorViewModel()
        {
            Context = new EditorContext(new SelectState());

            SelectCommand = new RelayCommand(() => Context.SetState(new SelectState()));
            CropCommand = new RelayCommand(() => Context.SetState(new CropState()));
            DrawCommand = new RelayCommand(() => Context.SetState(new DrawState()));

            RotateCommand = new RelayCommand(() => RotateImage(15));
            ScaleCommand = new RelayCommand(() => ScaleImage(1.1));
            ApplyEffectCommand = new RelayCommand(ApplyEffect);
            SaveCommand = new RelayCommand(SaveImage);
        }

        private void AppendLog(string message)
        {
            LogText += message + "\n";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void RotateImage(double angle)
        {
            var current = (RotateTransform)CurrentTransform;
            CurrentTransform = new RotateTransform(current.Angle + angle);
        }

        private void ScaleImage(double factor)
        {
            CurrentTransform = new ScaleTransform(factor, factor);
        }

        private void ApplyEffect() { /* TODO: виклик через IEffectService */ }

        private void SaveImage() { /* TODO: зберегти через IImageService */ }
    }
}
