using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Medium_Assignment.API.Models;
using Microsoft.Owin;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Medium_Assignment.API.Providers;
using Medium_Assignment.API.Results;


namespace Medium_Assignment.API.Controllers
{
    [Authorize(Roles = "OrganizationAdmin")]
    public class DepartmentsController : ApiController
    {
        private ApplicationDbContext DbContext { get; set; }

        private ApplicationUserManager _userManager;

        public DepartmentsController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/departments
        public IHttpActionResult Get()
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var departments = DbContext.Departments
                .Where(c => c.OrganizationId == organization.Id && !c.IsDeleted)
                .ToList();

            var model = new DepartmentListViewModel
            {

                Departments = new List<DepartmentGetViewModel>()
            };

            foreach (var department in departments) {
                var modelDepartment = new DepartmentGetViewModel
                {

                    Id = department.Id,

                    Name = department.Name,

                    OrganizationId = department.OrganizationId

                };

                model.Departments.Add(modelDepartment);

            }

            

            return Ok(model);
        }

        // GET api/departments/5
        public IHttpActionResult Get(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var department = DbContext.Departments
                .Where(c => c.OrganizationId == organization.Id && !c.IsDeleted && c.Id == id)
                .SingleOrDefault();

            if (department == null)
                return BadRequest();

            var model = new DepartmentGetViewModel { 
            
                Id = department.Id,

                Name = department.Name,

                OrganizationId = department.OrganizationId

            };

            return Ok(model);
        }

        // POST api/departments
        public IHttpActionResult Post(DepartmentNewViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var department = new Department { 
                Name = model.Name,
                OrganizationId = organization.Id            
            };

            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            DbContext.Departments.Add(department);

            DbContext.SaveChanges();

            return Ok();
        }

        // PUT api/departments/5
        [HttpPut]
        public IHttpActionResult Put(int id, DepartmentNewViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var department = DbContext.Departments
                .Where(c => c.OrganizationId == organization.Id && !c.IsDeleted && c.Id == id)
                .SingleOrDefault();

            if (department == null)
                return BadRequest();

            department.Name = model.Name;
            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            DbContext.SaveChanges();


            return Ok();
        }

        // DELETE api/departments/5
        public IHttpActionResult Delete(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var department = DbContext.Departments
                .Where(c => c.OrganizationId == organization.Id && !c.IsDeleted && c.Id == id)
                .SingleOrDefault();

            if (department == null)
                return BadRequest();

            department.IsDeleted = true;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            DbContext.SaveChanges();

            return Ok();
        }
    }
}
