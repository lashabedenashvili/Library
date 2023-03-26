using Library.Data.Domein.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Domein.Domein.EntityModelBuilders
{
    public class BookMB : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired();
            modelBuilder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            modelBuilder.Property(x => x.Description).HasMaxLength(500).IsRequired();
            modelBuilder.Property(x => x.InLibrary).IsRequired();

            modelBuilder
               .HasMany(book => book.BooksAuthors)
               .WithOne(booksAuthors => booksAuthors.Book)
               .HasForeignKey(book => book.BookId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
