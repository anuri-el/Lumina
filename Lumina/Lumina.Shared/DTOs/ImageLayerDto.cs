using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Shared.DTOs
{
    public class ImageLayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Layer";
        public int ImageId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }
        public double Opacity { get; set; }
        public List<EffectDto> AppliedEffects { get; set; } = new();
    }
}
