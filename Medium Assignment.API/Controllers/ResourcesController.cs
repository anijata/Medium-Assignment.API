using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Medium_Assignment.API.Models;

namespace Medium_Assignment.API.Controllers
{
    
    public class ResourcesController : ApiController
    {

        private ApplicationDbContext DbContext { get; set; }


        public ResourcesController()
        {
            DbContext = new ApplicationDbContext();
        }

        [Route("api/resources/countries")]
        public IEnumerable<Country> GetCountries() {
            var countries = DbContext.Countries.ToList();

            return countries;             
        }

        [Route("api/resources/countries/{id}")]
        public Country GetCountries(int id)
        {
            var country = DbContext.Countries.Find(id);

            return country;
        }


        [Route("api/resources/states/{countryid}")]
        public IEnumerable<State> GetStates(int countryid)
        {
            var states = DbContext.States.Where(c => countryid == c.CountryId).ToList();

            return states;
        }

        [Route("api/resources/cities/{stateid}")]
        public IEnumerable<City> GetCities(int stateid)
        {
            var cities = DbContext.Cities.Where(c => stateid == c.StateId).ToList();

            return cities;
        }

    }
}
