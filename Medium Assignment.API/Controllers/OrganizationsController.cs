﻿using System;
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
        public IHttpActionResult Get()
        {
            var organizations = UnitOfWork.Organizations.List();

            var model = new OrganizationListViewModel { Organizations = new List<OrganizationGetViewModel>()};

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
        public IHttpActionResult Get(int id)
        {
            var organization = UnitOfWork.Organizations.Get(id);


            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }
                

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
        public async Task<IHttpActionResult> Post(OrganizationPostViewModel model) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                //AddErrors(result.Errors);
                return BadRequest(ModelState);
            }
            

            result = await UserManager.AddToRoleAsync(user.Id, "OrganizationAdmin");

            if (!result.Succeeded)
            {
                //AddErrors(result.Errors);
                return BadRequest(ModelState);
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

            UnitOfWork.Organizations.Add(organization);

            UnitOfWork.Complete();

            return Ok();

        }

        // PUT: api/Organizations/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, OrganizationPutViewModel model) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
                
            var organization = UnitOfWork.Organizations.Get(id);

            if (organization == null) {
                //AddErrors("Cannot find resource.");
                return BadRequest(ModelState);
            }
                

            var user = UserManager.FindById(organization.ApplicationUserId);

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded) {
//AddErrors(result.Errors);
                return BadRequest(ModelState);

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

            UnitOfWork.Complete();

            return Ok();
        }

        // DELETE: api/Organizations/5
        public void Delete(int id)
        {
        }

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
