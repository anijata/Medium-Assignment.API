using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Medium_Assignment.API.Models
{
    public abstract class WebAPIClientBindingModel
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
