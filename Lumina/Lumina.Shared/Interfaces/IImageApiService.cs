using Lumina.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.Interfaces
{
    public interface IImageApiService
    {
        Task<ApiResponse<ImageDto>> UploadImageAsync(UploadImageRequest request);
        Task<ApiResponse<List<ImageDto>>> GetAllImagesAsync();
        Task<ApiResponse<ImageDto>> GetImageByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteImageAsync(int id);
    }
}
