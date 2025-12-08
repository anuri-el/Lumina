using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.DTOs
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
