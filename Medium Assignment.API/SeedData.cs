using Medium_Assignment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;

namespace Medium_Assignment.API
{
    public class SeedData
    {
        public ApplicationDbContext DbContext { get; set; }
        public UserStore<ApplicationUser> UserStore { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }

        public RoleStore<IdentityRole> RoleStore { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }

        public SeedData(ApplicationDbContext DbContext) {
            this.DbContext = DbContext;
            this.UserStore = new UserStore<ApplicationUser>(this.DbContext);
            this.UserManager = new UserManager<ApplicationUser>(this.UserStore);
            this.RoleStore = new RoleStore<IdentityRole>(this.DbContext);
            this.RoleManager = new RoleManager<IdentityRole>(this.RoleStore);

        }

        public void SeedRoles() {
            this.RoleManager.Create(new IdentityRole() { Name= "SuperAdmin" });
            this.RoleManager.Create(new IdentityRole() { Name= "OrganizationAdmin" });
            this.RoleManager.Create(new IdentityRole() { Name= "Employee" });

        }

        public void SeedUsers()
        {
            var user = new ApplicationUser() { UserName = "admin", Email = "admin@issq.com" };

            this.UserManager.Create(user, "$AdminPassword123");

            this.UserManager.AddToRole(user.Id, "SuperAdmin");


        }


        public void SeedOrganizations() {

            var user1 = new ApplicationUser 
            { UserName = "google",
                Email = "google@gmail.com",
                PhoneNumber = "3055283966"
            };

            UserManager.Create(user1, "$DefaultPassword123");

            UserManager.AddToRole(user1.Id, "OrganizationAdmin");

            var organization1 = new Organization
            {
                Id =1, 
                Name = "Google",
                Address1 = "7899 Thierer Pass",
                Address2 = "",
                CityId = 1,
                StateId = 1,
                CountryId = 1,
                Status = "Active",
                Description = "Google description",
                ApplicationUserId = user1.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };

            var user2 = new ApplicationUser
            {
                UserName = "meta",
                Email = "meta@email.com",
                PhoneNumber = "2842084312"
            };

            UserManager.Create(user2, "$DefaultPassword123");

            UserManager.AddToRole(user2.Id, "OrganizationAdmin");

            var organization2 = new Organization
            {
                Id = 2,
                Name = "Meta",
                Address1 = "2 Vernon Hill",
                Address2 = "",
                CityId = 1,
                StateId = 2,
                CountryId = 1,
                Status = "Active",
                Description = "Meta description",
                ApplicationUserId = user2.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };

            var user3 = new ApplicationUser
            {
                UserName = "amazon",
                Email = "amazon@email.com",
                PhoneNumber = "1704645420"
            };

            UserManager.Create(user3, "$DefaultPassword123");

            UserManager.AddToRole(user3.Id, "OrganizationAdmin");

            var organization3 = new Organization
            {
                Id = 3,
                Name = "Amazon",
                Address1 = "1708 Messerschmidt",
                Address2 = "Center",
                CityId = 1,
                StateId = 3,
                CountryId = 1,
                Status = "Active",
                Description = "Amazon description",
                ApplicationUserId = user3.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };


            DbContext.Organizations.Add(organization1);
            DbContext.Organizations.Add(organization2);
            DbContext.Organizations.Add(organization3);
            DbContext.SaveChanges();

        }

        public void SeedDepartments() {
            DbContext.Departments.Add(new Department { Id = 1, Name = "Human Resources", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 2 , Name  = "Engineering", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 3 , Name  = "Tech Support", OrganizationId = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 4 , Name  = "Marketing", OrganizationId = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 5 , Name  = "Law", OrganizationId = 3, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 6 , Name  = "Accounting", OrganizationId = 3, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });


            DbContext.SaveChanges();

        }

        public void SeedEmployees() {
            var user1 = new ApplicationUser
            {
                UserName = "jorbell0",
                Email = "jorbell0@gmail.com",
                PhoneNumber = "6195090923"
            };


            UserManager.Create(user1, "$DefaultPassword123");

            UserManager.AddToRole(user1.Id, "Employee");

            var employee1 = new Employee
            {
                Id = 1,
                FirstName = "Juline",
                LastName = "Orbell",
                Address1 = "08396 Evergreen ",
                Address2 = "Circle",
                CityId = 1,
                StateId = 2,
                CountryId = 1,
                ApplicationUserId = user1.Id,
                DepartmentId = 1,
                Designation = "Designation 1",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 1,
                EmployeeType = "Full Time",
                Gender = "Male",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            DbContext.Employees.Add(employee1);

            DbContext.SaveChanges();
        }

        public void SeedReviews()
        {

        }


        public void SeedCountries() {
            DbContext.Countries.Add(new Country { Id = 1, Name = "USA" });
            DbContext.Countries.Add(new Country { Id = 2, Name = "India" });
            DbContext.Countries.Add(new Country { Id = 3, Name = "Australia" });
        
            DbContext.SaveChanges();
        }

        public void SeedStates()
        {
            DbContext.States.Add(new State { Id = 1, Name = "California", CountryId = 1 });
            DbContext.States.Add(new State { Id = 2, Name = "Ohio", CountryId = 1 });
            DbContext.States.Add(new State { Id = 3, Name = "Texas", CountryId = 1 });
            DbContext.States.Add(new State { Id = 4, Name = "Telangana", CountryId = 2 });
            DbContext.States.Add(new State { Id = 5, Name = "Andhra Pradesh", CountryId = 2 });
            DbContext.States.Add(new State { Id = 6, Name = "Maharashtra", CountryId = 2 });
            DbContext.States.Add(new State { Id = 7, Name = "Victoria", CountryId = 3 });
            DbContext.States.Add(new State { Id = 8, Name = "New South Wales", CountryId = 3 });
            DbContext.States.Add(new State { Id = 9, Name = "Queensland", CountryId = 3 });

            DbContext.SaveChanges();
        }

        public void SeedCities()
        {
            DbContext.Cities.Add(new City { Id = 1, Name = "Los Angeles", StateId = 1 });
            DbContext.Cities.Add(new City { Id = 2, Name = "San Diego", StateId = 1 });
            DbContext.Cities.Add(new City { Id = 3, Name = "Columbus", StateId = 2 });
            DbContext.Cities.Add(new City { Id = 4, Name = "Cleveland", StateId = 2 });
            DbContext.Cities.Add(new City { Id = 5, Name = "Houston", StateId = 3 });
            DbContext.Cities.Add(new City { Id = 6, Name = "Dallas", StateId = 3 });
            DbContext.Cities.Add(new City { Id = 7, Name = "Hyderabad", StateId = 4 });
            DbContext.Cities.Add(new City { Id = 8, Name = "Karimnagar", StateId = 4 });
            DbContext.Cities.Add(new City { Id = 9, Name = "Visakhapatnam", StateId = 5 });
            DbContext.Cities.Add(new City { Id = 10, Name = "Tirupati", StateId = 5 });
            DbContext.Cities.Add(new City { Id = 11, Name = "Mumbai", StateId = 6 });
            DbContext.Cities.Add(new City { Id = 12, Name = "Pune", StateId = 6 });
            DbContext.Cities.Add(new City { Id = 13, Name = "Melbourne", StateId = 7 });
            DbContext.Cities.Add(new City { Id = 14, Name = "Mildura", StateId = 7 });
            DbContext.Cities.Add(new City { Id = 15, Name = "Sydney", StateId = 8 });
            DbContext.Cities.Add(new City { Id = 16, Name = "Orange", StateId = 8 });
            DbContext.Cities.Add(new City { Id = 17, Name = "Brisbane", StateId = 9 });
            DbContext.Cities.Add(new City { Id = 18, Name = "Mackay", StateId = 9 });

            DbContext.SaveChanges();
        }
    }
}