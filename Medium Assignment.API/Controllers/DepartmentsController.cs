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
using System.Data.Entity.Infrastructure;

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

        /*
         Gets all departments associated with the organization
        the user (organization admin) is associated.
         */

        // GET api/departments
        public IHttpActionResult Get()
        {
            var currentUserId = User.Identity.GetUserId();

            Organization organization;
            IEnumerable<Department> departments;

            try
            {
                //Get organizaition associated with the user (organization admin).
                //This is to determine if the user is authorized to interact with these departments.
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                // Get departments associated with this organization.
                departments = UnitOfWork.Departments.List(c => c.OrganizationId == organization.Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            // Populate binding model with data from departments. 
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



        /*
         Gets record of department given id and associated with the organization
        the user (organization admin) is associated.
         */
        // GET api/departments/{id}
        public IHttpActionResult Get(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            Organization organization;
            Department department;

            try
            {
                //Get organizaition associated with the user (organization admin).
                //This is to determine if the user is authorized to interact with these departments.
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                // Get department associated with this id and organization.
                department = UnitOfWork.Departments.Get(id);

                if (department == null || department.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            var model = new DepartmentGetViewModel { 
            
                Id = department.Id,

                Name = department.Name,


            };

            return Ok(model);
        }

        // POST api/departments
        /*
         Inserts a record of department and associated with the organization
        the user (organization admin) is associated.
         */
        public IHttpActionResult Post(DepartmentPostViewModel model)
        {
            //Check for model invalidation.
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                model.AddErrors(errors);
                return BadRequest(model.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();

            Organization organization = new Organization();

            try
            {
                //Get organizaition associated with the user (organization admin).
                //This is to determine if the user is authorized to interact with these departments.
                organization = UnitOfWork.Organizations
                  .List(c => c.ApplicationUserId.Equals(currentUserId))
                  .FirstOrDefault();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            

            if (organization == null) {
                return NotFound();
            }

            var department = new Department { 
                Name = model.Name,
                OrganizationId = organization.Id            
            };

            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;


            // Update Department record to DB.
            try
            {
                UnitOfWork.Departments.Add(department);
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }



            return Ok();
        }

        // PUT api/departments/5
        /*
        Update record of department given id and associated with the organization
       the user (organization admin) is associated.
        */
        [HttpPut]
        public IHttpActionResult Put(int id, DepartmentPutViewModel model)
        {
            //Check for model invalidation.

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                model.AddErrors(errors);
                return BadRequest(model.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();
            Organization organization;
            Department department;

            try
            {
                //Get organizaition associated with the user (organization admin).
                //This is to determine if the user is authorized to interact with these departments.
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                // Get department associated with this id and organization.
                department = UnitOfWork.Departments.Get(id);

                if (department == null || department.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            department.Name = model.Name;
            department.CreatedOn = DateTime.Now;
            department.CreatedBy = currentUserId;
            department.ModifiedBy = currentUserId;
            department.ModifiedOn = DateTime.Now;

            // Update Department record in DB.
            try {
                UnitOfWork.Complete();
            } catch (DbUpdateException ex) {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            return Ok();
        }

        // DELETE api/departments/5
        /*
       Delete record of department given id and associated with the organization
      the user (organization admin) is associated.
       */
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            Organization organization;
            Department department;

            try
            {
                //Get organizaition associated with the user (organization admin).
                //This is to determine if the user is authorized to interact with these departments.

                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId))
                .FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                // Get department associated with this id and organization.
                department = UnitOfWork.Departments.Get(id);

                if (department == null || department.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            // Soft Delete Department record in DB.
            try
            {
                UnitOfWork.Departments.Remove(department);

                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

            return Ok();
        }

    }
}
