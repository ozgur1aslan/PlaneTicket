using System.ComponentModel.DataAnnotations;

namespace PlaneTicketWeb.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
