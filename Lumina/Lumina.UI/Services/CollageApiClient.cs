using Lumina.Shared.DTOs;
using Lumina.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.UI.Services
{
    public class CollageApiClient : ICollageApiClient
    {
        private readonly HttpClient _httpClient;

        public CollageApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<CollageDto>> CreateCollageAsync(string title, int width, int height)
        {
            try
            {
                var request = new CreateCollageRequest
                {
                    Title = title,
                    Width = width,
                    Height = height
                };

                var response = await _httpClient.PostAsJsonAsync("/api/collages", request);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<CollageDto>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<CollageDto>>()
                    ?? new ApiResponse<CollageDto> { Success = false, Message = "Failed to parse response" };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<CollageDto>
                {
                    Success = false,
                    Message = $"Connection error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CollageDto>
                {
                    Success = false,
                    Message = $"Error creating collage: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<CollageDto>>> GetAllCollagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/collages");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<CollageDto>>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<List<CollageDto>>>()
                    ?? new ApiResponse<List<CollageDto>> { Success = false, Message = "Failed to parse response" };
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<List<CollageDto>>
                {
                    Success = false,
                    Message = $"Connection error: {ex.Message}. Make sure server is running on http://localhost:5155"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CollageDto>>
                {
                    Success = false,
                    Message = $"Error getting collages: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<CollageDto>> GetCollageByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/collages/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<CollageDto>
                    {
                        Success = false,
                        Message = $"HTTP {response.StatusCode}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<ApiResponse<CollageDto>>()
                    ?? new ApiResponse<CollageDto> { Success = false, Message = "Failed to parse response" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CollageDto>
                {
                    Success = false,
                    Message = $"Error getting collage: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> AddLayerAsync(int collageId, int imageId, double x, double y, double width, double height)
        {
            try
            {
                var request = new AddLayerRequest
                {
                    CollageId = collageId,
                    ImageId = imageId,
                    X = x,
                    Y = y,
                    Width = width,
                    Height = height
                };

                var response = await _httpClient.PostAsJsonAsync("/api/collages/layer", request);

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
                    Message = $"Error adding layer: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteCollageAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/collages/{id}");

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
                    Message = $"Error deleting collage: {ex.Message}"
                };
            }
        }
    }
}
