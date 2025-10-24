namespace Lumina.Core.Models
{
    public class CollageImage
    {
        public int CollageId { get; set; }
        public Collage? Collage { get; set; } = null;

        public int ImageId { get; set; }
        public Image? Image { get; set; } = null;
    }
}