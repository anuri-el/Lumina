using Lumina.Core.Interfaces;
using Lumina.Core.Models;

namespace Lumina.Core.Services
{
    public class EffectService : ServiceBase<Effect>, IEffectService
    {
        private readonly IRepository<Image> _imageRepository;

        public EffectService(IRepository<Effect> repository, IRepository<Image> imageRepository)
            : base(repository)
        {
            _imageRepository = imageRepository;
        }

        public async Task ApplyEffectAsync(int imageId, string effectName, string? parameters = null)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);
            if (image == null)
                throw new InvalidOperationException("Image not found.");

            var effect = new Effect
            {
                EffectName = effectName,
                Parameters = parameters
            };

            await _repository.AddAsync(effect);
        }
    }
}
