using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Micro.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext():base("BookManagement")
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleMapping> RoleMappings { get; set; }
    }
}