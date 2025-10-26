using Lumina.Core.Models;
using Lumina.Data.Entities;

namespace Lumina.Data.Mappers
{
    public static class CollageMapper
    {
        public static Collage ToModel(this CollageEntity entity) => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Width = entity.Width,
            Height = entity.Height,
            Layers = entity.Layers?.Select(l => l.ToModel()).ToList() ?? new(),
        };

        public static CollageEntity ToEntity(this Collage model) => new()
        {
            Id = model.Id,
            Title = model.Title,
            Width = model.Width,
            Height = model.Height,
            Layers = model.Layers.Select(l => l.ToEntity(model.Id)).ToList(),
        };
    }
}
