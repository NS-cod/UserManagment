using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                      .IsRequired();

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.CreatedTime)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                entity.Property(e => e.IsEmailVerified)
                      .HasDefaultValue(false);

                // Índices únicos
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Email");

                entity.HasIndex(e => e.Username)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Username");

                // Índices compuestos para mejor rendimiento
                entity.HasIndex(e => new { e.IsActive, e.Username })
                      .HasDatabaseName("IX_Users_IsActive_Username");

                entity.HasIndex(e => new { e.IsActive, e.Email })
                      .HasDatabaseName("IX_Users_IsActive_Email");

                // Nombre de tabla
                entity.ToTable("Users");
            });

            // Configuración Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.CreatedTime)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                entity.Property(e => e.IsSystemRole)
                      .HasDefaultValue(false);

                // Índices
                entity.HasIndex(e => e.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_Roles_Name");

                entity.HasIndex(e => new { e.IsActive, e.Name })
                      .HasDatabaseName("IX_Roles_IsActive_Name");

                entity.ToTable("Roles");
            });

            // Configuración Permission
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Resource)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Action)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.CreatedTime)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                entity.Property(e => e.IsSystemPermission)
                      .HasDefaultValue(false);

                // Índices
                entity.HasIndex(e => new { e.Resource, e.Action })
                      .IsUnique()
                      .HasDatabaseName("IX_Permissions_Resource_Action");

                entity.HasIndex(e => new { e.IsActive, e.Resource, e.Action })
                      .HasDatabaseName("IX_Permissions_IsActive_Resource_Action");

                entity.ToTable("Permissions");
            });

            // Configuración UserRole (muchos a muchos)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.AssignedBy)
                      .HasMaxLength(100);

                entity.Property(e => e.CreatedTime)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                // Índices
                entity.HasIndex(e => new { e.UserId, e.RoleId })
                      .IsUnique()
                      .HasDatabaseName("IX_UserRoles_UserId_RoleId");

                entity.HasIndex(e => new { e.IsActive, e.UserId })
                      .HasDatabaseName("IX_UserRoles_IsActive_UserId");

                entity.HasIndex(e => new { e.IsActive, e.RoleId })
                      .HasDatabaseName("IX_UserRoles_IsActive_RoleId");

                // Relaciones
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("UserRoles");
            });

            // Configuración RolePermission (muchos a muchos)
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedTime)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                // Índices
                entity.HasIndex(e => new { e.RoleId, e.PermissionId })
                      .IsUnique()
                      .HasDatabaseName("IX_RolePermissions_RoleId_PermissionId");

                entity.HasIndex(e => new { e.IsActive, e.RoleId })
                      .HasDatabaseName("IX_RolePermissions_IsActive_RoleId");

                entity.HasIndex(e => new { e.IsActive, e.PermissionId })
                      .HasDatabaseName("IX_RolePermissions_IsActive_PermissionId");

                // Relaciones
                entity.HasOne(e => e.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("RolePermissions");
            });

            // Datos iniciales (Seed Data)
            SeedData(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseModel)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedTime = DateTime.UtcNow;
                    // En una implementación real, obtendrías el usuario actual del contexto de seguridad
                    if (string.IsNullOrEmpty(entity.CreatedBy))
                        entity.CreatedBy = "System";
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedTime = DateTime.UtcNow;
                    // En una implementación real, obtendrías el usuario actual del contexto de seguridad
                    entity.ModifiedBy = "System";

                    // Evitar que se modifiquen los campos de creación
                    entry.Property(nameof(BaseModel.CreatedTime)).IsModified = false;
                    entry.Property(nameof(BaseModel.CreatedBy)).IsModified = false;
                }
            }
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Roles iniciales
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", Description = "Administrador del sistema con acceso completo", IsSystemRole = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Role { Id = 2, Name = "Admin", Description = "Administrador con permisos limitados", IsSystemRole = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Role { Id = 3, Name = "User", Description = "Usuario estándar", IsSystemRole = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" }
            );

            // Permisos iniciales
            var permissions = new[]
            {
                // User permissions
                new Permission { Id = 1, Name = "CreateUser", Resource = "User", Action = "Create", Description = "Crear usuarios", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 2, Name = "ReadUser", Resource = "User", Action = "Read", Description = "Ver usuarios", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 3, Name = "UpdateUser", Resource = "User", Action = "Update", Description = "Actualizar usuarios", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 4, Name = "DeleteUser", Resource = "User", Action = "Delete", Description = "Eliminar usuarios", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                
                // Role permissions
                new Permission { Id = 5, Name = "CreateRole", Resource = "Role", Action = "Create", Description = "Crear roles", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 6, Name = "ReadRole", Resource = "Role", Action = "Read", Description = "Ver roles", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 7, Name = "UpdateRole", Resource = "Role", Action = "Update", Description = "Actualizar roles", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 8, Name = "DeleteRole", Resource = "Role", Action = "Delete", Description = "Eliminar roles", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                
                // Permission permissions
                new Permission { Id = 9, Name = "CreatePermission", Resource = "Permission", Action = "Create", Description = "Crear permisos", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 10, Name = "ReadPermission", Resource = "Permission", Action = "Read", Description = "Ver permisos", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 11, Name = "UpdatePermission", Resource = "Permission", Action = "Update", Description = "Actualizar permisos", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 12, Name = "DeletePermission", Resource = "Permission", Action = "Delete", Description = "Eliminar permisos", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },

                // Dashboard permissions
                new Permission { Id = 13, Name = "ViewDashboard", Resource = "Dashboard", Action = "View", Description = "Ver dashboard", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" },
                new Permission { Id = 14, Name = "ViewReports", Resource = "Reports", Action = "View", Description = "Ver reportes", IsSystemPermission = true, CreatedTime = DateTime.UtcNow, CreatedBy = "System" }
            };

            modelBuilder.Entity<Permission>().HasData(permissions);

            // Asignar todos los permisos al SuperAdmin
            var superAdminPermissions = permissions.Select((p, index) => new RolePermission
            {
                Id = index + 1,
                RoleId = 1, // SuperAdmin
                PermissionId = p.Id,
                CreatedTime = DateTime.UtcNow
            }).ToArray();

            modelBuilder.Entity<RolePermission>().HasData(superAdminPermissions);

            // Asignar permisos básicos al Admin
            var adminPermissions = new[]
            {
                new RolePermission { Id = 15, RoleId = 2, PermissionId = 2, CreatedTime = DateTime.UtcNow }, // ReadUser
                new RolePermission { Id = 16, RoleId = 2, PermissionId = 3, CreatedTime = DateTime.UtcNow }, // UpdateUser
                new RolePermission { Id = 17, RoleId = 2, PermissionId = 6, CreatedTime = DateTime.UtcNow }, // ReadRole
                new RolePermission { Id = 18, RoleId = 2, PermissionId = 10, CreatedTime = DateTime.UtcNow }, // ReadPermission
                new RolePermission { Id = 19, RoleId = 2, PermissionId = 13, CreatedTime = DateTime.UtcNow }, // ViewDashboard
                new RolePermission { Id = 20, RoleId = 2, PermissionId = 14, CreatedTime = DateTime.UtcNow }  // ViewReports
            };

            modelBuilder.Entity<RolePermission>().HasData(adminPermissions);

            // Asignar permisos mínimos al User
            var userPermissions = new[]
            {
                new RolePermission { Id = 21, RoleId = 3, PermissionId = 2, CreatedTime = DateTime.UtcNow }, // ReadUser (solo su propio perfil)
                new RolePermission { Id = 22, RoleId = 3, PermissionId = 13, CreatedTime = DateTime.UtcNow } // ViewDashboard
            };

            modelBuilder.Entity<RolePermission>().HasData(userPermissions);
        }
    }
}