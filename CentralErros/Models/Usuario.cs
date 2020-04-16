using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralErros.Api.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        //1 para N
        //1 para 1
        [Column("Id")]
        [Required]
        [Key]
        public int Id { get; set; }

        [Column("Name")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Column("Email")]
        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        [Column("Password")]
        [StringLength(255)]
        [Required]
        public string Password { get; set; }

    }
}