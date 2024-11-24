using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Book_Hub_Web_API.Data.BookHubDBContext;
using static Book_Hub_Web_API.Models.Borrowed;

namespace Book_Hub_Web_API.Data
{
    public class BookHubDBContext : DbContext
    {
        public BookHubDBContext() { }
        public BookHubDBContext(DbContextOptions<BookHubDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Book Model 
            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(x => x.BookId);

                // Book-Genre (M-1)
                entity.HasOne(g => g.Genres)
                .WithMany(g => g.Books)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.Property(b => b.Cost)
                .HasColumnType("decimal(10, 2)");

                entity.Property(b => b.PublishedDate)
                .HasColumnType("date")
                .HasConversion(
                     v => v.ToDateTime(new TimeOnly(0, 0)),
                     v => DateOnly.FromDateTime(v)
                );
            });

            // Borrowed Model
            modelBuilder.Entity<Borrowed>(entity =>
            {
                entity.HasKey(x => x.BorrowId);

                // Borrowed-Book (M-1)
                entity.HasOne(b => b.Book)
                .WithMany(b => b.Borrowed)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

                // Borrowed-User (M-1)
                entity.HasOne(u => u.User)
                .WithMany(b => b.Borrowed)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.Property(b => b.BorrowDate)
                .HasColumnType("date")
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v)
                );

                entity.Property(b => b.ReturnDeadline)
                .HasColumnType("date")
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v)
                );

                entity.Property(b => b.ReturnDate)
                .HasColumnType("date")
                .IsRequired(false)  // Allow null values in the database
                .HasConversion(
                    // Converting Model object to Database record
                    v => v.HasValue ? v.Value.ToDateTime(new TimeOnly(0, 0)) : (DateTime?)null,  // Handle nullable DateOnly

                    // Converting Database record to Model object
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null  // Handle nullable DateTime
                );

                // Borrow Status conversion from Enum to string
                entity.Property(b => b.BorrowStatus)
                .HasConversion<string>();
            });


            // Fines Model
            modelBuilder.Entity<Fines>(entity =>
            {
                entity.HasKey(x => x.FineId);

                entity.Property(f => f.FineAmount)
                .HasColumnType("decimal(10, 2)");

                // Fines-Borrowed (M-1)
                entity.HasOne(b => b.Borrowed)
                .WithMany(f => f.Fines)
                .HasForeignKey(f => f.BorrowId)
                .OnDelete(DeleteBehavior.Cascade);

                // Fine Type conversion from Enum to string
                entity.Property(f => f.FineType)
                .HasConversion<string>();

                // Fine Paid Status conversion from Enum to string
                entity.Property(f => f.FinePaidStatus)
                .HasConversion<string>();

                entity.Property(f => f.PaidDate)
                .HasColumnType("date")
                .IsRequired(false)  // Allow null values in the database
                .HasConversion(
                    // Converting Model object to Database record
                    v => v.HasValue ? v.Value.ToDateTime(new TimeOnly(0, 0)) : (DateTime?)null,  // Handle nullable DateOnly

                    // Converting Database record to Model object
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null  // Handle nullable DateTime
                );
            });

            // Genres Model
            modelBuilder.Entity<Genres>(entity =>
            {
                entity.HasKey(x => x.GenreId);
            });

            // Notifications Model
            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(n => n.NotificationId);

                entity.Property(u => u.SentDate)
                .HasColumnType("date")
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v)
                );

                // Message Type conversion from Enum to string
                entity.Property(u => u.MessageType)
                .HasConversion<string>();

                // Notifications-Users (M-1)
                entity.HasOne(u => u.Users)
                .WithMany(n => n.Notifications)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });


            // Reservations Model
            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.HasKey(r => r.ReservationId);

                entity.Property(b => b.ExpectedAvailabilityDate)
                .HasColumnType("date")
                .IsRequired(false)  // Allow null values in the database
                .HasConversion(
                    // Converting Model object to Database record
                    v => v.HasValue ? v.Value.ToDateTime(new TimeOnly(0, 0)) : (DateTime?)null,  // Handle nullable DateOnly

                    // Converting Database record to Model object
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null  // Handle nullable DateTime
                );

                entity.Property(b => b.ReservationExpiryDate)
                .HasColumnType("date")
                .IsRequired(false)  // Allow null values in the database
                .HasConversion(
                    // Converting Model object to Database record
                    v => v.HasValue ? v.Value.ToDateTime(new TimeOnly(0, 0)) : (DateTime?)null,  // Handle nullable DateOnly

                    // Converting Database record to Model object
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null  // Handle nullable DateTime
                );

                // Reservation Status conversion from Enum to string
                entity.Property(u => u.ReservationStatus)
                .HasConversion<string>();

                // Reservations-Book (M-1)
                entity.HasOne(b => b.Book)
                .WithMany(r => r.Reservations)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

                // Reservations-Users (M-1)
                entity.HasOne(u => u.Users)
                .WithMany(r => r.Reservations)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Users Model
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.AccountCreatedDate)
                .HasColumnType("date")
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v)
                );

                // Role conversion from Enum to string
                entity.Property(u => u.Role)
                .HasConversion<string>();
            });

            // LogUserActivity Model
            modelBuilder.Entity<LogUserActivity>(entity =>
            {
                entity.HasKey(l => l.LogId);

                // Action Type conversion from Enum to string
                entity.Property(u => u.ActionType)
               .HasConversion<string>();

                // LogUserActivity-Users (M-1)
                entity.HasOne(u => u.User)
                .WithMany(l => l.LogUserActivity)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configuration = configBuilder.GetSection("ConnectionStrings");
            var conStr = configuration["BookHubDBConnStr"] ?? null;

            optionsBuilder.UseSqlServer(conStr);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Books> Books { get; set; }

        public DbSet<Borrowed> Borrowed { get; set; } = default!;

        public DbSet<Fines> Fines { get; set; } = default!;

        public DbSet<Genres> Genres { get; set; } = default!;

        public DbSet<LogUserActivity> LogUserActivity { get; set; } = default!;

        public DbSet<Notifications> Notifications { get; set; } = default!;

        public DbSet<Reservations> Reservations { get; set; } = default!;

        public DbSet<Users> Users { get; set; } = default!;
    }
}
