using Microsoft.EntityFrameworkCore;
using Connection.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace Connection
{
    public class AppDbContext : DbContext
    {
        public DbSet<PermissionEntity> permissionsEntities { get; set; }
        public DbSet<PermissionsTypeEntity> permissionsTypeEntities { get; set; }

        private readonly Action<EntityTypeBuilder<PermissionEntity>> _PermissionsBuilder = PermissionEntity.PermissionBuilder;
        private readonly Action<EntityTypeBuilder<PermissionsTypeEntity>> _PermissionsTypeEntityBuilder = PermissionsTypeEntity.PermissionsTypeEntityBuilder;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 

        }      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionEntity>(_PermissionsBuilder);
            modelBuilder.Entity<PermissionsTypeEntity>(_PermissionsTypeEntityBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<PermissionsTypeEntity>()
                        .HasMany(e => e.Permissions)
                        .WithOne(e => e.PermissionsTypeEntity)
                        .HasForeignKey(e => e.PermissionsType)
                        .HasPrincipalKey(e => e.Id);
        }        
    }
}
