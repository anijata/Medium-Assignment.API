using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medium_Assignment.API.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Address field 1")]
        public string Address1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Address field 2")]
        public string Address2 { get; set; }

        public Country Country { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public State State { get; set; }

        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }

        public City City { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        
        public string ApplicationUserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }


    }
}