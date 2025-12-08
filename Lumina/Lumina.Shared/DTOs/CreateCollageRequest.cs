using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.DTOs
{
    public class CreateCollageRequest
    {
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
