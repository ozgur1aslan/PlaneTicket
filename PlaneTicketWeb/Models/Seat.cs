using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneTicketWeb.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SeatName { get; set; }

        public bool isBooked { get; set; }
        public DateTime BookedTime { get; set; }

        public int FlightId { get; set; }
        [ForeignKey("FlightId")]

        public virtual Flight Flight { get; set; }
    }
}
