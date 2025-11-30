using Lumina.Core.Models;

namespace Lumina.Core.Interfaces
{
    public interface ICollageService : IService<Collage>
    {
        Task AddImageToCollageAsync(int collageId, int imageId, double x, double y);
        Task RemoveImageFromCollageAsync(int collageId, int imageId);
        Task DuplicateLayer(int collageId, int layerId);
    }
}
