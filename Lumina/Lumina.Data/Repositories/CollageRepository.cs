using Lumina.Data.Entities;
using Lumina.Data.Interfaces;

namespace Lumina.Data.Repositories
{
    public class CollageRepository : Repository<CollageEntity>, ICollageRepository
    {
        public CollageRepository(LuminaContext context) : base(context) { }
    }
}
