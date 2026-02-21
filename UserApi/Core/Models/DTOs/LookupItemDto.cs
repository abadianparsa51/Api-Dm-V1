namespace UserApi.Core.Models.DTOs
{
    public class LookupItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Code { get; set; } // optional (e.g. Symbol)
    }
}
