using Microsoft.EntityFrameworkCore;
using SubscriptionManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Infrastructure.DataContext
{
    public partial class SubscriptionManagementDataContext : DbContext
    {
        public SubscriptionManagementDataContext()
        {
        }
        public static string Connectionstring { get; set; }
        public SubscriptionManagementDataContext(DbContextOptions<SubscriptionManagementDataContext> options)
           : base(options)
        {
        }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ActiveServiceLog> ActiveServiceLogs { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(Connectionstring);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Subscriber>(entity =>
            //{
            //    entity.Property(e => e.Id).HasColumnName("ID");
            //    entity.Property(e => e.Password).HasMaxLength(50);
            //    entity.Property(e => e.PlaceId).HasColumnName("PlaceID");
            //    entity.Property(e => e.Type).HasMaxLength(50);
            //    entity.Property(e => e.UserName).HasMaxLength(50);

            //    entity.HasOne(d => d.Place).WithMany(p => p.Accounts)
            //        .HasForeignKey(d => d.PlaceId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Accounts_Place");
            //});
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
