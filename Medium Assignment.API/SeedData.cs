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

        public void SeedCountries()
        {
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

        public void SeedReviewStatuses() {
            DbContext.ReviewStatuses.Add(new ReviewStatus { Id = 1, Name = "New"});
            DbContext.ReviewStatuses.Add(new ReviewStatus { Id = 2, Name = "Assigned"});
            DbContext.ReviewStatuses.Add(new ReviewStatus { Id = 3, Name = "Submitted"});

            DbContext.SaveChanges();
        }

        public void CreateOrganization(Organization organization) {
            UserManager.Create(organization.ApplicationUser, "$DefaultPassword123");

            UserManager.AddToRole(organization.ApplicationUser.Id, "OrganizationAdmin");

            DbContext.Organizations.Add(organization);

            DbContext.SaveChanges();
        }

        public void SeedOrganizations() {

            var organization1 = new Organization
            {
                Id =1, 
                Name = "Google",
                Address1 = "7899 Thierer Pass",
                Address2 = "",
                CityId = 1,
                StateId = 1,
                CountryId = 1,
                ApplicationUser =new ApplicationUser
                {
                    UserName = "google",
                    Email = "google@gmail.com",
                    PhoneNumber = "3055283966"
                },
                Status = "Active",
                Description = "Google description",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };

            CreateOrganization(organization1);

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
                ApplicationUser = new ApplicationUser
                {
                    UserName = "meta",
                    Email = "meta@email.com",
                    PhoneNumber = "2842084312"
                },
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };

            CreateOrganization(organization2);

            var organization3 = new Organization
            {
                Id = 3,
                Name = "Amazon",
                Address1 = "1708 Messerschmidt",
                Address2 = "Center",
                CityId = 3,
                StateId = 2,
                CountryId = 1,
                Status = "Active",
                Description = "Amazon description",
                ApplicationUser = new ApplicationUser
                {
                    UserName = "amazon",
                    Email = "amazon@email.com",
                    PhoneNumber = "1704645420"
                },
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,

            };

            CreateOrganization(organization3);


        }

        public void SeedDepartments() {
            DbContext.Departments.Add(new Department { Id = 1, Name = "Human Resources", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 2 , Name  = "Engineering", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 3 , Name  = "Tech Support", OrganizationId = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 4 , Name  = "Marketing", OrganizationId = 2, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 5 , Name  = "Marketing", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 6 , Name  = "Law", OrganizationId = 3, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 7 , Name  = "Accounting", OrganizationId = 3, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 8 , Name  = "Accounting", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
            DbContext.Departments.Add(new Department { Id = 9 , Name  = "Training", OrganizationId = 1, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });


            DbContext.SaveChanges();

        }

        public void CreateEmployee(Employee employee) {
            UserManager.Create(employee.ApplicationUser, "$DefaultPassword123");

            UserManager.AddToRole(employee.ApplicationUser.Id, "Employee");

            DbContext.Employees.Add(employee);

            
            DbContext.SaveChanges();

            
        }

        public void SeedEmployees() {

            var employee1 = new Employee
            {
                Id = 1,
                FirstName = "Juline",
                LastName = "Orbell",
                Address1 = "08396 Evergreen ",
                Address2 = "Circle",
                CityId = 3,
                StateId = 2,
                CountryId = 1,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "jorbell0",
                    Email = "jorbell0@gmail.com",
                    PhoneNumber = "6195090923"
                },
                DepartmentId = 1,
                Designation = "Human resources manager",
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

            CreateEmployee(employee1);

            var employee2 = new Employee
            {
                Id = 2,
                FirstName = "Daniella",
                LastName = "Turn",
                Address1 = "6885 Declaration Crossing",
                Address2 = "",
                CityId = 13,
                StateId = 7,
                CountryId = 3,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "dturn0",
                    Email = "dturn0@gmail.com",
                    PhoneNumber = "5794531359"
                },
                DepartmentId = 9,
                Designation = "Training manager",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 1,
                EmployeeType = "Full Time",
                Gender = "Female",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            CreateEmployee(employee2);

            var employee3 = new Employee
            {
                Id = 3,
                FirstName = "Martainn",
                LastName = "Borthe",
                Address1 = "6 Cody Point",
                Address2 = "",
                CityId = 10,
                StateId = 5,
                CountryId = 2,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "mborthe0",
                    Email = "mborthe0@gmail.com",
                    PhoneNumber = "2026847418"
                },
                DepartmentId = 5,
                Designation = "VP Marketing",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 1,
                EmployeeType = "Full Time",
                Gender = "Female",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            CreateEmployee(employee3);

            var employee4 = new Employee
            {
                Id = 4,
                FirstName = "Mellie",
                LastName = "Masson",
                Address1 = "5 Blackbird Crossing",
                Address2 = "",
                CityId = 6,
                StateId = 3,
                CountryId = 1,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "mmasson2",
                    Email = "mmasson2@gmail.com",
                    PhoneNumber = "1172007054"
                },
                DepartmentId = 8,
                Designation = "Staff Accountant",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 1,
                EmployeeType = "Contract",
                Gender = "Male",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            CreateEmployee(employee4);

            var employee5 = new Employee
            {
                Id = 5,
                FirstName = "Arturo",
                LastName = "Stailey",
                Address1 = "6 Di Loreto Avenue",
                Address2 = "",
                CityId = 14,
                StateId = 7,
                CountryId = 3,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "astailey6",
                    Email = "astailey6@gmail.com",
                    PhoneNumber = "9045478352"
                },
                DepartmentId = 3,
                Designation = "Tech Support member",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 2,
                EmployeeType = "Contract",
                Gender = "Male",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            CreateEmployee(employee5);

            var employee6 = new Employee
            {
                Id = 6,
                FirstName = "Gusty",
                LastName = "Chinnick",
                Address1 = "5 Blackbird Crossing",
                Address2 = "",
                CityId = 6,
                StateId = 3,
                CountryId = 1,
                ApplicationUser = new ApplicationUser
                {
                    UserName = "gchinnick9",
                    Email = "gchinnick9@gmail.com",
                    PhoneNumber = "3018244345"
                },
                DepartmentId = 6,
                Designation = "Law Consultant",
                DOB = DateTime.Now,
                DOJ = DateTime.Now,
                Status = "Active",
                OrganizationId = 3,
                EmployeeType = "Full Time",
                Gender = "Female",
                CreatedBy = "",
                CreatedOn = DateTime.Now,
                ModifiedBy = "",
                ModifiedOn = DateTime.Now
            };

            CreateEmployee(employee6);
        }

        public void SeedReviews()
        {

        }
        
    }
}