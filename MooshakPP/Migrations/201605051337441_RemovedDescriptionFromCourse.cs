namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDescriptionFromCourse : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "description", c => c.String());
        }
    }
}
