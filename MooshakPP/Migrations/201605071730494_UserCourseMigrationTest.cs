namespace MooshakPP.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UserCourseMigrationTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersInCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userID = c.String(maxLength: 128),
                        courseID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.courseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.userID)
                .Index(t => t.userID)
                .Index(t => t.courseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersInCourses", "userID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersInCourses", "courseID", "dbo.Courses");
            DropIndex("dbo.UsersInCourses", new[] { "courseID" });
            DropIndex("dbo.UsersInCourses", new[] { "userID" });
            DropTable("dbo.UsersInCourses");
        }
    }
}
