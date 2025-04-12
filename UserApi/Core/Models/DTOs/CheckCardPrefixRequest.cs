using System.ComponentModel.DataAnnotations;

namespace UserApi.Core.Models.DTOs
{
    public class CheckCardPrefixRequest
    {
        [Required]
        [MinLength(6)]
        public string Prefix { get; set; } = string.Empty;
    }
}
