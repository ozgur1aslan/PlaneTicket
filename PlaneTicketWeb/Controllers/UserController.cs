using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlaneTicketWeb.Data;
using PlaneTicketWeb.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace PlaneTicketWeb.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }



        ///////////////////////////////////////////////////////////////////////////=====>>>>> FLIGHT ACTIONS

        //GET
        [Authorize]
        public IActionResult SearchFlight()
        {


            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");


            return View("SearchFlight/SearchFlight");
        }


        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchFlight(Flight obj)
        {

            if (obj.LocationFrom == obj.LocationTo)
            {
                ModelState.AddModelError("CustomError", "The departure point and destination cannot be the same.");
            }

            if (obj.DepartureDate < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure date can't be chosen from the past.");
            }

            if (obj.DepartureDate == DateOnly.FromDateTime(DateTime.Now) && obj.DepartureTime < TimeOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure time can't be chosen from the past.");
            }






            //DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            //DateOnly chosenDay = obj.DepartureDate;
            //DateOnly minus7Days = today > obj.DepartureDate.AddDays(-7) ? today : obj.DepartureDate.AddDays(-7);
            //DateOnly plus7Days = obj.DepartureDate.AddDays(7);

            //IEnumerable<Flight> objFlightList = _db.Flights.Where(x => x.LocationFrom == obj.LocationFrom)
            //.Where(x => x.LocationTo == obj.LocationTo).Where(x => x.DepartureDate >= minus7Days && x.DepartureDate <= plus7Days);




            DateTime now = DateTime.Now;
            DateOnly today = DateOnly.FromDateTime(now);
            TimeOnly currentTime = TimeOnly.FromDateTime(now);
            DateOnly chosenDay = obj.DepartureDate;
            DateOnly minus7Days = today > obj.DepartureDate.AddDays(-7) ? today : obj.DepartureDate.AddDays(-7);
            DateOnly plus7Days = obj.DepartureDate.AddDays(7);

            //IEnumerable<Flight> objFlightList = _db.Flights
            //    .Where(x => x.LocationFrom == obj.LocationFrom)
            //    .Where(x => x.LocationTo == obj.LocationTo)
            //    .Where(x =>
            //        (x.DepartureDate >= minus7Days || (x.DepartureDate == minus7Days && x.DepartureTime > currentTime)) &&
            //        (x.DepartureDate <= plus7Days)
            //    );


            IEnumerable<Flight> objFlightList = _db.Flights
                .Where(x => x.LocationFrom == obj.LocationFrom)
                .Where(x => x.LocationTo == obj.LocationTo)
                .Where(x =>
                    (x.DepartureDate > minus7Days || (x.DepartureDate == minus7Days && x.DepartureTime >= currentTime)) &&
                    (x.DepartureDate <= plus7Days)
                );


            IEnumerable<Flight> filteredOnlyChosenDate = objFlightList.Where(x => x.DepartureDate == obj.DepartureDate);

            //IEnumerable<Flight> filteredInRange14Days = objFlightList.Where(x => x.DepartureDate != obj.DepartureDate);


            

            List<Flight> FilteredWithoutChosen = new List<Flight>();

            foreach (var flight in objFlightList)
            {
                if (flight.DepartureDate != obj.DepartureDate)
                {
                    FilteredWithoutChosen.Add(flight);
                }
            }

            IEnumerable<Flight> filteredInRange14Days = FilteredWithoutChosen;







            if (ModelState.IsValid)
            {
                //_db.Flights.Add(obj);
                //_db.SaveChanges();


                if (filteredOnlyChosenDate.Count() == 0)
                {
                    string jsonData = JsonSerializer.Serialize(filteredInRange14Days);
                    TempData["FilteredData"] = jsonData;
                } else
                {
                    string jsonData = JsonSerializer.Serialize(filteredOnlyChosenDate);
                    TempData["FilteredData"] = jsonData;
                }




                




                return RedirectToAction("FilteredFlightList");
            }
            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");
            return View("SearchFlight/SearchFlight", obj);
        }




        //FilteredFlightList GET
        [Authorize]
        public IActionResult FilteredFlightList(int? id)
        {
            {

                string jsonData = TempData["FilteredData"] as string;

                if (string.IsNullOrEmpty(jsonData))
                {
                    return View("FilteredFlightList");
                }

                IEnumerable<Flight> filtered = JsonSerializer.Deserialize<IEnumerable<Flight>>(jsonData);


                if(filtered != null && filtered is IEnumerable<Flight>) {
                    return View("SearchFlight/FilteredFlightList", filtered);
                }



                return RedirectToAction("SearchFlight");

            }
        }


        //Seats GET
        [Authorize]
        public IActionResult CheckSeats(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            IEnumerable<Seat> objSeatList = _db.Seats.Where(x => x.FlightId == id);
            if (objSeatList == null)
            {
                return NotFound();
            }

            return View("SearchFlight/CheckSeats", objSeatList);
        }





        ///////////////////////////////////////////////////////////////////////////=====>>>>> RETURN FLIGHT ACTIONS


        [Authorize]
        public IActionResult ReturnTicket(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }


            Ticket obj = new Ticket();
            obj.SeatId = id.Value; ;



            return View("ReturnFlight/ReturnTicket", obj);
        }



        [Authorize]
        public IActionResult ReturnFlightResults(Ticket obj)
        {
            {
                Seat refSeat = _db.Seats.Find(obj.SeatId);
                Flight refFlight = _db.Flights.Find(refSeat.FlightId);



                //DateTime now = DateTime.Now;
                //DateOnly today = DateOnly.FromDateTime(now);
                //TimeOnly currentTime = TimeOnly.FromDateTime(now);
                //DateOnly chosenDay = refFlight.DepartureDate;
                //DateOnly minus7Days = today > refFlight.DepartureDate.AddDays(-7) ? today : refFlight.DepartureDate.AddDays(-7);
                //DateOnly plus7Days = refFlight.DepartureDate.AddDays(7);

                //IEnumerable<Flight> filteredForReturn = _db.Flights
                //    .Where(x => x.LocationFrom == refFlight.LocationTo)
                //    .Where(x => x.LocationTo == refFlight.LocationFrom)
                //    .Where(x =>
                //        (x.DepartureDate > minus7Days || (x.DepartureDate == minus7Days && x.DepartureTime > currentTime)) &&
                //        (x.DepartureDate < plus7Days)
                //    );


                DateTime now = DateTime.Now;
                DateOnly today = DateOnly.FromDateTime(now);
                TimeOnly currentTime = TimeOnly.FromDateTime(now);
                DateOnly chosenDay = refFlight.DepartureDate;
                DateOnly minus7Days = today > refFlight.DepartureDate.AddDays(-7) ? today : refFlight.DepartureDate.AddDays(-7);
                DateOnly plus7Days = refFlight.DepartureDate.AddDays(7);

                IEnumerable<Flight> filteredForReturn = _db.Flights
                    .Where(x => x.LocationFrom == refFlight.LocationTo)
                    .Where(x => x.LocationTo == refFlight.LocationFrom)
                    .Where(x =>
                        (x.DepartureDate > chosenDay || (x.DepartureDate == chosenDay && x.DepartureTime > refFlight.DepartureTime)) &&
                        (x.DepartureDate <= plus7Days)
                    );





                    //IEnumerable<Flight> filteredForReturn = _db.Flights.Where(x => x.LocationFrom == refFlight.LocationTo).Where(x => x.LocationTo == refFlight.LocationFrom);


                if (filteredForReturn != null && filteredForReturn is IEnumerable<Flight>)
                {

                    ViewBag.SeatId = obj.SeatId;
                    return View("ReturnFlight/ReturnFlightResults", filteredForReturn);
                }



                return RedirectToAction("SearchFlight");

            }

        }


        //Seats GET
        [Authorize]
        public IActionResult ReturnCheckSeats(Ticket? obj)
        {
            if (obj.SecondFlightId == null || obj.SecondFlightId == 0)
            {
                return NotFound();
            }


            IEnumerable<Seat> objSeatList = _db.Seats.Where(x => x.FlightId == obj.SecondFlightId);
            if (objSeatList == null)
            {
                return NotFound();
            }

            ViewBag.SeatId = obj.SeatId;
            //ViewBag.SecondFlightId = obj.SecondFlightId;

            return View("ReturnFlight/ReturnCheckSeats", objSeatList);
        }




        ///////////////////////////////////////////////////////////////////////////=====>>>>> PNR AND PURCHASED TICKETS SEARCH



        //GET
        [Authorize]
        public IActionResult SearchPNR()
        {
            return View("PNR/SearchPNR");
        }


        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchPNR(Ticket? obj)
        {
            //Ticket resultObj = _db.Tickets.Where(x => x.PNR == obj.PNR);

            Ticket resultObj = _db.Tickets.FirstOrDefault(ticket => ticket.PNR == obj.PNR);
            Seat seatObj = _db.Seats.Find(resultObj.SeatId);
            Flight flightObj = _db.Flights.Find(resultObj.FlightId);

            ViewBag.SeatName = seatObj.SeatName;
            ViewBag.FlightNo = flightObj.FlightNo;
            ViewBag.LocationFrom = flightObj.LocationFrom;
            ViewBag.LocationTo = flightObj.LocationTo;


            if(resultObj.isTwoWay)
            {
                Seat seatObj2 = _db.Seats.Find(resultObj.SecondSeatId);
                Flight flightObj2 = _db.Flights.Find(resultObj.SecondFlightId);

                ViewBag.SecondSeatName = seatObj2.SeatName;
                ViewBag.SecondFlightNo = flightObj2.FlightNo;
                ViewBag.SecondLocationFrom = flightObj2.LocationFrom;
                ViewBag.SecondLocationTo = flightObj2.LocationTo;
            }


            return View("PNR/ResultPNR", resultObj);
        }


    }
}
