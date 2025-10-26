using Lumina.Core.Models;
using Lumina.Data.Entities;

namespace Lumina.Data.Mappers
{
    public static class ImageLayerMapper
    {
        public static ImageLayer ToModel(this ImageLayerEntity entity) => new()
        {
            Id = entity.Id,
            Image = entity.Image.ToModel(),
            X = entity.X,
            Y = entity.Y,
            Width = entity.Width,
            Height = entity.Height,
            Rotation = entity.Rotation,
            Opacity = entity.Opacity,
        };

        public static ImageLayerEntity ToEntity(this ImageLayer model, int collageId) => new()
        {
            Id = model.Id,
            ImageId = model.Image.Id,
            X = model.X,
            Y = model.Y,
            Width = model.Width,
            Height = model.Height,
            Rotation = model.Rotation,
            Opacity = model.Opacity,
            CollageId = collageId,
        };
    }
}
