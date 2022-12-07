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
            Organization organization;
            IEnumerable<Review> reviews;
            try
            {
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                reviews = UnitOfWork.Reviews.List(c => organization.Id == c.OrganizationId);

            }
            catch (Exception ex) { 
                return InternalServerError();
            }

            
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
            Organization organization;
            Review review;

            try
            {
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                review = UnitOfWork.Reviews.Get(id);

                if (review == null || review.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
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

            Review review;

            try {
                review = UnitOfWork.Reviews.Get(id);

                if (review == null ||
                    !review.Reviewer.ApplicationUserId.Equals(currentUserId))
                {
                    return NotFound();
                }

            } catch (Exception ex) { 
                return InternalServerError();
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

            IEnumerable<Review> reviews;

            try
            {
                reviews = UnitOfWork.Reviews.
                    List(c => c.Reviewer.ApplicationUserId.Equals(currentUserId));
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            
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
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                viewModel.AddErrors(errors);
                return BadRequest(viewModel.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();

            Organization organization;

            try
            {
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();


                if (organization == null)
                {
                    return NotFound();
                }
            } catch (Exception ex) {
                return InternalServerError();
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

            try
            {
                UnitOfWork.Reviews.Add(review);

                UnitOfWork.Complete();

            }
            catch (DbUpdateException ex) {
                return InternalServerError();            
            }



            return Ok();
        }

        [Authorize(Roles = "OrganizationAdmin")]
        [HttpPut]
        [Route("api/reviews/edit/{id}")]
        public IHttpActionResult EditReview(int id, ReviewEditViewModel viewModel) {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                viewModel.AddErrors(errors);
                return BadRequest(viewModel.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();

            Organization organization;
            Review review;

            try
            {
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                review = UnitOfWork.Reviews.Get(id);

                if (review == null || review.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex) {
                return InternalServerError();
            }

            review.Agenda = viewModel.Agenda;
            review.ReviewCycleStartDate = viewModel.ReviewCycleStartDate;
            review.ReviewCycleEndDate = viewModel.ReviewCycleEndDate;
            review.MinRate = viewModel.MinRate;
            review.MaxRate = viewModel.MaxRate;
            review.Description = viewModel.Description;
            review.ModifiedBy = currentUserId;
            review.ModifiedOn = DateTime.Now;

            try {
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }

            

            return Ok();

        }

        [Authorize(Roles = "OrganizationAdmin")]
        [HttpPut]
        [Route("api/reviews/assign/{id}")]
        public IHttpActionResult AssignReview(int id, ReviewAssignBindingModel bindModel) 
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                bindModel.AddErrors(errors);
                return BadRequest(bindModel.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();

            Organization organization;
            Review review;

            try
            {
                organization = UnitOfWork.Organizations
                   .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                review = UnitOfWork.Reviews.Get(id);

                if (review == null || review.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
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

            try
            {
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }


            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut]
        [Route("api/reviews/submit/{id}")]
        public IHttpActionResult SubmitReview(int id, ReviewSubmitBindingModel bindModel) {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors)
                 .Select(e => e.ErrorMessage)
                 .ToList();
                bindModel.AddErrors(errors);
                return BadRequest(bindModel.GetErrors());
            }

            var currentUserId = User.Identity.GetUserId();

            Review review;

            try
            {
                review = UnitOfWork.Reviews.Get(id);

                if (review == null ||
                    !review.Reviewer.ApplicationUserId.Equals(currentUserId))
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }


            review.Feedback = bindModel.Feedback;
            review.Rating = bindModel.Rating;
            review.ReviewStatusId = 3;
            review.ModifiedBy = currentUserId;
            review.ModifiedOn = DateTime.Now;

            try
            {
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError();
            }

            return Ok();
        }


        // DELETE api/reviews/5
        [HttpDelete]
        [Authorize(Roles = "OrganizationAdmin")]
        public IHttpActionResult Delete(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            Organization organization;
            Review review;

            try
            {
                organization = UnitOfWork.Organizations
                .List(c => c.ApplicationUserId.Equals(currentUserId)).FirstOrDefault();

                if (organization == null)
                {
                    return NotFound();
                }

                review = UnitOfWork.Reviews.Get(id);

                if (review == null || review.OrganizationId != organization.Id)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            review.IsDeleted = true;

            try
            {
                UnitOfWork.Complete();
            }
            catch (DbUpdateException ex) {
                return InternalServerError();
            }
            

            return Ok();

        }

 

    }
}
