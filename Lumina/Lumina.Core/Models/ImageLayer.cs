using Lumina.Core.Patterns;

namespace Lumina.Core.Models
{
    public class ImageLayer : IPrototype<ImageLayer>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Layer";
        public Image Image { get; set; } = null!;
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }
        public double Opacity { get; set; }
        public List<IEffect> AppliedEffects { get; set; } = new();
        public ImageLayer() { }
        public ImageLayer(string name) { }

        public ImageLayer(Image image)
        {
            Image = image;
            Width = 300;
            Height = 300;
        }

        public ImageLayer Clone()
        {
            return new ImageLayer
            {
                Name = this.Name + " Copy",
                Image = this.Image,
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Rotation = this.Rotation,
                Opacity = this.Opacity,

                AppliedEffects = this.AppliedEffects
                .Select(e => e.CloneEffect())
                .ToList()
            };
        }
    }
}
