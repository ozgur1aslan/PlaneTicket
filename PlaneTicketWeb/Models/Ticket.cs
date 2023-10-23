using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PlaneTicketWeb.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(11)]
        public string TCKN { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        //[Required]
        //public string LocationFrom { get; set; }
        //[Required]
        //public string LocationTo { get; set; }
        //[Required]
        //public DateOnly DepartureDate { get; set; }
        //[Required]
        //public TimeOnly DepartureTime { get; set; }

        [Required]
        public bool isTwoWay { get; set; }

        [Required]
        [StringLength(14)]
        public string PNR { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Required]
        public int FlightId { get; set; }
        [ForeignKey("FlightId")]
        public virtual Flight Flight { get; set; }

        [Required]
        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }


        public int? SecondFlightId { get; set; } // New foreign key property
        [ForeignKey("SecondFlightId")]
        public virtual Flight SecondFlight { get; set; }

        public int? SecondSeatId { get; set; }
        [ForeignKey("SecondSeatId")]
        public virtual Seat SecondSeat { get; set; }


        public string AspNetUsersId { get; set; }
        [ForeignKey("AspNetUsersId")]
        public virtual AspNetUsers AspNetUsers { get; set; }


    }

    public class AspNetUsers : IdentityUser
    {
        //public virtual OrderModel OrderModel { get; set; }
    }
}
