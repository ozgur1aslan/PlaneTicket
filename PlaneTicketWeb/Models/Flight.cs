using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicketWeb.Models
{
	public class Flight
	{
        [Key]
        public int Id { get; set; }
        [Required]
        public int FlightNo { get; set; }

        [Required]
        public string LocationFrom { get; set; }
        [Required]
        public string LocationTo { get; set; }
        [Required]
        public DateOnly DepartureDate { get; set; }
        [Required]
        public TimeOnly DepartureTime { get; set; }
        //public DateOnly? ArrivalDate { get; set; }
        //public TimeOnly? ArrivalTime { get; set; }


    }
}
