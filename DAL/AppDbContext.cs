using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using DAL.DbExtensionMethods;
using DAL.ModelsEntities;

namespace DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext() : base("PeriodicalsDB")
        {
            Database.SetInitializer(new DbContextInizializer());
        }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<PublisherEntity> Publishers { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<TopicEntity> Topics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
