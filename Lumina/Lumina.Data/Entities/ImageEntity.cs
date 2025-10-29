namespace Lumina.Data.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<CollageImageEntity>? CollageImages { get; set; }
        public ICollection<ImageLayerEntity>? Layers { get; set; }
    }
}
