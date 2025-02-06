using System.ComponentModel.DataAnnotations;

namespace UserApi.Core.Models.DTOs
{
    public class CardDetailDTO
    {

        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string ExpirationDate { get; set; }
        [Required]
        public string CVV2 { get; set; }

    }
}
