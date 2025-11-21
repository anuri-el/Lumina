using Lumina.Core.Patterns;

namespace Lumina.Core.Effects
{
    public class BlurEffect : IEffect, IPrototype<BlurEffect>
    {
        public string Name => "Blur";
        public int Radius { get; set; }

        public BlurEffect()
        {
            Radius = 5;
        }

        public BlurEffect(int radius)
        {
            Radius = radius;
        }

        public BlurEffect Clone()
        {
            return new BlurEffect(this.Radius);
        }
        public IEffect CloneEffect() => Clone();

    }
}
