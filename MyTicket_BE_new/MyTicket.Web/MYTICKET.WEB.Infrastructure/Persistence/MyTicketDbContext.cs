using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.InfrastructureBase.Persistence;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.ConstantVaribale.Db;
using MYTICKET.WEB.DOMAIN.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MYTICKET.WEB.Infrastructure.Persistence
{
    public partial class MyTicketDbContext : ApplicationDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Suppiler> Suppilers { get; set; }

        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<TicketEvent> TicketEvents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public MyTicketDbContext() : base()
        {
        }

        public MyTicketDbContext(DbContextOptions<MyTicketDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DbSchemas.Default);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasDefaultValue(UserStatus.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<RolePermission>()
                .HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Suppiler>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            #region Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(ActiveStatus.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<Event>()
                        .HasOne(events => events.EventType)
                        .WithMany(eventType => eventType.Events)
                        .HasForeignKey(e => e.EventTypeId);
            modelBuilder.Entity<Event>()
                        .HasOne(events => events.Suppiler)
                        .WithMany(suppiler => suppiler.Events)
                        .HasForeignKey(e => e.SupplierId);
            #endregion
            #region EventDetail
            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(ActiveStatus.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<EventDetail>()
            .HasOne(eventDetails => eventDetails.Event)
            .WithMany(events => events.EventDetails)
            .HasForeignKey(e => e.EventId);
            #endregion
            #region TicketEvent
            modelBuilder.Entity<TicketEvent>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(ActiveStatus.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<TicketEvent>()
            .HasOne(ticketEvent => ticketEvent.EventDetail)
            .WithMany(eventDetail => eventDetail.TicketEvents)
            .HasForeignKey(e => e.EventDetailId);

            modelBuilder.Entity<TicketEvent>()
                .HasMany(te => te.Tickets)
                .WithOne(t => t.TicketEvent)
                .HasForeignKey(t => t.TicketEventId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
            #region Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<Order>()
            .HasOne(order => order.Customer)
            .WithMany(customer => customer.Orders)
            .HasForeignKey(e => e.CustomerId);
            #endregion
            #region OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
            });
            modelBuilder.Entity<OrderDetail>()
            .HasOne(orderDetail => orderDetail.Order)
            .WithMany(order => order.OrderDetails)
            .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<OrderDetail>()
            .HasOne(orderDetail => orderDetail.EventDetail)
            .WithMany(eventDetail => eventDetail.OrderDetails)
            .HasForeignKey(e => e.EventDetailId);

            modelBuilder.Entity<OrderDetail>()
                        .HasOne(od => od.Ticket)
                        .WithMany(t => t.OrderDetails)
                        .HasForeignKey(e => e.TicketId);
            #endregion
            modelBuilder.SeedData();
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
