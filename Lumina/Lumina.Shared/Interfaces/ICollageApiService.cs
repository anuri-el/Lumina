using Lumina.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.Interfaces
{
    public interface ICollageApiService
    {
        Task<ApiResponse<CollageDto>> CreateCollageAsync(CreateCollageRequest request);
        Task<ApiResponse<List<CollageDto>>> GetAllCollagesAsync();
        Task<ApiResponse<CollageDto>> GetCollageByIdAsync(int id);
        Task<ApiResponse<bool>> AddLayerAsync(AddLayerRequest request);
        Task<ApiResponse<bool>> ApplyEffectAsync(ApplyEffectRequest request);
        Task<ApiResponse<bool>> DeleteCollageAsync(int id);
    }
}
