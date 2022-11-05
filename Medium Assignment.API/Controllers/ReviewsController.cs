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
    public class ReviewsController : ApiController
    {
        private ApplicationDbContext DbContext { get; set; }

        private ApplicationUserManager _userManager;

        public ReviewsController()
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

        // GET api/reviews
        public IHttpActionResult Get()
        {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            var review = DbContext.Reviews
                .Where(c => !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            return BadRequest();

        }

        // GET api/reviews/5
        public IHttpActionResult Get(int id)
        {

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            var review = DbContext.Reviews
                .Where(c => !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            var model = new ReviewGetViewModel {
                Agenda = review.Agenda,
                Description = review.Description,
                EmployeeId = review.EmployeeId,
                Employee = review.Employee.DisplayName,
                Feedback = review.Feedback,
                MaxRate = review.MaxRate,
                MinRate = review.MinRate,
                OrganizationId = review.OrganizationId,
                Rating = review.Rating,
                ReviewCycleStartDate = review.ReviewCycleStartDate,
                ReviewCycleEndDate = review.ReviewCycleStartDate,
                Reviewer = review.Reviewer.DisplayName,
                ReviewerId = review.ReviewerId,
                ReviewStatus = review.ReviewStatus.Name,
                ReviewStatusId = review.ReviewStatusId

            };

            DbContext.Reviews.Add(review);

            DbContext.SaveChanges();

            return Ok(model);
        }

        // POST api/reviews
        public IHttpActionResult Post(ReviewNewViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var review = new Review { 
            
                Agenda = model.Agenda,
                Description = model.Description,
                ReviewCycleStartDate = model.ReviewCycleStartDate,
                ReviewCycleEndDate = model.ReviewCycleEndDate,
                MinRate = model.MinRate,
                MaxRate = model.MaxRate,
                OrganizationId = organization.Id,
                CreatedBy = currentUserId,
                CreatedOn = DateTime.Now,
                ModifiedBy = currentUserId,
                ModifiedOn = DateTime.Now

            };

            return Ok();
        }
        // PUT api/reviews/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/reviews/5
        public void Delete(int id)
        {
        }

    }
}
