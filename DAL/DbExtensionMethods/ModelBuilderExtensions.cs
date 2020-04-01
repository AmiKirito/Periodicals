using DAL.ModelsEntities;
using System.Data.Entity;

namespace DAL.DbExtensionMethods
{
    /// <summary>
    /// Class that is responsible for setting some EF models dependencies in the database
    /// </summary>
    public static class ModelBuilderExtensions
    {
        public static void Seed(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublisherEntity>()
                .HasMany(p => p.Authors)
                .WithMany(a => a.Publishers)
                .Map(m =>
                {
                    m.MapLeftKey("PublisherId");
                    m.MapRightKey("AuthorId");
                    m.ToTable("PublisherAuthors");
                });
            modelBuilder.Entity<PublisherEntity>()
                .HasMany(p => p.Topics)
                .WithMany(t => t.Publishers)
                .Map(m =>
                {
                    m.MapLeftKey("PublisherId");
                    m.MapRightKey("TopicId");
                    m.ToTable("PublisherTopics");
                });
            modelBuilder.Entity<PublisherEntity>()
                .HasMany(p => p.Subscriptions)
                .WithRequired(s => s.Publisher)
                .HasForeignKey(s => s.PublisherId);


            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Subscriptions)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId);
        }
    }
}
