using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medium_Assignment.API.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public State State { get; set; }
        
        public int StateId { get; set; }
    }
}