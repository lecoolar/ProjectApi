using Microsoft.EntityFrameworkCore;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Context
{
    public class ProjectTasksContext : DbContext
    {
        public DbSet<ProjectItem> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        public ProjectTasksContext(DbContextOptions<ProjectTasksContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<ProjectItem>()
                .HasMany<TaskItem>(p => p.TaskItems)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        }
    }
}
