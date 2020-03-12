namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publishers", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "ImagePath");
        }
    }
}
