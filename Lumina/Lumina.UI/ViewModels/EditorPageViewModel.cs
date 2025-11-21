using CommunityToolkit.Mvvm.Input;
using Lumina.Core.Effects;
using Lumina.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Lumina.UI.ViewModels
{
    public class EditorPageViewModel
    {
        public ObservableCollection<ImageLayer> Layers { get; set; } = new();

        public ICommand DuplicateLayerCommand { get; }
        public ICommand ApplyBlurCommand { get; }
        public ICommand ApplyGrayscaleCommand { get; }
        public ICommand ApplySepiaCommand { get; }

        public EditorPageViewModel()
        {
            DuplicateLayerCommand = new RelayCommand(DuplicateLayer);
            ApplyBlurCommand = new RelayCommand(ApplyBlur);
            ApplyGrayscaleCommand = new RelayCommand(ApplyGrayscale);
            ApplySepiaCommand = new RelayCommand(ApplySepia);
        }

        private void DuplicateLayer()
        {
            if (Layers.Any())
            {
                var lastLayer = Layers.Last();
                Layers.Add(lastLayer.Clone());
            }
        }

        private void ApplyBlur()
        {
            if (Layers.Any())
            {
                var layer = Layers.Last();
                layer.AppliedEffects.Add(new BlurEffect());
            }
        }

        private void ApplyGrayscale()
        {
            if (Layers.Any())
            {
                var layer = Layers.Last();
                layer.AppliedEffects.Add(new GrayscaleEffect());
            }
        }

        private void ApplySepia()
        {
            if (Layers.Any())
            {
                var layer = Layers.Last();
                layer.AppliedEffects.Add(new SepiaEffect());
            }
        }
    }
}
