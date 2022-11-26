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
    public class DepartmentRepository : GenericRepository<Department>

    {
        public override ApplicationDbContext DbContext { get; set; }
        public DepartmentRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            DbContext = _dbContext;
        }

        public override Department Get(int id)
        {
            var department = DbContext.Departments
                .Where(c => !c.IsDeleted && c.Id == id)
                .SingleOrDefault();

            return department;
        }

        public override IEnumerable<Department> List()
        {
            var departments = DbContext.Departments
                .Where(c => !c.IsDeleted)
                .ToList();

            return departments;
        }

        public override IEnumerable<Department> List(Expression<Func<Department, bool>> predicate)
        {
            var departments = DbContext.Departments
                  .Where(c => !c.IsDeleted)
                  .Where(predicate)
                  .ToList();

            return departments;
        }


    }
}