namespace UNETI.FIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fit1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "UserProfileID", "dbo.UserProfile");
            DropIndex("dbo.Messages", new[] { "UserProfileID" });
            AddColumn("dbo.Messages", "TeacherID", c => c.String(maxLength: 50));
            AddColumn("dbo.Messages", "StudentID", c => c.String(maxLength: 50));
            AddColumn("dbo.Students", "Email", c => c.String(maxLength: 100));
            AddColumn("dbo.Confirms", "CreateAt", c => c.DateTime(nullable: false));
            AddForeignKey("dbo.Messages", "TeacherID", "dbo.Teachers", "ID");
            AddForeignKey("dbo.Messages", "StudentID", "dbo.Students", "ID");
            CreateIndex("dbo.Messages", "TeacherID");
            CreateIndex("dbo.Messages", "StudentID");
            DropColumn("dbo.Messages", "CreateBy");
            DropColumn("dbo.Messages", "UserProfileID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "UserProfileID", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "CreateBy", c => c.String());
            DropIndex("dbo.Messages", new[] { "StudentID" });
            DropIndex("dbo.Messages", new[] { "TeacherID" });
            DropForeignKey("dbo.Messages", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Messages", "TeacherID", "dbo.Teachers");
            DropColumn("dbo.Confirms", "CreateAt");
            DropColumn("dbo.Students", "Email");
            DropColumn("dbo.Messages", "StudentID");
            DropColumn("dbo.Messages", "TeacherID");
            CreateIndex("dbo.Messages", "UserProfileID");
            AddForeignKey("dbo.Messages", "UserProfileID", "dbo.UserProfile", "ID", cascadeDelete: true);
        }
    }
}
