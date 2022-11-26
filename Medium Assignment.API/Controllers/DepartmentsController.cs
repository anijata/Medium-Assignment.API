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
using Medium_Assignment.API.Repo;

namespace Medium_Assignment.API.Controllers
{
    [Authorize(Roles = "OrganizationAdmin")]
    public class DepartmentsController : ApiController
    {
        private ApplicationDbContext DbContext { get; set; }

        private ApplicationUserManager _userManager;

        private UnitOfWork UnitOfWork;

        public DepartmentsController()
        {
            DbContext = new ApplicationDbContext();
            UnitOfWork = new UnitOfWork(DbContext);
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
            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var departments = UnitOfWork.Departments.List(c => c.OrganizationId == organization.Id);

            var model = new DepartmentListViewModel
            {

                Departments = new List<DepartmentGetViewModel>()
            };

            foreach (var department in departments) {
                var modelDepartment = new DepartmentGetViewModel
                {

                    Id = department.Id,

                    Name = department.Name,


                };

                model.Departments.Add(modelDepartment);

            }

            

            return Ok(model);
        }

        // GET api/departments/5
        public IHttpActionResult Get(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

            if (organization == null) {

                //AddErrors("Resource not found");
                return BadRequest(ModelState);

            }

            var department = UnitOfWork.Departments.Get(id);

            if (department == null || department.OrganizationId != organization.Id) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var model = new DepartmentGetViewModel { 
            
                Id = department.Id,

                Name = department.Name,


            };

            return Ok(model);
        }

        // POST api/departments
        public IHttpActionResult Post(DepartmentPostViewModel model)
        {
            if (!ModelState.IsValid) {
                
            }

            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                  .List(c => c.ApplicationUserId.Equals(currentUserId))
                  .FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var department = new Department { 
                Name = model.Name,
                OrganizationId = organization.Id            
            };

            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            UnitOfWork.Departments.Add(department);

            UnitOfWork.Complete();

            return Ok();
        }

        // PUT api/departments/5
        [HttpPut]
        public IHttpActionResult Put(int id, DepartmentPutViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var organization = UnitOfWork.Organizations
                  .List(c => c.ApplicationUserId.Equals(currentUserId))
                  .FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);               
            }

            var department = UnitOfWork.Departments.Get(id);

            if (department == null || department.OrganizationId != organization.Id) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            department.Name = model.Name;
            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            UnitOfWork.Complete();


            return Ok();
        }

        // DELETE api/departments/5
        //public IHttpActionResult Delete(int id)
        //{
        //    var currentUserId = User.Identity.GetUserId();
        //    var organization = DbContext.Organizations.Where(c => c.ApplicationUserId.Equals(currentUserId)).SingleOrDefault();

        //    if (organization == null) {
        //        ModelState.AddModelError("", "Not Authorized to delete this review");
        //        return BadRequest(ModelState);
        //    }

        //    var department = DbContext.Departments
        //        .Where(c => c.OrganizationId == organization.Id && !c.IsDeleted && c.Id == id)
        //        .SingleOrDefault();

        //    if (department == null) { 
        //        ModelState.AddModelError("", "Not Authorized to delete this review");
        //        return BadRequest(ModelState);
        //    }

        //    department.IsDeleted = true;
        //    department.ModifiedBy = currentUserId;
        //    department.ModifiedOn = DateTime.Now;

        //    DbContext.SaveChanges();

        //    return Ok();
        //}

        //public void AddErrors(string error)
        //{
        //    ModelState.AddModelError("", error);
        //}

        //public void AddErrors(IEnumerable<string> errors)
        //{
        //    foreach (var error in errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}
    }
}
