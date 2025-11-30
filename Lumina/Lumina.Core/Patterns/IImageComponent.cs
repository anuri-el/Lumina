namespace Lumina.Core.Patterns
{
    public interface IImageComponent
    {
        string Name { get; set; }
        Task RenderAsync();
        Task ApplyEffectAsync(string effectName, string? parameters = null);
    }
}