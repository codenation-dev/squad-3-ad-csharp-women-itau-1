using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralErros.Api.Models
{
    [Table("USER")]
    public class User
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

        [Column("EMAIL")]
        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        [Column("PASSWORD")]
        [StringLength(255)]
        [Required]
        public string Password { get; set; }

        [Column("TOKEN")]
        [Required]
        public string Token { get; internal set; }

        [Column("EXPIRATION")]
        [Required]
        public DateTime Expiration { get; internal set; }
    }
}