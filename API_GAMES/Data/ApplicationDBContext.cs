﻿using API_GAMES.Model;
using Azure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace API_GAMES.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Game> Games { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
            .HasMany(e => e.Users)
            .WithMany(e => e.GamesU);

            modelBuilder.Entity<ApplicationUser>()
           .HasMany(e => e.GamesU)
           .WithMany(e => e.Users);
        }
    }
}
