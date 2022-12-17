using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Medium_Assignment.API.Models;
using Medium_Assignment.API.Repo;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Medium_Assignment.API.Repo
{
    public class OrganizationRepository : GenericRepository<Organization>
    
    { 
        public override ApplicationDbContext DbContext { get; set; }
        public OrganizationRepository(ApplicationDbContext _dbContext): base(_dbContext)
        {
            DbContext = _dbContext;
        }
        public override Organization Get(int id)
        {
            var organization = DbContext.Organizations
                .Where(c => !c.IsDeleted && c.Id == id)
                .Include(c => c.ApplicationUser)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.City)
                .SingleOrDefault();

            return organization;
        }


        public override IEnumerable<Organization> List()
        {
            var organizations = DbContext.Organizations
                .Where(c => !c.IsDeleted)
                .Include(c => c.ApplicationUser)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.City)
                .ToList();

            return organizations;
        }

        public override IEnumerable<Organization> List(Expression<Func<Organization, bool>> predicate)
        {
            var organizations = DbContext.Organizations
                .Where(c => !c.IsDeleted)
                .Include(c => c.ApplicationUser)
                .Include(c => c.Country)
                .Include(c => c.State)
                .Include(c => c.City)
                .Where(predicate)
                .ToList();

            return organizations;
        }

        public override void Remove(Organization entity)
        {
            entity.IsDeleted = true;
        }

        public override void RemoveRange(IEnumerable<Organization> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

        }

    }

    
}