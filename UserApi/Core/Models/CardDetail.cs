using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Core.Models
{
    public class CardDetail
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(16)")]
        public string CardNumber { get; set; } = "";

        [MaxLength(10)]
        public string ExpirationDate { get; set; } = "";

        [MaxLength(4)]
        public string CVV2 { get; set; } = "";

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [MaxLength]
        //public decimal Balance { get; set; } // موجودی حساب

        // Foreign key for Bank
        public int BankId { get; set; }
        public Bank Bank { get; set; }


    [NotMapped]
    public string BankName => Bank?.Name; // مقدار BankName را از Bank می‌گیریم


    }
}
