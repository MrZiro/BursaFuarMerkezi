using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            // Ensure database exists and apply migrations if present
            var pendingMigrations = _db.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _db.Database.Migrate();
            }
            else
            {
                _db.Database.EnsureCreated();
            }

            // Seed roles and admin user idempotently
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            }
            if (!_roleManager.RoleExistsAsync(SD.Role_Editor).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Editor)).GetAwaiter().GetResult();
            }

            var adminEmail = "admin@BFM.com";
            var existingUser = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            if (existingUser == null)
            {
                var createResult = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "BFM"
                }, "Admin123*").GetAwaiter().GetResult();

                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create seed admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }

                existingUser = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            }

            if (existingUser != null && !_userManager.IsInRoleAsync(existingUser, SD.Role_Admin).GetAwaiter().GetResult())
            {
                _userManager.AddToRoleAsync(existingUser, SD.Role_Admin).GetAwaiter().GetResult();
            }
        }
    }
}
