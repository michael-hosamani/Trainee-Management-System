using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Data;
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
    public DbSet<ProcessingJob> ProcessingJobs { get; set; }

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

        modelBuilder.Entity<Submission>()
            .HasOne(s => s.TaskAssignment)
            .WithMany(ta => ta.Submissions)
            .HasForeignKey(s => s.TaskAssignmentId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Submission)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.SubmissionId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Mentor)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.MentorId);

        modelBuilder.Entity<SubmissionFile>()
            .HasOne(sf => sf.Submission)
            .WithMany(s => s.SubmissionFiles)
            .HasForeignKey(sf => sf.SubmissionId);
    }
}

