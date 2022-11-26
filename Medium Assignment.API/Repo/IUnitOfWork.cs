using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Medium_Assignment.API.Models;

namespace Medium_Assignment.API.Repo
{
    public interface IUnitOfWork : IDisposable
    {

        ApplicationDbContext DbContext { get; set; }
        OrganizationRepository Organizations { get; set; }

        EmployeeRepository Employees { get; set; }

        ReviewRepository Reviews { get; set; }

        void Complete();
    }
}