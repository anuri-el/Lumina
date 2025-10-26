using Lumina.Core.Models;

namespace Lumina.Core.Interfaces
{
    public interface IImageService : IService<Image>
    {
        Task<IEnumerable<Image>> GetRecentImagesAsync(int count);

    }
}
