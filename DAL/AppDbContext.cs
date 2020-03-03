using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DbExtensionMethods;

namespace DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext() : base("PeriodicalsDB")
        {
            Database.SetInitializer(new DbContextInizializer());
        }
        public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
