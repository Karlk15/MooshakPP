namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMilestonePropLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Milestones", "language", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Milestones", "language");
        }
    }
}
