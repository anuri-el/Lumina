using Lumina.Core.Interfaces;
using Lumina.Core.Models;

namespace Lumina.Core.Services
{
    public class CollageService : ServiceBase<Collage>, ICollageService
    {
        private readonly IRepository<Image> _imageRepository;

        public CollageService(IRepository<Collage> repository, IRepository<Image> imageRepository)
            : base(repository)
        {
            _imageRepository = imageRepository;
        }

        public async Task AddImageToCollageAsync(int collageId, int imageId, double x, double y)
        {
            var collage = await _repository.GetByIdAsync(collageId);
            var image = await _imageRepository.GetByIdAsync(imageId);

            if (collage == null || image == null)
                throw new InvalidOperationException("Collage or image not found.");

            collage.Layers.Add(new ImageLayer
            {
                Image = image,
                X = x,
                Y = y,
                Width = image.Width,
                Height = image.Height
            });

            await _repository.UpdateAsync(collage);
        }

        public async Task RemoveImageFromCollageAsync(int collageId, int imageId)
        {
            var collage = await _repository.GetByIdAsync(collageId);
            if (collage == null)
                throw new InvalidOperationException("Collage not found.");

            var layer = collage.Layers.FirstOrDefault(l => l.Image.Id == imageId);
            if (layer != null)
            {
                collage.Layers.Remove(layer);
                await _repository.UpdateAsync(collage);
            }
        }
        public async Task DuplicateLayer(int collageId, int layerId)
        {
            var collage = await GetByIdAsync(collageId);
            if (collage == null) return;

            var layer = collage.Layers.FirstOrDefault(l => l.Id == layerId);
            if (layer == null) return;

            var clone = layer.Clone();
            clone.X += 20;
            clone.Y += 20;

            collage.Layers.Add(clone);

            await UpdateAsync(collage);
        }
    }
}
