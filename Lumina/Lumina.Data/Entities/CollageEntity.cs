namespace Lumina.Data.Entities
{
    public class CollageEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1080;

        public ICollection<ImageLayerEntity> Layers { get; set; } = new List<ImageLayerEntity>();
        public ICollection<CollageImageEntity>? CollageImages { get; set; }
    }
}
