using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlaneTicketWeb.Data;
using PlaneTicketWeb.Models;
using System.Security.Claims;

namespace PlaneTicketWeb.Controllers
{
    public class PurchaseController : Controller
    {

        private readonly ApplicationDbContext _db;

        public PurchaseController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }




        ////Seats GET
        //public IActionResult PurchaseForm(int? SeatId)
        //{
        //    if (SeatId == null || SeatId == 0)
        //    {
        //        return NotFound();
        //    }


        //    Ticket obj = new Ticket();
        //    obj.SeatId = SeatId.Value;



        //    return View(obj);
        //}



        //Seats GET
        [Authorize]
        public IActionResult PurchaseForm(Ticket? obj)
        {
            if (obj.SeatId == null || obj.SeatId == 0)
            {
                return NotFound();
            }




            return View(obj);
        }


        //Suggesstion POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PurchaseFormPOST(Ticket obj)
        {


            Seat objSeat = _db.Seats.Find(obj.SeatId);

            obj.FlightId = objSeat.FlightId;


            if(obj.SecondSeatId != null)
            {
                Seat objSeat2 = _db.Seats.Find(obj.SecondSeatId);

                obj.SecondFlightId = objSeat2.FlightId;
            }
            




            //try
            //{
            //    _db.Tickets.Add(obj);
            //    _db.Seats.Update(objSeat);
            //    _db.SaveChanges();


            //    return RedirectToAction("Checkout", "Purchase", obj);
            //}
            //catch (Exception ex)
            //{

            //    return RedirectToAction("Index", "User");
            //}


            return RedirectToAction("Checkout", "Purchase", obj);

        }



        //Seats GET
        [Authorize]
        public IActionResult Checkout(Ticket? obj)
        {
            if (obj == null)
            {
                return NotFound();
            }


            Seat objSeat = _db.Seats.Find(obj.SeatId);
            Flight objFlight = _db.Flights.Find(obj.FlightId);


            if(obj.SecondSeatId != null)
            {
                Seat objSeat2 = _db.Seats.Find(obj.SecondSeatId);
                Flight objFlight2 = _db.Flights.Find(obj.SecondFlightId);


                ViewBag.SecondFlightNo = objFlight2.FlightNo;
                ViewBag.SecondLocationFrom = objFlight2.LocationFrom;
                ViewBag.SecondLocationTo = objFlight2.LocationTo;
                ViewBag.SecondSeatName = objSeat2.SeatName;

                obj.isTwoWay = true;
            }



            ViewBag.FlightNo = objFlight.FlightNo;
            ViewBag.LocationFrom = objFlight.LocationFrom;
            ViewBag.LocationTo = objFlight.LocationTo;
            ViewBag.SeatName = objSeat.SeatName;


            
            return View(obj);
        }



        //Seats GET
        [Authorize]
        public IActionResult CheckoutPOST(Ticket? obj)
        {
            if (obj == null)
            {
                return NotFound();
            }


            if (obj.SecondFlightId != null)
            {
                obj.isTwoWay = true;
            }

            

            obj.PurchaseDate = DateTime.UtcNow;


            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] pnr = new char[6];
            for (int i = 0; i < 6; i++)
            {
                pnr[i] = characters[random.Next(0, characters.Length)];
            }
            string randomPNR = new string(pnr);
            obj.PNR = randomPNR;


            obj.AspNetUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);






            Seat objSeat = _db.Seats.Find(obj.SeatId);
            Flight objFlight = _db.Flights.Find(obj.FlightId);




            objSeat.isBooked = true;
            objSeat.BookedTime = DateTime.UtcNow;

            //if (obj.isTwoWay)
            //{
            //    Seat objSeat2 = _db.Seats.Find(obj.SecondSeatId);
            //    objSeat2.isBooked = true;
            //    objSeat2.BookedTime = DateTime.UtcNow;
            //}


            try
            {
                _db.Tickets.Add(obj);
                _db.Seats.Update(objSeat);
                if (obj.isTwoWay)
                {
                    Seat objSeat2 = _db.Seats.Find(obj.SecondSeatId);
                    objSeat2.isBooked = true;
                    objSeat2.BookedTime = DateTime.UtcNow;
                    _db.Seats.Update(objSeat2);
                }
                _db.SaveChanges();


                return RedirectToAction("ThankYou", "Purchase", obj);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "User");
            }


        }


        //GET
        [Authorize]
        public IActionResult ThankYou(Ticket? obj)
        {

            ViewBag.PNR = obj.PNR;

            return View();
        }

    }
}
