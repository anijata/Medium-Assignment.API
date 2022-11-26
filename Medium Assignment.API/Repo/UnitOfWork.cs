using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Medium_Assignment.API.Models;

namespace Medium_Assignment.API.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext DbContext { get; set; }
        public OrganizationRepository Organizations { get; set; }

        public EmployeeRepository Employees { get; set; }

        public DepartmentRepository Departments { get; set; }

        public ReviewRepository Reviews { get; set; }

        public UnitOfWork(ApplicationDbContext _dbContext) {
            DbContext = _dbContext;
            Organizations = new OrganizationRepository(_dbContext);
            Employees = new EmployeeRepository(_dbContext);
            Departments = new DepartmentRepository(_dbContext);
            Reviews = new ReviewRepository(_dbContext);
        }
        public void Complete()
        {
            DbContext.SaveChanges();
        }
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}