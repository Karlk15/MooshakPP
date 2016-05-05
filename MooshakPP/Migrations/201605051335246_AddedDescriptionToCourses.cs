namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescriptionToCourses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "description");
        }
    }
}
