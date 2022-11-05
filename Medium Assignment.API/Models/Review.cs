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
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ReviewerId")]
        public Employee Reviewer { get; set; }

        public int? ReviewerId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public int? EmployeeId { get; set; }


        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public int OrganizationId { get; set; }

        public string Agenda { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Review Cycle Start Date")]
        public DateTime ReviewCycleStartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Review Cycle End Date")]
        public DateTime ReviewCycleEndDate { get; set; }

        [Display(Name = "Min Rate")]
        public decimal MinRate { get; set; }

        [Display(Name = "Max Rate")]
        public decimal MaxRate { get; set; }

        public string Description { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }

        
        [ForeignKey("ReviewStatusId")] 
        public ReviewStatus ReviewStatus { get; set; }

        public int ReviewStatusId { get; set; }

        public bool IsDeleted { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}