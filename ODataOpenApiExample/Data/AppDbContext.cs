using Microsoft.EntityFrameworkCore;
using ODataOpenApiExample.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ODataOpenApiExample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSetting>()
                .HasKey(u => new { u.UserId, u.SettingName });
        }
    }
}
