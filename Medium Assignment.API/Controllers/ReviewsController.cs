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

            var reviews = DbContext.Reviews
                .Where(c => !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .ToList();

            var model = new ReviewListViewModel {
                Reviews = new List<ReviewGetViewModel>()
            
            };

            foreach (var review in reviews) { 
                var getModel = new ReviewGetViewModel
                {
                    Id = review.Id,
                    Agenda = review.Agenda,
                    Description = review.Description,
                    EmployeeId = review.EmployeeId ?? 0,
                    Employee = (review.Employee == null) ? "" :  $"{review.Employee.FirstName}, {review.Employee.LastName}",
                    Feedback = review.Feedback,
                    MaxRate = review.MaxRate,
                    MinRate = review.MinRate,
                    OrganizationId = review.OrganizationId,
                    Rating = review.Rating,
                    ReviewCycleStartDate = review.ReviewCycleStartDate,
                    ReviewCycleEndDate = review.ReviewCycleStartDate,
                    Reviewer = (review.Reviewer == null) ? "" : $"{review.Reviewer.FirstName}, {review.Reviewer.LastName}",
                    ReviewerId = review.ReviewerId ?? 0,
                    ReviewStatus = review.ReviewStatus.Name,
                    ReviewStatusId = review.ReviewStatusId

                };

                

                model.Reviews.Add(getModel);
            }

            return Ok(model);

        }

        // GET api/reviews/{id}
        public IHttpActionResult Get(int id)
        {

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            var review = DbContext.Reviews
                .Where(c => c.Id == id && !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            if (review == null)
                return BadRequest();

            var model = new ReviewGetViewModel {
                Id = id,
                Agenda = review.Agenda,
                Description = review.Description,
                EmployeeId = review.EmployeeId ?? 0,
                Employee = (review.Employee == null) ? "" : $"{review.Employee.FirstName}, {review.Employee.LastName}",
                Feedback = review.Feedback,
                MaxRate = review.MaxRate,
                MinRate = review.MinRate,
                OrganizationId = review.OrganizationId,
                Rating = review.Rating,
                ReviewCycleStartDate = review.ReviewCycleStartDate,
                ReviewCycleEndDate = review.ReviewCycleStartDate,
                Reviewer = (review.Reviewer == null) ? "" : $"{review.Reviewer.FirstName}, {review.Reviewer.LastName}",
                ReviewerId = review.ReviewerId ?? 0,
                ReviewStatus = review.ReviewStatus.Name,
                ReviewStatusId = review.ReviewStatusId

            };


            return Ok(model);
        }

        // POST api/reviews
        [Route("api/reviews/new")]
        [HttpPost]
        public IHttpActionResult NewReview(ReviewNewViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            if (organization == null)
                return BadRequest();

            var review = new Review
            {
                EmployeeId = null,
                ReviewerId = null,
                ReviewStatusId = 1,
                Agenda = viewModel.Agenda,
                Description = viewModel.Description,
                ReviewCycleStartDate = viewModel.ReviewCycleStartDate,
                ReviewCycleEndDate = viewModel.ReviewCycleEndDate,
                MinRate = viewModel.MinRate,
                MaxRate = viewModel.MaxRate,
                OrganizationId = organization.Id,
                CreatedBy = currentUserId,
                CreatedOn = DateTime.Now,
                ModifiedBy = currentUserId,
                ModifiedOn = DateTime.Now

            };

            DbContext.Reviews.Add(review);

            DbContext.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("api/reviews/assign/{id}")]
        public IHttpActionResult AssignReview(int id, ReviewAssignBindingModel bindModel) {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            var review = DbContext.Reviews
                .Where(c => c.Id == id &&  !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            if (review == null)
                return BadRequest();

            review.ReviewerId = bindModel.ReviewerId;
            review.EmployeeId = bindModel.EmployeeIds.First();
            review.ReviewStatusId = 2;
            review.ModifiedBy = currentUserId;
            review.ModifiedOn = DateTime.Now;

            foreach (var employeeId in bindModel.EmployeeIds.Skip(1)) {
                var newReview = new Review
                {
                    EmployeeId = employeeId,
                    ReviewerId = bindModel.ReviewerId,
                    ReviewStatusId = 2,
                    Agenda = review.Agenda,
                    Description = review.Description,
                    ReviewCycleStartDate = review.ReviewCycleStartDate,
                    ReviewCycleEndDate = review.ReviewCycleEndDate,
                    MinRate = review.MinRate,
                    MaxRate = review.MaxRate,
                    OrganizationId = organization.Id,
                    CreatedBy = currentUserId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = currentUserId,
                    ModifiedOn = DateTime.Now

                };

                DbContext.Reviews.Add(newReview);
            }

            DbContext.SaveChanges();

            return Ok();
        }

        
        [HttpPut]
        [Route("api/reviews/submit/{id}")]
        public IHttpActionResult SubmitReview(int id, ReviewSubmitBindingModel bindModel) {
            var currentUserId = User.Identity.GetUserId();
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.ApplicationUserId.Equals(currentUserId))
                .SingleOrDefault();

            var review = DbContext.Reviews
                .Where(c => c.Id == id && !c.IsDeleted && c.OrganizationId == c.OrganizationId)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            if (review == null)
                return BadRequest();

            review.Feedback = bindModel.Feedback;
            review.Rating = bindModel.Rating;
            review.ReviewStatusId = 3;
            review.ModifiedBy = currentUserId;
            review.ModifiedOn = DateTime.Now;

            DbContext.SaveChanges();

            return Ok();
        }

        // DELETE api/reviews/5
        public void Delete(int id)
        {
        }

    }
}
