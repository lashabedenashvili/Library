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
    public class UserMB : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.Property(x => x.Id).IsRequired();
            modelBuilder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Property(x => x.SurName).HasMaxLength(150).IsRequired();
            modelBuilder.Property(x => x.BirthDate).HasColumnType("date");
            modelBuilder.Property(x => x.PhoneNumber).HasMaxLength(25).IsRequired();
            modelBuilder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Property(x => x.IsActive).IsRequired();

            modelBuilder
                .HasMany(user => user.UserPasswordHistory)
                .WithOne(userPasswordHistory => userPasswordHistory.User)
                .HasForeignKey(userPasswordHistory => userPasswordHistory.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
