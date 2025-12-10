using Lumina.Shared.DTOs;
using Lumina.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.UI.Services
{
    public class ImageApiClient : IImageApiClient
    {
        private readonly HttpClient _httpClient;

        public ImageApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<ImageDto>> UploadImageAsync(string filePath)
        {
            try
            {
                // Читаємо файл і конвертуємо в Base64
                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                string base64Data = Convert.ToBase64String(imageBytes);

                var request = new UploadImageRequest
                {
                    FileName = Path.GetFileName(filePath),
                    Base64Data = base64Data
                };

                var response = await _httpClient.PostAsJsonAsync("/api/images/upload", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<ImageDto>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}: {errorContent}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<ImageDto>>()
                    ?? new ApiResponse<ImageDto> { Success = false, Message = "Failed to parse response" };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Connection error: {ex.Message}. Make sure server is running on http://localhost:5155"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Error uploading image: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<ImageDto>>> GetAllImagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/images");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<List<ImageDto>>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}: {errorContent}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<List<ImageDto>>>()
                    ?? new ApiResponse<List<ImageDto>> { Success = false, Message = "Failed to parse response" };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<List<ImageDto>>
                {
                    Success = false,
                    Message = $"Connection error: {ex.Message}. Make sure server is running on http://localhost:5155"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ImageDto>>
                {
                    Success = false,
                    Message = $"Error getting images: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<ImageDto>> GetImageByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/images/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<ImageDto>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<ImageDto>>()
                    ?? new ApiResponse<ImageDto> { Success = false, Message = "Failed to parse response" };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Connection error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Error getting image: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteImageAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/images/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
                    ?? new ApiResponse<bool> { Success = false, Message = "Failed to parse response" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error deleting image: {ex.Message}"
                };
            }
        }
    }
}
