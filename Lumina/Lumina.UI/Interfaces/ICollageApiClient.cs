using Lumina.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.UI.Interfaces
{
    public interface ICollageApiClient
    {
        Task<ApiResponse<CollageDto>> CreateCollageAsync(string title, int width, int height);
        Task<ApiResponse<List<CollageDto>>> GetAllCollagesAsync();
        Task<ApiResponse<CollageDto>> GetCollageByIdAsync(int id);
        Task<ApiResponse<bool>> AddLayerAsync(int collageId, int imageId, double x, double y, double width, double height);
        Task<ApiResponse<bool>> DeleteCollageAsync(int id);
    }
}
