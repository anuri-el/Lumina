using Lumina.Core.Models;
using Lumina.Data.Entities;

namespace Lumina.Data.Mappers
{
    public static class ImageMapper
    {
        public static Image ToModel(this ImageEntity entity) => new()
        {
            Id = entity.Id,
            FilePath = entity.FilePath,
            Format = entity.Format,
            CreatedAt = entity.CreatedAt
        };

        public static ImageEntity ToEntity(this Image model) => new()
        {
            Id = model.Id,
            FilePath = model.FilePath,
            Format = model.Format,
            CreatedAt = model.CreatedAt,
        };
    }
}
