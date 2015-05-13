namespace UNETI.FIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fit2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeTai", "Goal", c => c.String());
            AddColumn("dbo.DeTai", "TotalModules", c => c.Int(nullable: false));
            DropColumn("dbo.DeTai", "Require");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeTai", "Require", c => c.String());
            DropColumn("dbo.DeTai", "TotalModules");
            DropColumn("dbo.DeTai", "Goal");
        }
    }
}
