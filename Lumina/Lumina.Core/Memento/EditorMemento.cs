namespace Lumina.Core.Memento
{
    public class EditorMemento
    {
        public string ImagePath { get; }
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public EditorMemento(string imagePath, double x, double y, double width, double height)
        {
            ImagePath = imagePath;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
