using Lumina.Core.Patterns;

namespace Lumina.Core.Effects
{
    public class SepiaEffect : IEffect, IPrototype<SepiaEffect>
    {
        public string Name => "Sepia";
        public double Amount { get; set; }
        public SepiaEffect() { Amount = 1.0; }

        public SepiaEffect(double amount)
        {
            Amount = amount;
        }

        public SepiaEffect Clone()
        {
            return new SepiaEffect(this.Amount);
        }
        public IEffect CloneEffect() => Clone();

    }
}
