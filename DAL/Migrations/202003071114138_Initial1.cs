namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publishers", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "Description");
        }
    }
}
