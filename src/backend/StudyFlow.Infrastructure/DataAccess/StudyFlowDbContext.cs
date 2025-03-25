using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Infrastructure.DataAccess
{
    public class StudyFlowDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Topic)
                .WithMany()
                .HasForeignKey(r => r.TopicId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudyFlowDbContext).Assembly);
        }
    }
}
