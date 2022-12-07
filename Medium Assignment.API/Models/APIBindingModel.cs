using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Medium_Assignment.API.Models
{
    public class APIBindingModel
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public string GetErrors() {

            return string.Join("\n", Errors);
        }

        public void AddErrors(string error)
        {
            Errors.Concat(new List<string> { error });
        }

        public void AddErrors(IEnumerable<string> errors) {
            Errors = Errors.Concat(errors);
        }
    }
}
