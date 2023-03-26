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
    public class Context: DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistory { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<BooksAuthors> BooksAuthors { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMB());
            modelBuilder.ApplyConfiguration(new AuthorMB());
            modelBuilder.ApplyConfiguration(new UserPasswordHistoryMB());
            modelBuilder.ApplyConfiguration(new BookMB());
            modelBuilder.ApplyConfiguration(new BooksAuthorsMB());
        }
    }
}
