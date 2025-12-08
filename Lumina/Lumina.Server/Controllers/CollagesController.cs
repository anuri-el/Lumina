using Lumina.Core.Interfaces;
using Lumina.Core.Models;
using Lumina.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lumina.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollagesController : ControllerBase
    {
        private readonly ICollageService _collageService;
        private readonly ILogger<CollagesController> _logger;

        public CollagesController(ICollageService collageService, ILogger<CollagesController> logger)
        {
            _collageService = collageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CollageDto>>> CreateCollage([FromBody] CreateCollageRequest request)
        {
            try
            {
                _logger.LogInformation("Creating collage: {Title}", request.Title);

                var collage = new Collage
                {
                    Title = request.Title,
                    Width = request.Width,
                    Height = request.Height
                };

                var savedCollage = await _collageService.AddAsync(collage);

                var dto = new CollageDto
                {
                    Id = savedCollage.Id,
                    Title = savedCollage.Title,
                    Width = savedCollage.Width,
                    Height = savedCollage.Height
                };

                return Ok(new ApiResponse<CollageDto>
                {
                    Success = true,
                    Message = "Collage created successfully",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating collage");
                return BadRequest(new ApiResponse<CollageDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CollageDto>>>> GetAllCollages()
        {
            try
            {
                var collages = await _collageService.GetAllAsync();
                var dtos = collages.Select(c => new CollageDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Width = c.Width,
                    Height = c.Height,
                    Layers = c.Layers.Select(l => new ImageLayerDto
                    {
                        Id = l.Id,
                        Name = l.Name,
                        ImageId = l.Image.Id,
                        X = l.X,
                        Y = l.Y,
                        Width = l.Width,
                        Height = l.Height,
                        Rotation = l.Rotation,
                        Opacity = l.Opacity
                    }).ToList()
                }).ToList();

                return Ok(new ApiResponse<List<CollageDto>>
                {
                    Success = true,
                    Data = dtos
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting collages");
                return BadRequest(new ApiResponse<List<CollageDto>>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CollageDto>>> GetCollageById(int id)
        {
            try
            {
                var collage = await _collageService.GetByIdAsync(id);
                if (collage == null)
                {
                    return NotFound(new ApiResponse<CollageDto>
                    {
                        Success = false,
                        Message = "Collage not found"
                    });
                }

                var dto = new CollageDto
                {
                    Id = collage.Id,
                    Title = collage.Title,
                    Width = collage.Width,
                    Height = collage.Height,
                    Layers = collage.Layers.Select(l => new ImageLayerDto
                    {
                        Id = l.Id,
                        Name = l.Name,
                        ImageId = l.Image.Id,
                        X = l.X,
                        Y = l.Y,
                        Width = l.Width,
                        Height = l.Height,
                        Rotation = l.Rotation,
                        Opacity = l.Opacity
                    }).ToList()
                };

                return Ok(new ApiResponse<CollageDto>
                {
                    Success = true,
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting collage {Id}", id);
                return BadRequest(new ApiResponse<CollageDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("layer")]
        public async Task<ActionResult<ApiResponse<bool>>> AddLayer([FromBody] AddLayerRequest request)
        {
            try
            {
                await _collageService.AddImageToCollageAsync(
                    request.CollageId,
                    request.ImageId,
                    request.X,
                    request.Y);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Layer added successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding layer");
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCollage(int id)
        {
            try
            {
                await _collageService.DeleteAsync(id);
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Collage deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting collage {Id}", id);
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
