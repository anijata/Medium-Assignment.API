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
    public class EmployeeRepository : GenericRepository<Employee>

    {
        public override ApplicationDbContext DbContext { get; set; }
        public EmployeeRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            DbContext = _dbContext;
        }
        public override Employee Get(int id)
        {
            var employee = DbContext.Employees
                .Include(c => c.ApplicationUser)
                .Include(c => c.City)
                .Include(c => c.State)
                .Include(c => c.Country)
                .Include(c => c.Department)
                .Include(c => c.Organization)
                .Where(c => !c.IsDeleted && c.Id == id)
                .SingleOrDefault();

            return employee;
        }
        public override IEnumerable<Employee> List()
        {
            var employees = DbContext.Employees
                .Include(c => c.ApplicationUser)
                .Include(c => c.City)
                .Include(c => c.State)
                .Include(c => c.Country)
                .Include(c => c.Department)
                .Include(c => c.Organization)
                .Where(c => !c.IsDeleted)
                .ToList();

            return employees;
        }

        public override IEnumerable<Employee> List(Expression<Func<Employee, bool>> predicate)
        {
            var employees = DbContext.Employees
                .Include(c => c.ApplicationUser)
                .Include(c => c.City)
                .Include(c => c.State)
                .Include(c => c.Country)
                .Include(c => c.Department)
                .Include(c => c.Organization)
                .Where(c => !c.IsDeleted)
                .Where(predicate)
                .ToList();

            return employees;
        }

        public override void Remove(Employee entity)
        {
            entity.IsDeleted = true;
        }

        public override void RemoveRange(IEnumerable<Employee> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }

        }

    }
}