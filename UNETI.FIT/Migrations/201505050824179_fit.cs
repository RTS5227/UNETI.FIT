namespace UNETI.FIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CreateAt = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        IsReaded = c.Boolean(nullable: false),
                        UserProfileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileID)
                .Index(t => t.UserProfileID);
            
            AddColumn("dbo.Confirms", "Content", c => c.Int(nullable: false));
            DropColumn("dbo.Confirms", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Confirms", "IsConfirmed", c => c.Boolean(nullable: false));
            DropIndex("dbo.Messages", new[] { "UserProfileID" });
            DropForeignKey("dbo.Messages", "UserProfileID", "dbo.UserProfile");
            DropColumn("dbo.Confirms", "Content");
            DropTable("dbo.Messages");
        }
    }
}
