using System.ComponentModel.DataAnnotations;

public class ContactDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(11)]
    public string Phone { get; set; }

    [MaxLength(100)]
    public string Mail { get; set; }
}
