using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RMISDemo.API.Models
{
    public partial class RmisDbContext : DbContext
    {
        public RmisDbContext()
        {
        }

        public RmisDbContext(DbContextOptions<RmisDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<ApiUser> ApiUser { get; set; }
        public virtual DbSet<BlogPost> BlogPost { get; set; }
        public virtual DbSet<BlogPostUrl> BlogPostUrl { get; set; }
        public virtual DbSet<BlogPostUrlCategory> BlogPostUrlCategory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer("...");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiUser>(entity =>
            {
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PostContent)
                    .IsRequired()
                    .HasMaxLength(4096)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BlogPostUrl>(entity =>
            {
                entity.Property(e => e.UrlPath)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.UrlTypeId)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.BlogPost)
                    .WithMany(p => p.BlogPostUrl)
                    .HasForeignKey(d => d.BlogPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogPostUrl_BlogPost");

                entity.HasOne(d => d.UrlType)
                    .WithMany(p => p.BlogPostUrl)
                    .HasForeignKey(d => d.UrlTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogPostUrl_BlogPostUrlCategory");
            });

            modelBuilder.Entity<BlogPostUrlCategory>(entity =>
            {
                entity.HasKey(e => e.UrlTypeId)
                    .HasName("PK_BlogPostUrlType");

                entity.Property(e => e.UrlTypeId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UrlTypeName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
