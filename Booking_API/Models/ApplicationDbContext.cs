using Booking_API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Booking_API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
