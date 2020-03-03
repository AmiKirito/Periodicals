using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DbExtensionMethods
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Authors)
                .WithMany(a => a.Publishers)
                .Map(m =>
                {
                    m.MapLeftKey("PublisherId");
                    m.MapRightKey("AuthorId");
                    m.ToTable("PublisherAuthors");
                });
            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Topics)
                .WithMany(t => t.Publishers)
                .Map(m =>
                {
                    m.MapLeftKey("PublisherId");
                    m.MapRightKey("TopicId");
                    m.ToTable("PublisherTopics");
                });
            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Subscriptions)
                .WithRequired(s => s.Publisher)
                .HasForeignKey(s => s.PublisherId);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Subscriptions)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId);
        }
    }
}
