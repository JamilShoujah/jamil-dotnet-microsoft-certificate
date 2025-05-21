// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using JamilDotnetMicrosoftCertificate.Models;

namespace JamilDotnetMicrosoftCertificate.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
    }
}
