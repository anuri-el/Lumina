using Lumina.Core.Patterns;

namespace Lumina.Core.Models
{
    public class Effect : IPrototype<Effect>
    {
        public int Id { get; set; }
        public string EffectName { get; set; } = string.Empty;
        public string? Parameters { get; set; }

        public Effect Clone()
        {
            return new Effect
            {
                EffectName = this.EffectName,
                Parameters = this.Parameters
            };
        }
    }
}
