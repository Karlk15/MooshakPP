namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationIsComplaining : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UsersInCourses", "RoleID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsersInCourses", "RoleID", c => c.Int(nullable: false));
        }
    }
}
