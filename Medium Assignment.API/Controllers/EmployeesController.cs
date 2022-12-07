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
    public class EmployeesController : ApiController
    {

        private ApplicationDbContext DbContext { get; set; }

        private ApplicationUserManager _userManager;

        private IUnitOfWork UnitOfWork { get; set; }

        public EmployeesController()
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


        // GET api/employees
        public IHttpActionResult Get()
        {
            string currentUserId;
            IEnumerable<Employee> employees;
            Organization organization;

            try {
                currentUserId = User.Identity.GetUserId();
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                employees = UnitOfWork.Employees.List(c => c.OrganizationId == organization.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            var model = new EmployeeListViewModel { Employees = new List<EmployeeGetViewModel>() };

            foreach (var employee in employees) {
                var modelemp = new EmployeeGetViewModel
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DisplayName = employee.DisplayName,
                    Email = employee.ApplicationUser.Email,
                    PhoneNumber = employee.ApplicationUser.PhoneNumber,
                    Address1 = employee.Address1,
                    Address2 = employee.Address2,
                    Country = employee.Country.Name,
                    CountryId = employee.CountryId,
                    State = employee.State.Name,
                    StateId = employee.StateId,
                    City = employee.City.Name,
                    CityId = employee.CityId,
                    ApplicationUserId = employee.ApplicationUserId,
                    Department = employee.Department.Name,
                    DepartmentId = employee.DepartmentId,
                    UserName = employee.ApplicationUser.UserName,
                    Designation = employee.Designation,
                    DOB = employee.DOB,
                    DOJ = employee.DOJ,
                    EmployeeType = employee.EmployeeType,
                    Gender = employee.Gender,
                    OrganizationId = employee.OrganizationId,
                    Status = employee.Status
                };

                model.Employees.Add(modelemp);
            }

            return Ok(model);
        }

        // GET api/employees/5
        public IHttpActionResult Get(int id)
        {
            string currentUserId;
            Employee employee;
            Organization organization;

            try
            {
                currentUserId = User.Identity.GetUserId();
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return Unauthorized();
                }

                employee = UnitOfWork.Employees.Get(id);

                if (employee == null || employee.OrganizationId != organization.Id)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            

            var model = new EmployeeGetViewModel { 
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DisplayName = employee.DisplayName,
                Email = employee.ApplicationUser.Email,
                PhoneNumber = employee.ApplicationUser.PhoneNumber,
                Address1 = employee.Address1,
                Address2 = employee.Address2,
                Country = employee.Country.Name,
                CountryId = employee.CountryId,
                State = employee.State.Name,
                StateId = employee.StateId,
                City = employee.City.Name,
                CityId = employee.CityId,
                ApplicationUserId = employee.ApplicationUserId,
                Department = employee.Department.Name,
                DepartmentId = employee.DepartmentId,
                UserName = employee.ApplicationUser.UserName,
                Designation = employee.Designation,
                DOB = employee.DOB,
                DOJ = employee.DOJ,
                EmployeeType = employee.EmployeeType,
                Gender = employee.Gender,
                OrganizationId = employee.OrganizationId,
                Status = employee.Status
            };

            return Ok(model);
        }

        // POST api/employees
        [HttpPost]
        public async Task<IHttpActionResult> Post(EmployeePostViewModel model)
        {
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

            try {

                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();



            }
            catch (Exception ex) {
                return InternalServerError();
            }

            
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());
            }


            result = await UserManager.AddToRoleAsync(user.Id, "Employee");

            if (!result.Succeeded)
            {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());
            }

            var employee = new Employee { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address1 = model.Address1,
                Address2 = model.Address2,
                CityId = model.CityId,
                StateId = model.StateId,
                CountryId = model.CountryId,
                ApplicationUserId = user.Id,
                DepartmentId = model.DepartmentId,
                Designation  = model.Designation,
                DOB = model.DOB,
                DOJ = model.DOJ,
                Status = model.Status,
                OrganizationId = organization.Id,
                EmployeeType = model.EmployeeType,
                Gender = model.Gender,
                CreatedBy = currentUserId,
                CreatedOn = DateTime.Now,
                ModifiedBy = currentUserId,
                ModifiedOn = DateTime.Now
            };

            try {
                UnitOfWork.Employees.Add(employee);

                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }


            

            return Ok();
        }

        // PUT api/employees/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, EmployeePutViewModel model)
        {
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

            Employee employee;

            try
            {

                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();

                }

                employee = UnitOfWork.Employees.Get(id);

                if (employee == null || employee.OrganizationId != organization.Id)
                {
                    return NotFound();

                }

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            var user = UserManager.FindById(employee.ApplicationUserId);

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Address1 = model.Address1;
            employee.Address2 = model.Address2;
            employee.CityId = model.CityId;
            employee.StateId = model.StateId;
            employee.CountryId = model.CountryId;
            employee.DepartmentId = model.DepartmentId;
            employee.Designation = model.Designation;
            employee.DOB = model.DOB;
            employee.DOJ = model.DOJ;
            employee.Status = model.Status;
            employee.EmployeeType = model.EmployeeType;
            employee.Gender = model.Gender;
            employee.ModifiedBy = currentUserId;
            employee.ModifiedOn = DateTime.Now;

            try
            {
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex) {

                return InternalServerError();
            } 

            

            return Ok();
        }

        // DELETE api/employees/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            Organization organization;

            Employee employee;

            try
            {

                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();

                }

                employee = UnitOfWork.Employees.Get(id);

                if (employee == null || employee.OrganizationId != organization.Id)
                {
                    return NotFound();

                }

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }


            try {
                UnitOfWork.Employees.Remove(employee);

                UnitOfWork.Complete();

            } catch (DbUpdateException ex) {
                return InternalServerError();
            }




            return Ok();
        }
    }
}
