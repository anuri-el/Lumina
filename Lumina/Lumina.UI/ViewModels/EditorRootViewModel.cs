namespace Lumina.UI.ViewModels
{
    public class EditorRootViewModel
    {
        public EditorViewModel Tools { get; }
        public EditorPageViewModel Layers { get; }

        public EditorRootViewModel()
        {
            Tools = new EditorViewModel();
            Layers = new EditorPageViewModel();
        }
    }
}
