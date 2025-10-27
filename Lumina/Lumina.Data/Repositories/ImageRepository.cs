using Lumina.Data.Entities;
using Lumina.Data.Interfaces;

namespace Lumina.Data.Repositories
{
    public class ImageRepository : Repository<ImageEntity>, IImageRepository
    {
        public ImageRepository(LuminaContext context) : base(context) { }
    }
}
