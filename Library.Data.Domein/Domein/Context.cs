using Library.Data.Domein.Data;
using Library.Data.Domein.Domein.EntityModelBuilders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Domein.Domein
{
    public class Context: DbContext,IContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistory { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMB());
        }
    }
}
