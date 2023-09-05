using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenAuthor.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenAuthor.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        // DbSet is a collection of entities that can be queried from the database
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}