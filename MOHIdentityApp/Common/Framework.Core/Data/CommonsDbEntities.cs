using Framework.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Framework.Core.Data
{
    public class CommonsDbEntities : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer(@"Server=AHEJAZI-PC\MSSQLSERVER2017;Database=MOHWSO2;User Id=sa; Password=sa@123;MultipleActiveResultSets=true")
                .UseLoggerFactory(new LoggerFactory().AddConsole());
        }

        /// <summary>
        /// Gets or sets the activation codes.
        /// </summary>
        public virtual DbSet<EventLog> EventLog { get; set; }

        /// <summary>
        /// Gets or sets the system settings.
        /// </summary>
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLog>().ToTable("EventLog");
            
            modelBuilder.HasDefaultSchema("common").Entity<SystemSetting>().ToTable("SystemSettings");
            modelBuilder.HasDefaultSchema("common").Entity<AttachmentType>().ToTable("AttachmentTypes");
        }

    }

}
