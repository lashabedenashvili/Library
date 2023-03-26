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
    public class AuthorMB : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired();
            modelBuilder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Property(x => x.Surname).HasMaxLength(150).IsRequired();
            modelBuilder.Property(x => x.BirthDate).HasColumnType("date");

            modelBuilder
               .HasMany(author => author.BooksAuthors)
               .WithOne(booksAuthors => booksAuthors.Author)
               .HasForeignKey(author => author.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
