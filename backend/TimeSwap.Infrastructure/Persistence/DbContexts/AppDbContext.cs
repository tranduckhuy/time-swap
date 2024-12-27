using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;

namespace TimeSwap.Infrastructure.Persistence.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<JobApplicant> JobApplicants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewCriteria> ReviewsCriteria { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<SubscriptionHistory> SubscriptionHistories { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationReply> ConversationReplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // JobPost entity
            modelBuilder.Entity<JobPost>(entity =>
            {
                entity.Property(j => j.Title).HasMaxLength(255).IsRequired();
                entity.Property(j => j.Description).IsRequired();
                entity.Property(j => j.Fee).HasColumnType("decimal(18, 2)");
                entity.Property(j => j.DueDate).IsRequired();
            });


            // UserProfile entity
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.Property(u => u.CurrentSubscription).HasMaxLength(50);
                entity.Property(u => u.Balance).HasColumnType("decimal(18, 2)");
                entity.Property(u => u.AvatarUrl).HasMaxLength(255);
            });

            // Location entity
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(l => l.Street).HasMaxLength(255);
                entity.Property(l => l.Ward).HasMaxLength(100);
                entity.Property(l => l.District).HasMaxLength(100);
                entity.Property(l => l.City).HasMaxLength(100);
            });

            // Payment entity
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.TransactionId).HasMaxLength(255);
                entity.Property(p => p.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.ExpiryDate).IsRequired();
            });

            // Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(r => r.CategoryName).HasMaxLength(255);
            });

            // Industry entity
            modelBuilder.Entity<Industry>(entity =>
            {
                entity.Property(r => r.IndustryName).HasMaxLength(255);
            });
        }
    }
}
