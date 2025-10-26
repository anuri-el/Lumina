using Lumina.Core.Models;

namespace Lumina.Core.Interfaces
{
    public interface IEffectService : IService<Effect>
    {
        Task ApplyEffectAsync(int imageId, string effectName, string? parameters = null);
    }
}
