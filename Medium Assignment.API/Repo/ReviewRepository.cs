using System.Collections.Generic;
using System.Linq;
using System.Web;
using Medium_Assignment.API.Models;
using Medium_Assignment.API.Repo;
using System.Linq.Expressions;
using System.Data.Entity;
using System;

namespace Medium_Assignment.API.Repo
{
    public class ReviewRepository : GenericRepository<Review>

    {
        public override ApplicationDbContext DbContext { get; set; }
        public ReviewRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            DbContext = _dbContext;
        }
        public override Review Get(int id)
        {
            var review = DbContext.Reviews
                .Where(c => c.Id == id && !c.IsDeleted)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .SingleOrDefault();

            return review;
        }

        public override IEnumerable<Review> List()
        {
            var reviews = DbContext.Reviews
                .Where(c => !c.IsDeleted)
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .ToList();

            return reviews;
        }

        public override IEnumerable<Review> List(Expression<Func<Review, bool>> predicate)
        {
            var reviews = DbContext.Reviews                
                .Include(c => c.Employee)
                .Include(c => c.Reviewer)
                .Include(c => c.ReviewStatus)
                .Where(c => !c.IsDeleted)
                .Where(predicate)
                .ToList();

            return reviews;
        }


    }
}