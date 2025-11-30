namespace Lumina.Core.Patterns
{
    public class LayerGroup : IImageComponent
    {
        public string Name { get; set; }
        private readonly List<IImageComponent> _children = new();

        public LayerGroup(string name)
        {
            Name = name;
        }

        public void Add(IImageComponent component)
        {
            _children.Add(component);
        }

        public void Remove(IImageComponent component)
        {
            _children.Remove(component);
        }

        public async Task RenderAsync()
        {
            Console.WriteLine($"Rendering group '{Name}' with {_children.Count} child components...");
            foreach (var child in _children)
                await child.RenderAsync();
        }

        public async Task ApplyEffectAsync(string effectName, string? parameters = null)
        {
            Console.WriteLine($"Applying effect '{effectName}' to group '{Name}'");
            foreach (var child in _children)
                await child.ApplyEffectAsync(effectName, parameters);
        }
    }
}
