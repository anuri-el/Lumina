namespace Lumina.Core.Patterns
{
    public interface IEffect
    {
        string Name { get; }
        IEffect CloneEffect();
    }
}
