using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaneTicketWeb.Data;
using PlaneTicketWeb.Models;
using System.Data;
using System.Security.Claims;

namespace PlaneTicketWeb.Controllers
{
	public class AdminController : Controller
	{

        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }




        //INDEX

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
		{
			return View();
		}




        ///////////////////////////////////////////////////////////////////////////=====>>>>> COUNTRY ACTIONS

        [Authorize(Roles = "Admin")]
        public IActionResult CountryList()
        {

            IEnumerable<Country> objCountryList = _db.Countries;
            return View("Country/CountryList", objCountryList);
        }


        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCountry()
        {

            IEnumerable<Country> objCountryList = _db.Countries;
            return View("Country/CreateCountry");
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCountry(Country obj)
        {
            if (ModelState.IsValid)
            {
                obj.CreatedDateTime = DateTime.UtcNow;
                _db.Countries.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("CountryList");
            }
            return View("Country/CreateCountry", obj);

        }



        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult EditCountry(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var countryFromDb = _db.Countries.Find(id);
            if (countryFromDb == null)
            {
                return NotFound();
            }

            return View("Country/EditCountry", countryFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCountry(Country obj)
        {
            if (ModelState.IsValid)
            {
                _db.Countries.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("CountryList");
            }
            return View("Country/EditCountry", obj);

        }



        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCountry(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var countryFromDb = _db.Countries.Find(id);
            if (countryFromDb == null)
            {
                return NotFound();
            }

            return View("Country/DeleteCountry", countryFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCountryPOST(int? id)
        {

            var obj = _db.Countries.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Countries.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("CountryList");


        }



        ///////////////////////////////////////////////////////////////////////////=====>>>>> CITY ACTIONS

        [Authorize(Roles = "Admin")]
        public IActionResult CityList()
        {


            IEnumerable<City> objCityList = _db.Cities.Include(c => c.Country).ToList();

            
            return View("City/CityList", objCityList);
        }


        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCity()
        {

            IEnumerable<City> objCityList = _db.Cities;

            ViewBag.CountryList = new SelectList(_db.Countries, "Id", "Name");
            return View("City/CreateCity");
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCity(City obj)
        {

            _db.Cities.Add(obj);
            _db.SaveChanges();

            return RedirectToAction("CityList");


            //if (ModelState.IsValid)
            //{
            //    _db.Cities.Add(obj);
            //    _db.SaveChanges();

            //    return RedirectToAction("CityList");
            //}
            //ViewBag.CountryList = new SelectList(_db.Countries, "Id", "Name");
            //return View(obj);

        }



        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult EditCity(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cityFromDb = _db.Cities.Find(id);
            if (cityFromDb == null)
            {
                return NotFound();
            }

            ViewBag.CountryList = new SelectList(_db.Countries, "Id", "Name");

            return View("City/EditCity", cityFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCity(City obj)
        {

            _db.Cities.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("CityList");

            //if (ModelState.IsValid)
            //{
            //    _db.Cities.Update(obj);
            //    _db.SaveChanges();
            //    return RedirectToAction("CityList");
            //}
            //return View(obj);

        }



        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCity(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var cityFromDb = _db.Cities.Find(id);
            if (cityFromDb == null)
            {
                return NotFound();
            }

            return View("City/DeleteCity", cityFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCityPOST(int? id)
        {

            var obj = _db.Cities.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Cities.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("CityList");

            //return View(obj);

        }



        ///////////////////////////////////////////////////////////////////////////=====>>>>> FLIGHT ACTIONS

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult FlightList()
        {

            IEnumerable<Flight> objFlightList = _db.Flights;
            return View("Flight/FlightList", objFlightList);
        }




        //CreateFlight GET
        [Authorize(Roles = "Admin")]
        public IActionResult CreateFlight()
        {


            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");


            return View("Flight/CreateFlight");
        }


        //CreateFlight POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFlight(Flight obj)
        {

            if (obj.LocationFrom == obj.LocationTo)
            {
                ModelState.AddModelError("CustomError", "Departure point and the destination can't be same.");
            }

            if (obj.DepartureDate < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure date can't chosen from past.");
            }

            if (obj.DepartureDate == DateOnly.FromDateTime(DateTime.Now) && obj.DepartureTime < TimeOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure time can't chosen from past2.");
            }




            //IEnumerable<Flight> objFlightList = _db.Flights;
            //int count = objFlightList.Count();
            //if (count == 0)
            //{
            //    obj.FlightNo = 1;
            //} else
            //{
            //    obj.FlightNo = _db.Flights.Max(p => p.FlightNo) + 1;
            //}

            Random rnd = new Random();
            obj.FlightNo = rnd.Next(100000, 999999);


            //_db.Flights.Add(obj);
            //_db.SaveChanges();

            //return RedirectToAction("FlightList", "Admin");

            if (ModelState.IsValid)
            {
                _db.Flights.Add(obj);
                _db.SaveChanges();



                for (int j = 1; j < 21; j++)
                {
                    Seat dummy = new Seat();
                    dummy.SeatName = j + "A";
                    dummy.isBooked = false;
                    dummy.FlightId = obj.Id;
                    _db.Seats.Add(dummy);
                    _db.SaveChanges();

                }

                for (int j = 1; j < 21; j++)
                {
                    Seat dummy = new Seat();
                    dummy.SeatName = j + "B";
                    dummy.isBooked = false;
                    dummy.FlightId = obj.Id;
                    _db.Seats.Add(dummy);
                    _db.SaveChanges();

                }

                for (int j = 1; j < 21; j++)
                {
                    Seat dummy = new Seat();
                    dummy.SeatName = j + "C";
                    dummy.isBooked = false;
                    dummy.FlightId = obj.Id;
                    _db.Seats.Add(dummy);
                    _db.SaveChanges();

                }

                for (int j = 1; j < 21; j++)
                {
                    Seat dummy = new Seat();
                    dummy.SeatName = j + "D";
                    dummy.isBooked = false;
                    dummy.FlightId = obj.Id;
                    _db.Seats.Add(dummy);
                    _db.SaveChanges();

                }




                return RedirectToAction("FlightList");
            }
            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");
            return View("Flight/CreateFlight", obj);
        }





        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult EditFlight(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var flightFromDb = _db.Flights.Find(id);
            if (flightFromDb == null)
            {
                return NotFound();
            }

            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");

            return View("Flight/EditFlight", flightFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFlight(Flight obj)
        {

            if (obj.LocationFrom == obj.LocationTo)
            {
                ModelState.AddModelError("CustomError", "Departure point and the destination can't be same.");
            }

            if (obj.DepartureDate < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure date can't chosen from past.");
            }

            if (obj.DepartureDate == DateOnly.FromDateTime(DateTime.Now) && obj.DepartureTime < TimeOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("CustomError", "Departure time can't chosen from past.");
            }


            if (ModelState.IsValid)
            {
                _db.Flights.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("FlightList");
            }

            ViewBag.CityList = new SelectList(_db.Cities, "Name", "Name");

            return View("Flight/EditFlight", obj);

        }






        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFlight(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var flightFromDb = _db.Flights.Find(id);
            if (flightFromDb == null)
            {
                return NotFound();
            }

            return View("Flight/DeleteFlight", flightFromDb);
        }


        //POST
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFlightPOST(int? id)
        {

            var obj = _db.Flights.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Flights.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("FlightList");

            //return View(obj);

        }




        //GET
        [Authorize(Roles = "Admin")]
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

            return View("Flight/CheckSeats", objSeatList);
        }
    }
}
