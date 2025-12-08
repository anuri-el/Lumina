using Lumina.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Lumina.Shared.DTOs;
using Lumina.Core.Models;


namespace Lumina.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageService imageService, ILogger<ImagesController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<ImageDto>>> UploadImage([FromBody] UploadImageRequest request)
        {
            try
            {
                _logger.LogInformation("Uploading image: {FileName}", request.FileName);

                byte[] imageBytes = Convert.FromBase64String(request.Base64Data);
                string uploadPath = Path.Combine("uploads", request.FileName);
                Directory.CreateDirectory("uploads");
                await System.IO.File.WriteAllBytesAsync(uploadPath, imageBytes);

                var image = new Image
                {
                    FilePath = uploadPath,
                    Format = Path.GetExtension(request.FileName).TrimStart('.'),
                    Width = 0, // TODO: визначити розміри з imageBytes
                    Height = 0,
                    CreatedAt = DateTime.UtcNow
                };

                var savedImage = await _imageService.AddAsync(image);

                var dto = new ImageDto
                {
                    Id = savedImage.Id,
                    FilePath = savedImage.FilePath,
                    Format = savedImage.Format,
                    Width = savedImage.Width,
                    Height = savedImage.Height,
                    CreatedAt = savedImage.CreatedAt
                };

                return Ok(new ApiResponse<ImageDto>
                {
                    Success = true,
                    Message = "Image uploaded successfully",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return BadRequest(new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ImageDto>>>> GetAllImages()
        {
            try
            {
                var images = await _imageService.GetAllAsync();
                var dtos = images.Select(img => new ImageDto
                {
                    Id = img.Id,
                    FilePath = img.FilePath,
                    Format = img.Format,
                    Width = img.Width,
                    Height = img.Height,
                    CreatedAt = img.CreatedAt
                }).ToList();

                return Ok(new ApiResponse<List<ImageDto>>
                {
                    Success = true,
                    Data = dtos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting images");
                return BadRequest(new ApiResponse<List<ImageDto>>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ImageDto>>> GetImageById(int id)
        {
            try
            {
                var image = await _imageService.GetByIdAsync(id);
                if (image == null)
                {
                    return NotFound(new ApiResponse<ImageDto>
                    {
                        Success = false,
                        Message = "Image not found"
                    });
                }

                // Читаємо файл і конвертуємо в Base64
                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(image.FilePath);

                var dto = new ImageDto
                {
                    Id = image.Id,
                    FilePath = image.FilePath,
                    Format = image.Format,
                    Width = image.Width,
                    Height = image.Height,
                    CreatedAt = image.CreatedAt,
                    ImageData = imageBytes
                };

                return Ok(new ApiResponse<ImageDto>
                {
                    Success = true,
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting image {Id}", id);
                return BadRequest(new ApiResponse<ImageDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteImage(int id)
        {
            try
            {
                var image = await _imageService.GetByIdAsync(id);
                if (image == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Image not found"
                    });
                }

                // Видаляємо файл
                if (System.IO.File.Exists(image.FilePath))
                {
                    System.IO.File.Delete(image.FilePath);
                }

                await _imageService.DeleteAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Image deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {Id}", id);
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
