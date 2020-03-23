using InfectionWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfectionWebApp.Data
{
    public class ProjectionDataContext : DbContext
    {
        public DbSet<Projection> Projections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./projection.db");
        }
    }
}
