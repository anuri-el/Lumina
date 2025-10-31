using Lumina.UI.Commands;
using System.Windows.Input;
using System.Windows.Media;

namespace Lumina.UI.ViewModels
{
    public class EditorViewModel
    {
        public string CurrentImagePath { get; set; } = "";
        public Transform CurrentTransform { get; set; } = new RotateTransform(0);

        public ICommand RotateCommand { get; }
        public ICommand ScaleCommand { get; }
        public ICommand ApplyEffectCommand { get; }
        public ICommand SaveCommand { get; }

        public EditorViewModel()
        {
            RotateCommand = new RelayCommand(() => RotateImage(15));
            ScaleCommand = new RelayCommand(() => ScaleImage(1.1));
            ApplyEffectCommand = new RelayCommand(ApplyEffect);
            SaveCommand = new RelayCommand(SaveImage);
        }

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
