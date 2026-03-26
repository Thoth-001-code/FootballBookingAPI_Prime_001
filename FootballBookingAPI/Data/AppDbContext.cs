namespace FootballBookingAPI.Data
{
    using FootballBookingAPI.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldSchedule> FieldSchedules { get; set; }
        public DbSet<FieldImage> FieldImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ================= ENUM → STRING =================
            builder.Entity<ApplicationUser>()
                .Property(u => u.Status)
                .HasConversion<string>();

            builder.Entity<Field>()
                .Property(f => f.Status)
                .HasConversion<string>();

            builder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();

            builder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();

            // ================= FIELD =================
            builder.Entity<Field>()
                .HasOne(f => f.Owner)
                .WithMany(u => u.Fields)
                .HasForeignKey(f => f.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ================= BOOKING =================
            builder.Entity<Booking>()
                .HasIndex(b => new { b.FieldId, b.StartTime, b.EndTime });

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Field)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FieldId);

            // ================= PAYMENT (1-1) =================
            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            // ================= REVIEW =================
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder.Entity<Review>()
                .HasOne(r => r.Field)
                .WithMany(f => f.Reviews)
                .HasForeignKey(r => r.FieldId);

            // ================= FAVORITE =================
            builder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.FieldId })
                .IsUnique();

            builder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            builder.Entity<Favorite>()
                .HasOne(f => f.Field)
                .WithMany(f => f.Favorites)
                .HasForeignKey(f => f.FieldId);

            // ================= IMAGE =================
            builder.Entity<FieldImage>()
                .HasOne(i => i.Field)
                .WithMany(f => f.Images)
                .HasForeignKey(i => i.FieldId);

            // ================= SCHEDULE =================
            builder.Entity<FieldSchedule>()
                .HasOne(s => s.Field)
                .WithMany(f => f.Schedules)
                .HasForeignKey(s => s.FieldId);
        }
    }
}
