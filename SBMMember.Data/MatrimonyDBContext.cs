using Microsoft.EntityFrameworkCore;
using SBMMember.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBMMember.Data
{
    public class MatrimonyDBContext : DbContext
    {

        public MatrimonyDBContext(DbContextOptions<MatrimonyDBContext> options) : base(options)
        {

        }
        public DbSet<SBMSubscriptionCharges> SubscriptionCharges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SBMSubscriptionCharges>().ToTable("App_SubscriptionCharges");
        }
    }
}
