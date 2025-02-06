using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ContactDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string DestinationCardNumber { get; set; }
}
