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

        private string _currentImagePath = "";
        public string CurrentImagePath
        {
            get => _currentImagePath;
            set { _currentImagePath = value; OnPropertyChanged(); }
        }

        private Transform _currentTransform = new RotateTransform(0);
        public Transform CurrentTransform
        {
            get => _currentTransform;
            set { _currentTransform = value; OnPropertyChanged(); }
        }

        public ICommand RotateCommand { get; }
        public ICommand ScaleCommand { get; }
        public ICommand ApplyEffectCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand CropCommand { get; }
        public ICommand DrawCommand { get; }

        private IImageComponent _rootComponent;
        public IImageComponent RootComponent
        {
            get => _rootComponent;
            private set { _rootComponent = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IImageComponent> Layers { get; set; } = new();

        public ICommand AddLayerCommand { get; }
        public ICommand AddGroupCommand { get; }
        public ICommand ApplyBlurCommand { get; }
        public ICommand DuplicateLayerCommand { get; }

        public EditorViewModel()
        {
            Context = new EditorContext(new SelectState());
            Context.OnLog += AppendLog;

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
            AppendLog($"Added layer: {newLayer.Name}");
        }

        private void AddGroup()
        {
            var group = new LayerGroup($"Group {Layers.Count + 1}");
            (RootComponent as LayerGroup)?.Add(group);
            Layers.Add(group);
            AppendLog($"Added group: {group.Name}");
        }

        private async void ApplyBlur()
        {
            foreach (var layer in Layers)
            {
                await layer.ApplyEffectAsync("Blur", "radius=5");
            }
            AppendLog("Applied blur effect to all layers");
        }

        private void DuplicateSelectedLayer()
        {
            if (Layers.Count == 0)
            {
                AppendLog("No layers to duplicate");
                return;
            }

            var first = Layers[0];
            if (first is ImageLeaf leaf)
            {
                var clone = new ImageLeaf($"{leaf.Name} Copy", leaf.Layer.Clone());
                (RootComponent as LayerGroup)?.Add(clone);
                Layers.Add(clone);
                AppendLog($"Duplicated layer: {leaf.Name}");
            }
            else if (first is LayerGroup group)
            {
                AppendLog("Cannot duplicate groups yet");
            }
        }

        private void AppendLog(string message)
        {
            LogText += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void RotateImage(double angle)
        {
            var current = CurrentTransform as RotateTransform;
            if (current != null)
            {
                CurrentTransform = new RotateTransform(current.Angle + angle);
            }
            else
            {
                CurrentTransform = new RotateTransform(angle);
            }
            AppendLog($"Rotated by {angle} degrees");
        }

        private void ScaleImage(double factor)
        {
            CurrentTransform = new ScaleTransform(factor, factor);
            AppendLog($"Scaled by factor {factor}");
        }

        private void ApplyEffect()
        {
            AppendLog("Apply effect called - implement with IEffectService");
        }

        private void SaveImage()
        {
            AppendLog("Save image called - implement with IImageService");
        }
    }
}
