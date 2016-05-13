namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTheSubmissionsEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "passCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submissions", "passCount");
        }
    }
}
