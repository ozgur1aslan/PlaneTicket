using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicketWeb.Models
{
	public class City
	{
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")]

        public virtual Country Country { get; set; }
    }
}
