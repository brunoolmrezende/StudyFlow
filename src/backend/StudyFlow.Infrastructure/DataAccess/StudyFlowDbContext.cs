using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Infrastructure.DataAccess
{
    public class StudyFlowDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudyFlowDbContext).Assembly);
        }
    }
}
