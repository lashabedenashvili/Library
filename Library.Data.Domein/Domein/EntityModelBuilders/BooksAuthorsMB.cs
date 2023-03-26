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
    public class BooksAuthorsMB : IEntityTypeConfiguration<BooksAuthors>
    {
        public void Configure(EntityTypeBuilder<BooksAuthors> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired();
            modelBuilder.Property(x => x.BookId).IsRequired();
            modelBuilder.Property(x => x.AuthorId).IsRequired();
        }
    }
}
