namespace Lumina.Core.Models
{
    public class Collage
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1080;
        public List<ImageLayer> Layers { get; set; } = new();

        public void AddLayer(ImageLayer layer)
        {
            Layers.Add(layer);
        }
    }
}
