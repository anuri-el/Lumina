namespace Lumina.Data.Entities
{
    public class CollageImageEntity
    {
        public int CollageId { get; set; }
        public CollageEntity? Collage { get; set; }

        public int ImageId { get; set; }
        public ImageEntity? Image { get; set; }
    }
}
