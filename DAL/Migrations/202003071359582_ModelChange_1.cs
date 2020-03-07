namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publishers", "IsRemoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "IsRemoved");
        }
    }
}
