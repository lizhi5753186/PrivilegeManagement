using System.Data.Entity;
using LH.Domain.Model;

namespace LH.Repository.DBContext
{
    public class UserManagerDBContext : DbContext
    {
        static UserManagerDBContext()
        {
            Database.SetInitializer<UserManagerDBContext>(null);
        }

        public UserManagerDBContext()
            : base("PermissionManagement")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}