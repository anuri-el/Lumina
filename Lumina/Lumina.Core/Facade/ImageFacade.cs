using Lumina.Core.Interfaces;
using Lumina.Core.Models;

namespace Lumina.Core.Facade
{
    public class ImageFacade
    {
        private readonly IImageService _imageService;
        private readonly ICollageService _collageService;
        private readonly IEffectService _effectService;

        public ImageFacade(IImageService imageService,
                           ICollageService collageService,
                           IEffectService effectService)
        {
            _imageService = imageService;
            _collageService = collageService;
            _effectService = effectService;
        }

        public async Task<Image> OpenImageAsync(string filePath)
        {
            var image = new Image
            {
                FilePath = filePath,
                Format = Path.GetExtension(filePath).TrimStart('.')
            };

            return await _imageService.AddAsync(image);
        }

        public async Task<IEnumerable<Image>> GetRecentImagesAsync(int count)
            => await _imageService.GetRecentImagesAsync(count);

        public async Task<Collage> CreateCollageAsync(string title, int width, int height)
        {
            var collage = new Collage { Title = title, Width = width, Height = height };
            return await _collageService.AddAsync(collage);
        }

        public async Task AddImageToCollageAsync(int collageId, int imageId, double x, double y)
            => await _collageService.AddImageToCollageAsync(collageId, imageId, x, y);

        public async Task DuplicateLayerAsync(int collageId, int layerId)
            => await _collageService.DuplicateLayer(collageId, layerId);

        public async Task ApplyEffectToImageAsync(int imageId, string effectName, string? parameters = null)
            => await _effectService.ApplyEffectAsync(imageId, effectName, parameters);
    }
}
