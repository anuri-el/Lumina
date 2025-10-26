namespace Lumina.Core.Models
{
    public class ImageLayer
    {
        public int Id { get; set; }
        public Image Image { get; set; } = null!;
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; } = 0;
        public double Opacity { get; set; } = 1;
    }
}
