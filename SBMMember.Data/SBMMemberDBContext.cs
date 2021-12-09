using Microsoft.EntityFrameworkCore;
using System;

namespace SBMMember.Data
{
    public class SBMMemberDBContext:DbContext
    {

        public SBMMemberDBContext(DbContextOptions<SBMMemberDBContext> options) : base(options)
        {

        }
    }
}
