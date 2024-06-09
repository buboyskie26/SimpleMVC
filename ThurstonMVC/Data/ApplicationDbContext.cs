using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ThurstonMVC.Models.Entities;

namespace ThurstonMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

    }
}
