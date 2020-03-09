namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publishers", "MonthlySubscriptionPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "MonthlySubscriptionPrice");
        }
    }
}
