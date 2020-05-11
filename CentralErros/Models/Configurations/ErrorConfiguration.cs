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

            builder.HasOne(x => x.Environment)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.EnvironmentId)
                   .HasConstraintName("FK_Error_Environment_Id");


            builder.HasOne(x => x.Level)
                   .WithMany(y => y.ErrorOccurrences)
                   .HasForeignKey(x => x.LevelId)
                   .HasConstraintName("FK_Error_Level_Id");
        }
    }

}
