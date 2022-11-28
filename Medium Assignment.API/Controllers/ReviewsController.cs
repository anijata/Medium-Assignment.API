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
    
    public class ReviewsController : ApiController
    {
        private ApplicationDbContext DbContext { get; set; }

        private ApplicationUserManager _userManager;

        private UnitOfWork UnitOfWork;

        public ReviewsController()
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

        [Authorize(Roles = "OrganizationAdmin")]
        // GET api/reviews
        public IHttpActionResult Get()
        {
            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not available");
                return BadRequest(ModelState);
            }

            var reviews = UnitOfWork.Reviews.List(c => organization.Id == c.OrganizationId);

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

        [Authorize(Roles = "OrganizationAdmin")]
        // GET api/reviews/{id}
        public IHttpActionResult Get(int id)
        {

            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var review = UnitOfWork.Reviews.Get(id);

            if (review == null || review.OrganizationId != organization.Id)
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

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

        [HttpGet]
        [Authorize(Roles = "Employee")]
        [Route("api/reviews/assigned/{id}")]
        public IHttpActionResult AssignedReview(int id)
        {

            var currentUserId = User.Identity.GetUserId();

            var review = UnitOfWork.Reviews.Get(id);

            if (review == null ||
                !review.Reviewer.ApplicationUserId.Equals(currentUserId))
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var model = new ReviewGetViewModel
            {
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


        [HttpGet]
        [Route("api/reviews/assigned")]
        [Authorize(Roles = "Employee")]
        public IHttpActionResult AssignedReviews() {
            var currentUserId = User.Identity.GetUserId();

            var reviews = UnitOfWork.Reviews.List(c => c.Reviewer.ApplicationUserId.Equals(currentUserId));

            var model = new ReviewListViewModel
            {
                Reviews = new List<ReviewGetViewModel>()

            };

            foreach (var review in reviews)
            {
                var getModel = new ReviewGetViewModel
                {
                    Id = review.Id,
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



                model.Reviews.Add(getModel);
            }

            return Ok(model);
        
        }

        [Authorize(Roles = "OrganizationAdmin")]
        // POST api/reviews
        [Route("api/reviews/new")]
        [HttpPost]
        public IHttpActionResult NewReview(ReviewNewViewModel viewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();


            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

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

            UnitOfWork.Reviews.Add(review);

            UnitOfWork.Complete();

            return Ok();
        }

        [Authorize(Roles = "OrganizationAdmin")]
        [HttpPut]
        [Route("api/reviews/assign/{id}")]
        public IHttpActionResult AssignReview(int id, ReviewAssignBindingModel bindModel) 
        {
            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                   .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

            if (organization == null) {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var review = UnitOfWork.Reviews.Get(id);

            if (review == null || review.OrganizationId != organization.Id)
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
                
            }

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

                UnitOfWork.Reviews.Add(newReview);
            }

            UnitOfWork.Complete();

            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut]
        [Route("api/reviews/submit/{id}")]
        public IHttpActionResult SubmitReview(int id, ReviewSubmitBindingModel bindModel) {
            var currentUserId = User.Identity.GetUserId();

            var review = UnitOfWork.Reviews.Get(id);

            if (review == null ||
                !review.Reviewer.ApplicationUserId.Equals(currentUserId))
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            review.Feedback = bindModel.Feedback;
            review.Rating = bindModel.Rating;
            review.ReviewStatusId = 3;
            review.ModifiedBy = currentUserId;
            review.ModifiedOn = DateTime.Now;

            UnitOfWork.Complete();

            return Ok();
        }


        // DELETE api/reviews/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            var organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

            if (organization == null)
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            var review = UnitOfWork.Reviews.Get(id);

            if (review == null || review.OrganizationId != organization.Id)
            {
                //AddErrors("Resource not found");
                return BadRequest(ModelState);
            }

            review.IsDeleted = true;

            UnitOfWork.Complete();

            return Ok();

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
