using Lumina.Core.Patterns;

namespace Lumina.Core.Effects
{
    public class GrayscaleEffect : IEffect, IPrototype<GrayscaleEffect>
    {
        public string Name => "Grayscale";
        public double Strength { get; set; }
        public GrayscaleEffect() { Strength = 1.0; }

        public GrayscaleEffect(double strength)
        {
            Strength = strength;
        }

        public GrayscaleEffect Clone()
        {
            return new GrayscaleEffect(this.Strength);
        }
        public IEffect CloneEffect() => Clone();

    }
}
