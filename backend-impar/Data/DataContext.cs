using backend_impar.Models;
using Microsoft.EntityFrameworkCore;
namespace backend_impar.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Car> cars { get; set; }

        public DbSet<Photo> photos { get; set; }
    }
}
