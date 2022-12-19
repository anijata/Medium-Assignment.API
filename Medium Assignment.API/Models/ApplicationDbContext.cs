using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;


namespace Medium_Assignment.API.Models
{
    public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var seedData = new SeedData(context);
            seedData.SeedCountries();
            seedData.SeedStates();
            seedData.SeedCities();
            seedData.SeedRoles();
            seedData.SeedUsers();
            seedData.SeedReviewStatuses();
            seedData.SeedOrganizations();
            seedData.SeedDepartments();
            seedData.SeedEmployees();
            seedData.SeedReviews();
            base.Seed(context);
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbContextInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewStatus> ReviewStatuses { get; set; }
        public DbSet<State> States { get; set; }

    }
}