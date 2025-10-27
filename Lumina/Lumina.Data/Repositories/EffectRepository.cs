using Lumina.Data.Entities;
using Lumina.Data.Interfaces;

namespace Lumina.Data.Repositories
{
    public class EffectRepository : Repository<EffectEntity>, IEffectRepository
    {
        public EffectRepository(LuminaContext context) : base(context) { }
    }
}
