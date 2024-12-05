using Application.DTO;
using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class HogwartsContext: DbContext
{
 
    public HogwartsContext(DbContextOptions<HogwartsContext> options) : base(options) {}
    
    public DbSet<Shift> Shift { get; init; }
    public DbSet<Students> Students { get; init; }
    public DbSet<Tasks> Tasks { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map Students
        modelBuilder.Entity<Students>(entity =>
        {
            entity.ToTable("students");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(36);

            entity.Property(e => e.SalaryRate)
                .HasColumnName("salary_rate");

            entity.Property(e => e.ExtraSalary)
                .HasColumnName("after_17_extra_salary_rate");

            entity.Property(e => e.House)
                .HasColumnName("house")
                .HasMaxLength(64);

            entity.Property(e => e.Tasks)
                .HasColumnName("tasks")
                .HasMaxLength(128);
        });
        
        modelBuilder.Entity<Shift>(entity =>
        {
            entity.ToTable("shift");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(36);

            entity.Property(e => e.StartTime)
                .HasColumnName("start_time");

            entity.Property(e => e.EndTime)
                .HasColumnName("ent_time");

            entity.Property(e => e.UnpaidBreak)
                .HasColumnName("unpaid_break");

            entity.Property(e => e.BillableRate)
                .HasColumnName("billable_rate");

            
            entity.Property(e => e.AppointedById)
                .HasColumnName("appointed_by") 
                .HasMaxLength(36)
                .IsRequired(false);

            entity.HasOne(e => e.AppointedBy)
                .WithMany(s => s.ShiftSet)
                .HasForeignKey(e => e.AppointedById)
                .HasConstraintName("fk_shift_students_id")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        });
        
        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.ToTable("tasks");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(36);
            
            entity.Property(e => e.StudentsId)
                .HasColumnName("students_id")
                .HasMaxLength(36);
            
            entity.Property(e => e.Task)
                .HasColumnName("task")
                .HasMaxLength(128);
            
            entity.Property(e => e.Weight)
                .HasColumnName("weight");

            entity.HasOne(e => e.StudentsBy)
                .WithMany(s => s.TasksSet)
                .HasForeignKey(e => e.StudentsId)
                .HasConstraintName("fk_students_tasks_id");
        });
    }
}