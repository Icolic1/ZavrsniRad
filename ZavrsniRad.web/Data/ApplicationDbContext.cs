using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZavrsniRad.Domain.Entities;

namespace ZavrsniRad.web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet entiteta iz domena
        public DbSet<Professor> Professors { get; set; } = default!;
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Thesis> Theses { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //
            // PROFESSOR konfiguracija
            //
            builder.Entity<Professor>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.FirstName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(p => p.LastName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasMany(p => p.Students)
                      .WithOne(s => s.Mentor)
                      .HasForeignKey(s => s.MentorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Theses)
                      .WithOne(t => t.Mentor)
                      .HasForeignKey(t => t.MentorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //
            // STUDENT konfiguracija
            //
            builder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Jmbag)
                      .HasMaxLength(20);

                entity.Property(s => s.StudyProgram)
                      .HasMaxLength(200);

                entity.HasOne(s => s.Mentor)
                      .WithMany(p => p.Students)
                      .HasForeignKey(s => s.MentorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //
            // THESIS konfiguracija
            //
            builder.Entity<Thesis>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TitleHr)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(t => t.TitleEn)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.HasOne(t => t.Student)
                      .WithMany(s => s.Theses)
                      .HasForeignKey(t => t.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Mentor)
                      .WithMany(p => p.Theses)
                      .HasForeignKey(t => t.MentorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
