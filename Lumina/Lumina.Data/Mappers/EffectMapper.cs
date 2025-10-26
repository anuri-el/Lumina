using Lumina.Core.Models;
using Lumina.Data.Entities;

namespace Lumina.Data.Mappers
{
    public static class EffectMapper
    {
        public static Effect ToModel(this EffectEntity entity) => new()
        {
            Id = entity.Id,
            EffectName = entity.EffectName,
            Parameters = entity.Parameters,
        };

        public static EffectEntity ToEntity(this Effect model) => new()
        {
            Id = model.Id,
            EffectName = model.EffectName,
            Parameters = model.Parameters,
        };
    }
}
