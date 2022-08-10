using Microsoft.EntityFrameworkCore;

namespace UFC_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UFC> fighters { get; set; }
    }
}
