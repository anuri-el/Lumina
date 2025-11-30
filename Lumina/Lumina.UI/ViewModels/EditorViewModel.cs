using Lumina.Core.Facade;
using Lumina.Core.Models;
using Lumina.Core.Patterns;
using Lumina.UI.Commands;
using System.Collections.ObjectModel;
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
        public IImageComponent RootComponent { get; private set; }

        public ObservableCollection<IImageComponent> Layers { get; set; } = new();

        public ICommand AddLayerCommand { get; }
        public ICommand AddGroupCommand { get; }
        public ICommand ApplyBlurCommand { get; }
        public ICommand DuplicateLayerCommand { get; }


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

            RootComponent = new LayerGroup("Root");

            AddLayerCommand = new RelayCommand(AddLayer);
            AddGroupCommand = new RelayCommand(AddGroup);
            ApplyBlurCommand = new RelayCommand(ApplyBlur);
            DuplicateLayerCommand = new RelayCommand(DuplicateSelectedLayer);
        }

        private void AddLayer()
        {
            var newLayer = new ImageLeaf($"Layer {Layers.Count + 1}", new ImageLayer { Width = 100, Height = 100 });
            (RootComponent as LayerGroup)?.Add(newLayer);
            Layers.Add(newLayer);
        }

        private void AddGroup()
        {
            var group = new LayerGroup($"Group {Layers.Count + 1}");
            (RootComponent as LayerGroup)?.Add(group);
            Layers.Add(group);
        }

        private async void ApplyBlur()
        {
            foreach (var layer in Layers)
            {
                await layer.ApplyEffectAsync("Blur", "radius=5");
            }
        }

        private void DuplicateSelectedLayer()
        {
            // Приклад дублювання першого шару
            if (Layers.Count == 0) return;

            var first = Layers[0];
            if (first is ImageLeaf leaf)
            {
                var clone = new ImageLeaf($"{leaf.Name} Copy", leaf.Layer.Clone());
                (RootComponent as LayerGroup)?.Add(clone);
                Layers.Add(clone);
            }
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
