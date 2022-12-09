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
    [Authorize(Roles = "SuperAdmin")]
    public class OrganizationsController : ApiController
    {
        private ApplicationDbContext DbContext { get; set; }

        private IUnitOfWork UnitOfWork { get; set; }

        private ApplicationUserManager _userManager;

        public OrganizationsController() {
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


        // GET: api/Organizations
        /*
         * 
         Gets list of organizations from database
        and returns binding model containing the list of organizations.
         */
        public IHttpActionResult Get()
        {
            IEnumerable<Organization> organizations= new List<Organization>();

            try
            {
                // Get list of organizations from repo.
                organizations = UnitOfWork.Organizations.List();

            }
            catch (Exception ex) {
                return InternalServerError();
            }
            

            var model = new OrganizationListViewModel { Organizations = new List<OrganizationGetViewModel>()};

            // populate binding model with organizations and their fields.
            foreach (var organization in organizations) {
                var modelOrg = new OrganizationGetViewModel
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    UserName = organization.ApplicationUser.UserName,
                    Email = organization.ApplicationUser.Email,
                    PhoneNumber = organization.ApplicationUser.PhoneNumber,
                    Address1 = organization.Address1,
                    Address2 = organization.Address2,
                    ApplicationUserId = organization.ApplicationUserId,
                    Country = organization.Country.Name,
                    CountryId = organization.CountryId,
                    State = organization.State.Name,
                    StateId = organization.StateId,
                    City = organization.City.Name,
                    CityId = organization.CityId,
                    Status = organization.Status,
                    Description = organization.Description,
                };
                model.Organizations.Add(modelOrg);

            }

            return Ok(model);
        }


        // GET: api/Organizations/5

        /*
         Gets organization assoicated with input id
        from database and returns binding model
        containing the organizaiton.
         */
        public IHttpActionResult Get(int id)
        {
            var organization = new Organization();

            try
            {
                // Get organizaiton from repo 
                organization = UnitOfWork.Organizations.Get(id);

            }
            catch (Exception ex)
            {

                return InternalServerError();
            }


            if (organization == null) {
                return NotFound();
            }
                
            // populate binding model with organization fields
            var model = new OrganizationGetViewModel { 
                Id = organization.Id,
                Name = organization.Name,
                UserName = organization.ApplicationUser.UserName,
                Email = organization.ApplicationUser.Email,
                PhoneNumber = organization.ApplicationUser.PhoneNumber,
                Address1 = organization.Address1,
                Address2 = organization.Address2,
                ApplicationUserId = organization.ApplicationUserId,
                Country = organization.Country.Name,
                CountryId = organization.CountryId,
                State = organization.State.Name,
                StateId = organization.StateId,
                City = organization.City.Name,
                CityId = organization.CityId,
                Status = organization.Status,
                Description = organization.Description,        
            };

            return Ok(model);

        }

        // POST: api/Organizations
        [HttpPost]
        /*
         Post request to insert a new Organization record on DB
         */
        public async Task<IHttpActionResult> Post(OrganizationPostViewModel model) 
        {
            // Check for model invalidations.
            if (!ModelState.IsValid) {
                var errors =ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                model.AddErrors(errors);
                return BadRequest(model.GetErrors());
            }

            // Create a new user associated with the new organization.
            // Return error message if not successfull.
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());
            }

            // Adds user to organization admin role

            result = await UserManager.AddToRoleAsync(user.Id, "OrganizationAdmin");
            if (!result.Succeeded)
            {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());
            }

            var organization = new Organization {
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                CityId = model.CityId,
                StateId = model.StateId,
                CountryId = model.CountryId,
                Status = model.Status,
                Description = model.Description,
                ApplicationUserId = user.Id,
                CreatedBy = User.Identity.GetUserId(),
                CreatedOn = DateTime.Now,
                ModifiedBy = User.Identity.GetUserId(),
                ModifiedOn = DateTime.Now,
                
            };

            try
            {
                // Adds organization record to DB.

                UnitOfWork.Organizations.Add(organization);

                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex) {
                return InternalServerError();
            }


            return Ok();

        }

        // PUT: api/Organizations/5
        /*
         Put request to update an Organization record on DB
         */
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, OrganizationPutViewModel model) {

            // Check for model invalidations.
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                model.AddErrors(errors);
                return BadRequest(model.GetErrors());
            }

            var organization = new Organization();

            try
            {
                // Get organizaiton from DB repo.
                organization = UnitOfWork.Organizations.Get(id);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            if (organization == null) {
                return NotFound();
            }
                
            // Get user record associated with this organization.
            var user = UserManager.FindById(organization.ApplicationUserId);

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            // Update any new Get user record associated with this organization.
            // Return error message if the update is unsuccessfull.
            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded) {
                model.AddErrors(result.Errors);
                return BadRequest(model.GetErrors());

            }

            organization.Name = model.Name;
            organization.Address1 = model.Address1;
            organization.Address2 = model.Address2;
            organization.CityId = model.CityId;
            organization.StateId = model.StateId;
            organization.CountryId = model.CountryId;
            organization.Status = model.Status;
            organization.Description = model.Description;
            organization.ModifiedBy = User.Identity.GetUserId();
            organization.ModifiedOn = DateTime.Now;

            try
            {
                // Update the organization record in DB.
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }


            return Ok();
        }

        // DELETE: api/Organizations/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var organization = new Organization();

            try {
                // Get organizaiton from DB repo.
                organization = UnitOfWork.Organizations.Get(id);
                if (organization == null)
                {
                    return NotFound();
                }
            }     
            catch (Exception ex)
            {
                return InternalServerError();
            }


            

            try
            {
                // Soft delete organization from DB
                UnitOfWork.Organizations.Remove(organization);

                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }

            return Ok();
        }

    }
}
