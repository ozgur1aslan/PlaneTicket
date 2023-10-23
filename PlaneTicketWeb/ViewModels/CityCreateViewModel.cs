using Microsoft.AspNetCore.Mvc.Rendering;
using PlaneTicketWeb.Models;

namespace PlaneTicketWeb.ViewModels
{
    public class CityCreateViewModel
    {
        public City? City { get; set; }
        public SelectList? CountryList { get; set; }
    }
}
