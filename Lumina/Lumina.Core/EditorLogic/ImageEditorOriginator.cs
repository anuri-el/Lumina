using Lumina.Core.Memento;

namespace Lumina.Core.EditorLogic
{
    public class ImageEditorOriginator
    {
        public string CurrentImagePath { get; private set; } = "";
        public double PosX { get; private set; }
        public double PosY { get; private set; }
        public double Width { get; private set; } = 400;
        public double Height { get; private set; } = 300;

        public void SetState(string path, double x, double y, double width, double height)
        {
            CurrentImagePath = path;
            PosX = x;
            PosY = y;
            Width = width;
            Height = height;
        }

        public EditorMemento Save()
        {
            return new EditorMemento(CurrentImagePath, PosX, PosY, Width, Height);
        }

        public void Restore(EditorMemento memento)
        {
            CurrentImagePath = memento.ImagePath;
            PosX = memento.X;
            PosY = memento.Y;
            Width = memento.Width;
            Height = memento.Height;
        }
    }
}
