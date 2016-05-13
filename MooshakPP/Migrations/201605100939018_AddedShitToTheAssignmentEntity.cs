namespace MooshakPP.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedShitToTheAssignmentEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "isDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assignments", "teacherID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "teacherID");
            DropColumn("dbo.Assignments", "isDeleted");
        }
    }
}
