using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Core.Models
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(20)]
        public string SwiftCode { get; set; } = "";
        [NotMapped]
        public string BankName => Name; // مقدار BankName را از Name می‌گیریم

        // Navigation property
        public ICollection<CardDetail> CardDetails { get; set; }
        public ICollection<CardPrefix> CardPrefixes { get; set; }

    }
}
