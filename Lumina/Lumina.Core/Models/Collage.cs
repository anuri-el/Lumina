using Lumina.Core.Patterns;

namespace Lumina.Core.Models
{
    public class Collage : IPrototype<Collage>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public List<ImageLayer> Layers { get; set; } = new();

        public void AddLayer(ImageLayer layer)
        {
            Layers.Add(layer);
        }

        public Collage Clone()
        {
            var clone = new Collage
            {
                Title = this.Title + " (Copy)",
                Width = this.Width,
                Height = this.Height,
                Layers = new List<ImageLayer>()
            };

            foreach (var layer in Layers)
                clone.Layers.Add(layer.Clone());

            return clone;
        }
    }
}
