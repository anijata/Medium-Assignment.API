namespace Medium_Assignment.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedData : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'ae6b19b1-940e-4259-8fbd-3461d5687f3a', N'admin@issq.com', 0, N'ACJmIDw7YlFmSdptGQrRWTC0G2d3K6IFRfvVh0++BznBkxPh0QTMBd9g5pDa0ND8jQ==', N'4e3015e5-89d4-4556-9240-57cdf7f2529d', NULL, 0, 0, NULL, 1, 0, N'admin')");
            Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'b963108b-72ed-42ef-af16-d1c42279fccf', N'Employee')");
            Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8e86a172-39b4-458f-9cff-255257e92494', N'OrganizationAdmin')");
            Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'15f800bc-2d22-4178-8150-fa005b65dc87', N'SuperAdmin')");
            Sql(@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ae6b19b1-940e-4259-8fbd-3461d5687f3a', N'15f800bc-2d22-4178-8150-fa005b65dc87')");

        }

        public override void Down()
        {
        }
    }
}
