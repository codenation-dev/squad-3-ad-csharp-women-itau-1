using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.Models
{
    [Table("LEVEL")]
    public class Level
    {
        [Column("ID")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdLevel { get; set; }

        [Column("LEVEL")]
        [StringLength(50)]
        [Required]
        public string LevelName { get; set; }

        public virtual ICollection<ErrorOccurrence> ErrorOccurrences { get; set; }
    }
}
