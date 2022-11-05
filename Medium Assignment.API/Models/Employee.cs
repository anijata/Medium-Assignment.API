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
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return $"{FirstName} {LastName} ({ApplicationUser.UserName})";
            }
        }


        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        [Display(Name = "Date of Joining")]
        public DateTime DOJ { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [Required]
        public int DepartmentId {get; set;} 

        [Required]
        [Display(Name = "Address field 1")]
        public string Address1 { get; set; }

        [Required]
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

        public string EmployeeType { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public int OrganizationId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}