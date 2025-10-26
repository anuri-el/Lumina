namespace Lumina.Data.Entities
{
    public class EffectEntity
    {
        public int Id { get; set; }
        public string EffectName { get; set; } = string.Empty;
        public string? Parameters { get; set; }
    }
}
