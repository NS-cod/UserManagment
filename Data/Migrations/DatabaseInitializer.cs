using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Data.Migrations
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

            // Crear la base de datos si no existe
            await context.Database.EnsureCreatedAsync();

            // Aplicar migraciones pendientes
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            // Seed inicial de datos si es necesario
            await SeedInitialDataAsync(context);
        }

        private static async Task SeedInitialDataAsync(UserManagementDbContext context)
        {
            // Verificar si ya hay usuarios (el seed data del DbContext ya creó roles y permisos)
            if (await context.Users.AnyAsync())
                return;

            // Crear usuario SuperAdmin por defecto
            var superAdminUser = new User
            {
                FirstName = "Super",
                LastName = "Administrator",
                Email = "superadmin@userservice.com",
                Username = "superadmin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!"),
                IsEmailVerified = true,
                CreatedBy = "System",
                CreatedTime = DateTime.UtcNow
            };

            context.Users.Add(superAdminUser);
            await context.SaveChangesAsync();

            // Obtener el rol SuperAdmin
            var superAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            if (superAdminRole != null)
            {
                // Asignar rol SuperAdmin al usuario
                var userRole = new UserRole
                {
                    UserId = superAdminUser.Id,
                    RoleId = superAdminRole.Id,
                    AssignedBy = "System",
                    CreatedTime = DateTime.UtcNow
                };

                context.UserRoles.Add(userRole);
                await context.SaveChangesAsync();
            }

            // Opcional: Crear usuario Admin de ejemplo
            var adminUser = new User
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@userservice.com",
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                IsEmailVerified = true,
                CreatedBy = "System",
                CreatedTime = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // Asignar rol Admin
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole != null)
            {
                var adminUserRole = new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id,
                    AssignedBy = "System",
                    CreatedTime = DateTime.UtcNow
                };

                context.UserRoles.Add(adminUserRole);
                await context.SaveChangesAsync();
            }
        }
    }
}