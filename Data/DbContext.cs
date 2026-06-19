using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Trainee> Trainees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Mentor> Mentors { get; set; }
    public DbSet<LearningTask> LearningTasks { get; set; }
    public DbSet<TaskAssignment> TaskAssignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<SubmissionFile> SubmissionFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trainee>()
            .HasMany(e => e.TaskAssignments)
            .WithOne(e => e.Trainee)
            .HasForeignKey(e => e.TraineeId)
            .IsRequired();

        modelBuilder.Entity<Mentor>()
            .HasMany(e => e.TaskAssignments)
            .WithOne(e => e.Mentor)
            .HasForeignKey(e => e.MentorId)
            .IsRequired();

        modelBuilder.Entity<LearningTask>()
            .HasMany(e => e.TaskAssignments)
            .WithOne(e => e.LearningTask)
            .HasForeignKey(e => e.LearningTaskId)
            .IsRequired();
    }
}

