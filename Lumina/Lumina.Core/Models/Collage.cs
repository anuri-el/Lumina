namespace Lumina.Core.Models
{
    public class Collage
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CollageImage>? CollageImages { get; set; }
    }
}
