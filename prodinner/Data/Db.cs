using System;
using System.Data.Entity;
using Omu.ProDinner.Core.Model;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;

namespace Omu.ProDinner.Data
{
    public class Db : DbContext
    {
        public Db() : base(GetSqlConnString())
        {

            Database.SetInitializer<Db>(null);
        }

        public static string GetSqlConnString()
        {

            string resolvedConnString = null;

            if (string.Equals(Environment.MachineName, "KaZet_Pc", StringComparison.CurrentCultureIgnoreCase))
                resolvedConnString = ConfigurationManager.ConnectionStrings["DbHome"].ConnectionString;
            else
                resolvedConnString = ConfigurationManager.ConnectionStrings["DbOffice"].ConnectionString;

            return resolvedConnString;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dinner>().HasMany(r => r.Meals).WithMany(o => o.Dinners).Map(f =>
            {
                f.MapLeftKey("DinnerId");
                f.MapRightKey("MealId");
            });

            modelBuilder.Entity<User>().HasMany(r => r.Roles).WithMany(o => o.Users).Map(f =>
            {
                f.MapLeftKey("UserId");
                f.MapRightKey("RoleId");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}