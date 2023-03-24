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
    public class UserPasswordHistoryMB : IEntityTypeConfiguration<UserPasswordHistory>
    {
        public void Configure(EntityTypeBuilder<UserPasswordHistory> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired();
            modelBuilder.Property(x => x.PasswordHash).IsRequired();
            modelBuilder.Property(x => x.PasswordSalt).IsRequired();
            modelBuilder.Property(x => x.CreateTime).HasColumnType("date").IsRequired();
            modelBuilder.Property(x => x.IsActive).IsRequired();
            modelBuilder.Property(x => x.UpdateTime).HasColumnType("date");
        }
    }
}
