namespace MooshakPP.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedStartDateToAssignment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "startDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "startDate");
        }
    }
}
