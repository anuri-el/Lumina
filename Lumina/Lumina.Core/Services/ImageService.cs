using Lumina.Core.Interfaces;
using Lumina.Core.Models;

namespace Lumina.Core.Services
{
    public class ImageService : ServiceBase<Image>, IImageService
    {
        public ImageService(IRepository<Image> repository)
            : base(repository) { }

        public async Task<IEnumerable<Image>> GetRecentImagesAsync(int count)
        {
            var all = await _repository.GetAllAsync();
            return all
                .OrderByDescending(i => i.CreatedAt)
                .Take(count)
                .ToList();
        }
    }
}
