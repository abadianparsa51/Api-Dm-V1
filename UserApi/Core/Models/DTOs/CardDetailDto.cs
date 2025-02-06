namespace UserApi.Core.Models.DTOs
{
    public class CardDetailDTO
    {
        public int Id { get; set; }

        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV2 { get; set; }

    }
}
