namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "IsRemoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "IsRemoved");
        }
    }
}
