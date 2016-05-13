namespace MooshakPP.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedEnumResultToSubmissionsEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Submissions", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Submissions", "status");
        }
    }
}
