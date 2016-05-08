namespace MooshakPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserIdInSubmissionEntityToAString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Submissions", "userID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Submissions", "userID", c => c.Int(nullable: false));
        }
    }
}
