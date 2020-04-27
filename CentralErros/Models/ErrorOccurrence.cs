using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CentralErros.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CentralErros.Models
{
    [Table("ERROR_OCURRENCE")]
    public class ErrorOccurrence
    {
        [Column("ID"), Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("TITLE"), Required]
        public string Title { get; set; }

        [Column("REGISTRATION_DATE"), Required]
        public DateTime RegistrationDate { get; set; }

        [Column("ORIGIN"), Required]
        public string Origin { get; set; }

        [Column("FILED"), Required]
        public bool Filed { get; set; }

        [Column("DETAILS"), Required]
        public string Details { get; set; }

        [Column("USER_ID"), Required]
        public int UserId { get; set; }
        [ForeignKey("UserId"), Required]
        public virtual User User { get; set; }

        [Column("TOKEN_USER"), Required]
        public string TokenUser { get; set; }
        [ForeignKey("TokenUser"), Required]
        public virtual User Token { get; set; }

        [Column("ENVIRONMENT_ID"), Required]
        public int EnvironmentId { get; set; }
        [ForeignKey("EnvironmentId"), Required]
        public virtual Api.Models.Environment Environment_Id { get; set; }

        [Column("ENVIRONMENT_NAME"), Required]
        public string EnvironmentName { get; set; }
        [ForeignKey("EnvironmentName"), Required]
        public virtual Api.Models.Environment Environment_Name { get; set; }

        [Column("LEVEL_ID"), Required]
        public int LevelId { get; set; }
        [ForeignKey("LevelId"), Required]
        public virtual Level Level_Id { get; set; }

        [Column("LEVEL_NAME"), Required]
        public string LevelName { get; set; }
        [ForeignKey("LevelName"), Required]
        public virtual Level Level_Name { get; set; }
    }
}
