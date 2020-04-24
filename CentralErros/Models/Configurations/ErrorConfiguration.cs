using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralErros.Models.Configurations
{
    public class ErrorConfiguration : IEntityTypeConfiguration<ErrorOccurrence>
    {
        public void Configure(EntityTypeBuilder<ErrorOccurrence> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.HasOne(x => x.User)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.UserId)
                   .HasConstraintName("FK_Error_User");

            builder.HasOne(x => x.Token)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.TokenUser)
                   .HasConstraintName("FK_Error_Token");

            builder.HasOne(x => x.Environment_Id)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.EnvironmentId)
                   .HasConstraintName("FK_Error_Environment_Id");

            builder.HasOne(x => x.Environment_Name)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.EnvironmentName)
                   .HasConstraintName("FK_Error_Environment_Name");

            builder.HasOne(x => x.Level_Id)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.LevelId)
                   .HasConstraintName("FK_Error_Level_Id");

            builder.HasOne(x => x.Level_Name)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.LevelName)
                   .HasConstraintName("FK_Error_Level_Name");
        }
    }

}
