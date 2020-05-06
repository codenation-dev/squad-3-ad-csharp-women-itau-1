using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
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

        [Column("ID_EVENTS")]
        public int IdEvent { get; set; }

        [ForeignKey("ENVIRONMENT_ID"), Required]
        public int EnvironmentId { get; set; }

        [Column("ENVIRONMENT_ID"), Required]
        public Environment Environment { get; set; }

        [ForeignKey("LEVEL_ID"), Required]
        public int LevelId { get; set; }

        [Column("LEVEL_ID"), Required]
        public Level Level { get; set; }

        // Informacoes relevantes para mostrar no retorno

        public string LevelName { get; set; }

        public string EnvironmentName { get; set; }

    }
}
