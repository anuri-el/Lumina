using Lumina.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.UI.Interfaces
{
    public interface IImageApiClient
    {
        Task<ApiResponse<ImageDto>> UploadImageAsync(string filePath);
        Task<ApiResponse<List<ImageDto>>> GetAllImagesAsync();
        Task<ApiResponse<ImageDto>> GetImageByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteImageAsync(int id);
    }
}
