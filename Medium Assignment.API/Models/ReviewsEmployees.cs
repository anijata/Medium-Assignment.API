using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medium_Assignment.API.Models
{
    public class ReviewsEmployees
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ReviewId")]
        public Review Review { get; set; }
        public int ReviewId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee  { get; set; }
        public int EmployeeId { get; set; }    
        public int Rating { get; set; }
        public string Feedback { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}