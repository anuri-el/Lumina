using Lumina.Core.Models;

namespace Lumina.Core.Patterns
{
    public class ImageLeaf : IImageComponent
    {
        public string Name { get; set; }
        public ImageLayer Layer { get; set; }

        public ImageLeaf(string name, ImageLayer layer)
        {
            Name = name;
            Layer = layer;
        }

        public Task RenderAsync()
        {
            Console.WriteLine($"Rendering layer '{Name}' at ({Layer.X},{Layer.Y}) size {Layer.Width}x{Layer.Height}");
            return Task.CompletedTask;
        }

        public Task ApplyEffectAsync(string effectName, string? parameters = null)
        {
            Console.WriteLine($"Applying effect '{effectName}' to layer '{Name}'");
            return Task.CompletedTask;
        }
    }
}
