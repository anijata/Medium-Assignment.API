using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Medium_Assignment.API.Models
{
    public class DepartmentListViewModel
    {
        public List<DepartmentGetViewModel> Departments { get; set; }
    }

    public class DepartmentGetViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }

    public class DepartmentPostViewModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class DepartmentPutViewModel
    {

        [Required]
        public string Name { get; set; }
    }
}