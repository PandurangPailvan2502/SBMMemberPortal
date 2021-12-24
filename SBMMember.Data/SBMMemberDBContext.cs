using Microsoft.EntityFrameworkCore;
using System;
using SBMMember.Models;
namespace SBMMember.Data
{
    public class SBMMemberDBContext : DbContext
    {

        public SBMMemberDBContext(DbContextOptions<SBMMemberDBContext> options) : base(options)
        {

        }

        public DbSet<Members> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Members>().ToTable("Members");
        }
    }
}
