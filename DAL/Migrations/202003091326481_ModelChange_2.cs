namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "IsExpired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "IsExpired");
        }
    }
}
