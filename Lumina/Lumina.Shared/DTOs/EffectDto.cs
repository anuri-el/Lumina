using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.DTOs
{
    public class EffectDto
    {
        public int Id { get; set; }
        public string EffectName { get; set; } = string.Empty;
        public string? Parameters { get; set; }
    }
}
