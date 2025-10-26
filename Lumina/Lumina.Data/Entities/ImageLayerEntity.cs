namespace Lumina.Data.Entities
{
    public class ImageLayerEntity
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public ImageEntity Image { get; set; } = null!;

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; } = 0;
        public double Opacity { get; set; } = 1;

        public int CollageId { get; set; }
        public CollageEntity Collage { get; set; } = null!;
    }
}
