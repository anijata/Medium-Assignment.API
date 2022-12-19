namespace Medium_Assignment.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeAddress2Changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Address2", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Address2", c => c.String(nullable: false));
        }
    }
}
