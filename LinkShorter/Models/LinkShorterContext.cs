using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LinkShorter.Models
{
    public partial class LinkShorterContext : DbContext
    {
        IConfiguration _configuration;
        public LinkShorterContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LinkShorterContext(DbContextOptions<LinkShorterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Link"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Link>(entity =>
            {
                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrginalUrl).IsRequired();

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.ShortUrl)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Browser)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("IP");

                entity.Property(e => e.Os)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("OS");

                entity.Property(e => e.Device)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("Device");

                entity.Property(e => e.VisitedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Link)
                    .WithMany()
                    .HasForeignKey(d => d.LinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reports_Reports");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
