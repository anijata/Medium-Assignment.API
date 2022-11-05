using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medium_Assignment.API.Models
{
    public class State
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }
    }
}