namespace Lumina.Core.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CollageImage>? CollageImages { get; set; }
    }
}
