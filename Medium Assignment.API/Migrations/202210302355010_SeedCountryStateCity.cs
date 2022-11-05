namespace Medium_Assignment.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedCountryStateCity : DbMigration
    {
        public override void Up()
        {

            Sql(@"INSERT INTO dbo.Countries (Name) VALUES ('USA'), ('India'), ('Australia')");

            Sql(@"INSERT INTO dbo.States (Name, CountryId) VALUES ('California', 1), ('Ohio', 1), ('Texas', 1)");
            Sql(@"INSERT INTO dbo.States (Name, CountryId) VALUES ('Telangana', 2), ('Andhra Pradesh', 2), ('Maharashtra', 2)");
            Sql(@"INSERT INTO dbo.States (Name, CountryId) VALUES ('Victoria', 3), ('New South Wales', 3), ('Queensland', 3)");

            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Los Angeles', 1), ('San Diego', 1)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Columbus', 2), ('Cleveland', 2)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Houston', 3), ('Dallas', 3)");

            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Hyderabad', 4), ('Karimnagar', 4)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Visakhapatnam', 5), ('Tirupati', 5)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Mumbai', 6), ('Pune', 6)");

            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Melbourne', 7), ('Mildura', 7)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Sydney', 8), ('Orange', 8)");
            Sql(@"INSERT INTO dbo.Cities (Name, StateId) VALUES ('Brisbane', 9), ('Mackay', 9)");


        }

        public override void Down()
        {
        }
    }
}
