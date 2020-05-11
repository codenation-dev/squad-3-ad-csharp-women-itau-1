using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralErros.Models
{
    [Table("ENVIRONMENT")]
    public class Environment
    {

        [Column("ID")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("NAME")]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<ErrorOccurrence> ErrorOccurrences { get; set; }
    }
}